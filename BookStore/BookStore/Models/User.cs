using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BookStore.Models
{


    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime Birthdate { get; set; }

        public string Email { get; set; }

        public string Premiumuser { get; set; }

        public string Role { get; set; }

    }
}