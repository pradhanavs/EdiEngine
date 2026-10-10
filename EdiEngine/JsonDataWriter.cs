using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using EdiEngine.Runtime;

namespace EdiEngine
{
    public class JsonDataWriter : DataWriter
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            // keep EDI separators such as '>' unescaped
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public override Stream WriteToStream(EdiBatch batch)
        {
            MemoryStream s = new MemoryStream();
            JsonSerializer.Serialize(s, batch, SerializerOptions);

            s.Position = 0;
            return s;
        }

        public override string WriteToString(EdiBatch batch)
        {
            return JsonSerializer.Serialize(batch, SerializerOptions);
        }
    }
}
