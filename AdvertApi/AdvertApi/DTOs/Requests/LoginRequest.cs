﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.DTOs.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Login field has to be specified")]
        [MaxLength(15)]
        public string Login { get; set; }
        [Required(ErrorMessage = "Password field has to be specified")]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
