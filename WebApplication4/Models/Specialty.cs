using System.Text.Json.Serialization;

namespace WebApplication4.Models
{
    public class Specialty
    {
        
        [JsonPropertyName("FinancingFormPeriod")]
        public List<string> FinancingFormPeriod { get; set; } = new(); // format: "financing,form,period" ("бюджет,дневная,полная")

        [JsonPropertyName("SpecialtyFacultyAndName")]
        public string SpecialtyFacultyAndName { get; set; } = ""; // format: "Faculty.Name" ("ФКСиС.ИиТП")

        [JsonPropertyName("IsPhysics")]
        public bool IsPhysics { get; set; } = true;

        [JsonPropertyName("SpecialtyCode")]
        public string SpecialtyCode { get; set; }="";
    }
}
