using EdiEngine.Common.Definitions;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using EdiEngine.Validation;
using EdiEngine.Xml;

namespace EdiEngine.Runtime
{
    public class EdiTrans : EdiLoop, IValidatedEntity
    {
        public EdiTrans()
        {
            ValidationErrors = new List<ValidationError>();
        }

        public EdiTrans(MapBaseEntity definition) : base(definition, null)
        {
            ValidationErrors = new List<ValidationError>();
        }

        [JsonPropertyOrder(0)]
        [XmlIgnore]
        public override string Type => "M";

        [JsonPropertyOrder(-1)]
        [XmlProperty(Order = 2)]
        public EdiSegment ST { get; set; }

        [JsonPropertyOrder(-1)]
        [XmlProperty(Order = 3)]
        public EdiSegment SE { get; set; }

        [JsonPropertyOrder(100)]
        [XmlProperty(Order = 100)]
        public virtual List<ValidationError> ValidationErrors { get; }
    }
}
