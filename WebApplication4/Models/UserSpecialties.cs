using System.Text.Json.Serialization;

namespace WebApplication4.Models
{
    public class UserSpecialties
    {
        public string FinancingFormPeriod { get; set; } = "";   // format: "financing,form,period" ("бюджет,дневная,полная")

        public List<string> SpecialtiesCodes { get; set; } = new();
    }
}
