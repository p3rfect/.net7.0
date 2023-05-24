using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApplication4.Models
{
    public class UserInfo
    {
        public string Lastname { get; set; } = "";
        public string LastnameLat { get; set; } = "";
        public string Firstname { get; set; } = "";
        public string FirstnameLat { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Birthday { get; set; } = "";
        public bool IsMale { get; set; } = true;
        public bool IsSingle { get; set; } = true;
        public string DocumentType { get; set; } = "";
        public string IdentyNumber { get; set; } = "";
        public string Series { get; set; } = "";
        public string Number { get; set; } = "";
        public string DateOfIssue { get; set; } = "";
        public string Validity { get; set; } = "";
        public string IssuedBy { get; set; } = "";
        public string Education { get; set; } = "";
        public string InstitutionType { get; set; } = "";
        public string Document { get; set; } = "";
        public string Institution { get; set; } = "";
        public string DocumentNumber { get; set; } = "";
        public string GraduationDate { get; set; } = "";
        public string Language { get; set; } = "";
        public int AverageScore { get; set; } = 0;
        public string PostalCode { get; set; } = "";
        public string Country { get; set; } = "";
        public string Region { get; set; } = "";
        public string District { get; set; } = "";
        public string LocalityType { get; set; } = "";
        public string LocalityName { get; set; } = "";
        public string StreetType { get; set; } = "";
        public string Street { get; set; } = "";
        public string HouseNumber { get; set; } = "";
        public string HousingNumber { get; set; } = "";
        public string FlatNumber { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Benefits { get; set; } = "";
        public string FatherType { get; set; } = "";
        public string FatherLastname { get; set; } = "";
        public string FatherFirstname { get; set; } = "";
        public string FatherSurname { get; set; } = "";
        public string FatherAddress { get; set; } = "";
        public string MotherType { get; set; } = "";
        public string MotherLastname { get; set; } = "";
        public string MotherFirstname { get; set; } = "";
        public string MotherSurname { get; set; } = "";
        public string MotherAddress { get; set; } = "";
    }
}   