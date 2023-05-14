using System.Text.Json.Serialization;

namespace WebApplication4.Models
{
    public class Exams
    {
        public bool IsRussian { get; set; } = true;
        public bool IsPhysics { get; set; } = true;
        public string LanguageExam { get; set; } = "";
        public string MathExam { get; set; } = "";
        public string PhysicsExam { get; set; } = "";
        public int LanguageScore { get; set; } = 0;
        public int MathScore { get; set; } = 0;
        public int PhysicsScore { get; set; } = 0;
        public int LanguageMark { get; set; } = 0;
        public int MathMark { get; set; } = 0;
        public int PhysicsMark { get; set; } = 0;
    }
}
