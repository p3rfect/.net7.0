using System.Text.Json.Serialization;

namespace WebApplication4.Models
{
    public class Exams
    {
        [JsonPropertyName("IsRussian")]
        public bool IsRussian { get; set; } = true;

        [JsonPropertyName("IsPhysics")]
        public bool IsPhysics { get; set; } = true;

        [JsonPropertyName("LanguageExam")]
        public string LanguageExam { get; set; } = "";

        [JsonPropertyName("MathExam")]
        public string MathExam { get; set; } = "";

        [JsonPropertyName("SpecialtyExam")]
        public string PhysicsExam { get; set; } = "";

        [JsonPropertyName("LanguageScore")]
        public int LanguageScore { get; set; } = 0;

        [JsonPropertyName("MathScore")]
        public int MathScore { get; set; } = 0;

        [JsonPropertyName("SpecialtyMark")]
        public int PhysicsMark { get; set; } = 0;

        [JsonPropertyName("LanguageMark")]
        public int LanguageMark { get; set; } = 0;

        [JsonPropertyName("MathMark")]
        public int MathMark { get; set; } = 0;

        [JsonPropertyName("SpecialtyName")]
        public int PhysicsScore { get; set; } = 0;
    }
}
