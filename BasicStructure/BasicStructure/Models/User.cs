﻿namespace BasicStructure.Models
{
    public class User
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public int RoleId { get; set; }

        public string Email { get; set; }
    }
}
