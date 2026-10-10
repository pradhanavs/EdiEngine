using EdiEngine.Common.Definitions;
using EdiEngine.Xml;
using System.Text.Json.Serialization;

namespace EdiEngine.Runtime
{
    [XmlElement(IgnoreElementRoot = true)]
    public class EdiSimpleDataElement : DataElementBase
    {
        public EdiSimpleDataElement(MapBaseDataElement definition, string value) : base(definition)
        {
            if (definition != null)
            {
                Type = definition.GetType().Name;
            }
            Val = value;
        }
        [JsonIgnore]
        [XmlIgnore]
        public new MapSimpleDataElement Definition => (MapSimpleDataElement)base.Definition;

        [JsonIgnore]
        [XmlIgnore]
        public string Type { get; }

        [JsonPropertyName("E")]
        [JsonPropertyOrder(1)]
        [XmlProperty(Order = 1, PropertyName = "E")]
        public override string Val { get; }
    }
}
