﻿namespace WebApplication4.Models
{
    public class User
    {
        public int? Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public bool Confirmed { get; set;} 
        public bool Accepted { get; set;}
    }
}
