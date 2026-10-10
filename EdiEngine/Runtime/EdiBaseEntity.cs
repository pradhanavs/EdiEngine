using EdiEngine.Common.Definitions;
using EdiEngine.Xml;
using System.Text.Json.Serialization;

namespace EdiEngine.Runtime
{
    [JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor)]
    [JsonDerivedType(typeof(EdiLoop))]
    [JsonDerivedType(typeof(EdiTrans))]
    [JsonDerivedType(typeof(EdiHlLoop))]
    [JsonDerivedType(typeof(EdiSegment))]
    public abstract class EdiBaseEntity
    {
        protected EdiBaseEntity()
        {

        }

        protected EdiBaseEntity(MapBaseEntity definition)
        {
            if (definition != null)
            {
                Name = definition.Name;
                Definition = definition;
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        public MapBaseEntity Definition { get; set; }

        [JsonPropertyOrder(1)]
        [XmlProperty(Order = 1)]
        public string Name { get; set; }

        [JsonPropertyOrder(0)]
        [XmlProperty(Order = 0)]
        public abstract string Type { get; }
        }
}
