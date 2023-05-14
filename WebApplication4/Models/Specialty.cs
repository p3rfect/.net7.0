using System.Text.Json.Serialization;

namespace WebApplication4.Models
{
    public class Specialty
    {
        public List<string> FinancingFormPeriod { get; set; } = new(); // format: "financing,form,period" ("бюджет,дневная,полная")
        public string SpecialtyFacultyAndName { get; set; } = ""; // format: "Faculty.Name" ("ФКСиС.ИиТП")
        public bool IsPhysics { get; set; } = true;
        public string SpecialtyCode { get; set; } = "";
    }
}
