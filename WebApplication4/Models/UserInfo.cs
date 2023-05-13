using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApplication4.Models
{
    public class UserInfo
    {
        [JsonPropertyName("Lastname")]
        public string Lastname { get; set; } = "";

        [JsonPropertyName("LastnameLat")]
        public string LastnameLat { get; set; } = "";

        [JsonPropertyName("Firstname")]
        public string Firstname { get; set; } = "";

        [JsonPropertyName("Firstnamelat")]
        public string Firstnamelat { get; set; } = "";

        [JsonPropertyName("Surname")]
        public string Surname { get; set; } = "";

        [JsonPropertyName("Birthday")]
        public string Birthday { get; set; } = "";

        [JsonPropertyName("IsMale")]
        public bool IsMale { get; set; } = true;

        [JsonPropertyName("IsSingle")]
        public bool IsSingle { get; set; } = true;


        [JsonPropertyName("DocumentType")]
        public string DocumentType { get; set; } = "";

        [JsonPropertyName("IdentyNumber")]
        public string IdentyNumber { get; set; } = "";

        [JsonPropertyName("Series")]
        public string Series { get; set; } = "";

        [JsonPropertyName("Number")]
        public string Number { get; set; } = "";

        [JsonPropertyName("DateOfIssue")]
        public string DateOfIssue { get; set; } = "";

        [JsonPropertyName("Validity")]
        public string Validity { get; set; } = "";

        [JsonPropertyName("IssuedBy")]
        public string IssuedBy { get; set; } = "";

        [JsonPropertyName("Education")]
        public string Education { get; set; } = "";

        [JsonPropertyName("InstitutionType")]
        public string InstitutionType { get; set; } = "";

        [JsonPropertyName("Document")]
        public string Document { get; set; } = "";

        [JsonPropertyName("Institution")]
        public string Institution { get; set; } = "";

        [JsonPropertyName("DocumentNumber")]
        public string DocumentNumber { get; set; } = "";

        [JsonPropertyName("GraduationDate")]
        public string GraduationDate { get; set; } = "";

        [JsonPropertyName("Language")]
        public string Language { get; set; } = "";

        [JsonPropertyName("AverageScore")]
        public int AverageScore { get; set; } = 0;

        [JsonPropertyName("PostalCode")]
        public string PostalCode { get; set; } = "";

        [JsonPropertyName("Country")]
        public string Country { get; set; } = "";

        [JsonPropertyName("Region")]
        public string Region { get; set; } = "";

        [JsonPropertyName("District")]
        public string District { get; set; } = "";

        [JsonPropertyName("LocalityType")]
        public string LocalityType { get; set; } = "";

        [JsonPropertyName("LocalityName")]
        public string LocalityName { get; set; } = "";

        [JsonPropertyName("StreetType")]
        public string StreetType { get; set; } = "";

        [JsonPropertyName("Street")]
        public string Street { get; set; } = "";

        [JsonPropertyName("HouseNumber")]
        public string HouseNumber { get; set; } = "";

        [JsonPropertyName("HousingNumber")]
        public string HousingNumber { get; set; } = "";

        [JsonPropertyName("FlatNumber")]
        public string FlatNumber { get; set; } = "";

        [JsonPropertyName("PhoneNumber")]
        public string PhoneNumber { get; set; } = "";

        [JsonPropertyName("Benefits")]
        public string Benefits { get; set; } = "";

        [JsonPropertyName("FatherType")]
        public string FatherType { get; set; } = "";

        [JsonPropertyName("FatherLastname")]
        public string FatherLastname { get; set; } = "";

        [JsonPropertyName("FatherFirstname")]
        public string FatherFirstname { get; set; } = "";

        [JsonPropertyName("FatherSurname")]
        public string FatherSurname { get; set; } = "";

        [JsonPropertyName("FatherAddress")]
        public string FatherAddress { get; set; } = "";

        [JsonPropertyName("MotherType")]
        public string MotherType { get; set; } = "";

        [JsonPropertyName("MotherLastname")]
        public string MotherLastname { get; set; } = "";

        [JsonPropertyName("MotherFirstname")]
        public string MotherFirstname { get; set; } = "";

        [JsonPropertyName("MotherSurname")]
        public string MotherSurname { get; set; } = "";

        [JsonPropertyName("MotherAddress")]
        public string MotherAddress { get; set; } = "";
    }
}