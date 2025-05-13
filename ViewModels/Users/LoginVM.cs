﻿using System.ComponentModel.DataAnnotations;

namespace ProniaMVCProject.ViewModels
{
    public class LoginVM
    {
        [MaxLength(100)]
        public string UserNameOrEmail { get; set; }
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; }
        public bool IsPersistent { get; set; }
    }
}
