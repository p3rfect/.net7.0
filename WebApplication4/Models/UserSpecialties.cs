using System.Text.Json.Serialization;

namespace WebApplication4.Models
{
    public class UserSpecialties
    {
        [JsonPropertyName("FinancingFormPeriod")]
        public string FinancingFormPeriod { get; set; } = "";   // format: "financing,form,period" ("бюджет,дневная,полная")

        [JsonPropertyName("SpecialtiesCodes")]
        public List<string> SpecialtiesCodes { get; set; } = new();
    }
}
