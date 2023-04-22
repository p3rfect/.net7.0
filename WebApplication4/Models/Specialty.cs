using System.Text.Json.Serialization;

namespace WebApplication4.Models
{
    public class Specialty
    {
        [JsonPropertyName("FinancingFormPeriod")]
        public List<string> FinancingFormPeriod = new();

        [JsonPropertyName("SpecialtyFacultyAndName")]
        public string SpecialtyFacultyAndName { get; set; } = "";

        [JsonPropertyName("IsPhysics")]
        public bool IsPhysics { get; set; } = true;
    }
}
