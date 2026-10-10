using EdiEngine.Common.Definitions;
using EdiEngine.Xml;
using System.Text.Json.Serialization;

namespace EdiEngine.Runtime
{
    [JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor)]
    [JsonDerivedType(typeof(EdiSimpleDataElement))]
    [JsonDerivedType(typeof(EdiCompositeDataElement))]
    public abstract class DataElementBase
    {
        protected DataElementBase(MapBaseDataElement definition)
        {
            Definition = definition;
        }

        [JsonIgnore]
        [XmlIgnore]
        public MapBaseDataElement Definition { get; }

        [JsonIgnore]
        public abstract string Val { get;}

        public override string ToString()
        {
            return Val ?? string.Empty;
        }
    }
}
