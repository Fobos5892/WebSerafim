using Microsoft.AspNetCore.Identity;
using System;

namespace SerafimeWeb.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }

        public DateTime Birthdate { get; set; }

        public string Address { get; set; }
    }
}
