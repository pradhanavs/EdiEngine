using EdiEngine.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text.Json;
using System.Linq;
using EdiEngine.Standards.X12_004010.Maps;
using M_940 = EdiEngine.Standards.X12_004010.Maps.M_940;
using EdiEngine.Tests.Maps;

namespace EdiEngine.Tests
{
    [TestClass]
    public class JsonReadWriteTests
    {
        [TestMethod]
        public void JsonReadWrite_JsonSerializationTest()
        {
            using (Stream s = GetType().Assembly.GetManifestResourceStream("EdiEngine.Tests.TestData.940.OK.edi"))
            {
                EdiDataReader r = new EdiDataReader();
                EdiBatch b = r.FromStream(s);

                //Write Json using System.Text.Json
                //check no exception
                JsonSerializer.Serialize(b);
                JsonSerializer.Serialize(b.Interchanges[0].Groups[0].Transactions[0]);

                //or use writer to write to string or stream
                JsonDataWriter w  = new JsonDataWriter();
                string str = w.WriteToString(b);
                Stream stream = w.WriteToStream(b);

                Assert.IsNotNull(str);

                Assert.IsNotNull(stream);
                Assert.AreEqual(0, stream.Position);
                Assert.IsTrue(stream.CanRead);

                Assert.AreEqual(str.Length, stream.Length);
            }
        }

        [TestMethod]
        public void JsonReadWrite_DeserializeJsonOK()
        {
            string json = TestUtils.ReadResourceStream("EdiEngine.Tests.TestData.940.OK.json");

            M_940 map = new M_940();
            JsonMapReader r = new JsonMapReader(map);

            EdiTrans t = r.ReadToEnd(json);

            Assert.AreEqual(0, t.ValidationErrors.Count);
        }


        [TestMethod]
        public void JsonReadWrite_DeserializeJsonWithValidationErrors()
        {
            string json = TestUtils.ReadResourceStream("EdiEngine.Tests.TestData.940.ERR.json");

            M_940 map = new M_940();
            JsonMapReader r = new JsonMapReader(map);

            EdiTrans t = r.ReadToEnd(json);

            Assert.AreEqual(2, t.ValidationErrors.Count);
        }

        [TestMethod]
        public void JsonReadWrite_JsonSerializationHlLoopTest()
        {
            using (Stream s = GetType().Assembly.GetManifestResourceStream("EdiEngine.Tests.TestData.856.Crossdock.OK.edi"))
            {
                EdiDataReader r = new EdiDataReader();
                EdiBatch b = r.FromStream(s);

                JsonDataWriter jsonWriter = new JsonDataWriter();
                jsonWriter.WriteToString(b);
            }
        }

        [TestMethod]
        public void JsonReadWrite_DeserializeJsonHlLoopOk()
        {
            string json = TestUtils.ReadResourceStream("EdiEngine.Tests.TestData.856.Crossdock.OK.json");

            M_856 map = new M_856();
            JsonMapReader r = new JsonMapReader(map);

            EdiTrans t = r.ReadToEnd(json);

            Assert.AreEqual(0, t.ValidationErrors.Count);

            //string edi = TestUtils.WriteEdiEnvelope(t, "SH");
        }

        [TestMethod]
        public void JsonReadWrite_SerializeComposite()
        {
            using (Stream s = GetType().Assembly.GetManifestResourceStream("EdiEngine.Tests.TestData.850.Composite.SLN.OK.edi"))
            {
                EdiDataReader r = new EdiDataReader();
                EdiBatch b = r.FromStream(s);

                JsonDataWriter jsonWriter = new JsonDataWriter();
                jsonWriter.WriteToString(b);
            }
        }

        [TestMethod]
        public void JsonReadWrite_DeserializeComposite()
        {
            string json = TestUtils.ReadResourceStream("EdiEngine.Tests.TestData.001.Fake.Composite.json");

            M_001 map = new M_001();
            JsonMapReader r = new JsonMapReader(map);

            EdiTrans t = r.ReadToEnd(json);

            Assert.AreEqual(0, t.ValidationErrors.Count);

            var sln = (EdiSegment)t.Content.First();
            Assert.IsTrue(sln.Content[4] is EdiCompositeDataElement);
            Assert.AreEqual(6, ((EdiCompositeDataElement)sln.Content[4]).Content.Count);

        }

        [TestMethod]
        [DataRow("940.OK")]
        [DataRow("940.2.OK")]
        [DataRow("850.OK")]
        [DataRow("850.Composite.SLN.OK")]
        [DataRow("856.Crossdock.OK")]
        [DataRow("850.SyntaxNotes.ERR")]
        [DataRow("MultipleInterchangesAndGroups")]
        [DataRow("AckTest")]
        public void JsonReadWrite_OutputMatchesBaseline(string name)
        {
            //Baseline files were produced by the Newtonsoft.Json based writer
            string expected = TestUtils.ReadResourceStream($"EdiEngine.Tests.TestData.Baseline.{name}.json");

            using (Stream s = GetType().Assembly.GetManifestResourceStream($"EdiEngine.Tests.TestData.{name}.edi"))
            {
                EdiDataReader r = new EdiDataReader();
                EdiBatch b = r.FromStream(s);
                b.Date = new System.DateTime(2020, 1, 2, 3, 4, 5);

                JsonDataWriter w = new JsonDataWriter();
                Assert.AreEqual(expected, w.WriteToString(b));

                using (StreamReader sr = new StreamReader(w.WriteToStream(b)))
                {
                    Assert.AreEqual(expected, sr.ReadToEnd());
                }
            }
        }
    }
}
