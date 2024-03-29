﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace FruitMVC.ViewModels
{
    public class RegisterVM
    {
        [Required]

        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
