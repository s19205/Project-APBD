using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.DTOs.Requests
{
    public class RegistrationRequest
    {
        [Required(ErrorMessage = "First name has to be specified")]
        [MaxLength(25)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name has to be specified")]
        [MaxLength(35)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email has to be specified")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone has to be specified")]
        [Phone]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Login has to be specified")]
        [MaxLength(15)]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password has to be specified")]
        [MaxLength(30)]
        public string Password { get; set; }
    }
}
