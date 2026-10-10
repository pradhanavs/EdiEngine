using EdiEngine.Common.Definitions;
using EdiEngine.Runtime;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace EdiEngine
{
    public class JsonMapReader : InetermediateFormatReader
    {
        public JsonMapReader(MapLoop map) : base(map)
        {
        }

        protected override EdiIntermediateEntity ReadIntermediateTree(string rawData)
        {
            Utf8JsonReader reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(rawData), new JsonReaderOptions
            {
                CommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            });

            EdiIntermediateEntity context = new EdiIntermediateEntity(null);
            PropertyInfo prop = null;

            while (reader.Read())
            {
                string val;
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartObject:
                        var ent = new EdiIntermediateEntity(context);
                        context?.Children.Add(ent);
                        context = ent;
                        break;

                    case JsonTokenType.PropertyName:
                        val = reader.GetString();
                        prop = typeof(EdiIntermediateEntity).GetProperty(val);
                        break;

                    case JsonTokenType.String:
                        val = reader.GetString();
                        prop?.SetValue(context, val);
                        break;

                    case JsonTokenType.EndObject:
                        context = context?.Parent;
                        break;
                }
            }
            return context?.Children[0];
        }
    }
}
