﻿using System.ComponentModel.DataAnnotations;

namespace SecurityService_Core.Models.ControllerDTO.Administrator
{
    public class AdminChangeInputModel
    {
        [Required]
        [Length(6, 100)]
        public string UserName { get; set; }
        [Required]
        [Length(6, 50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Length(11, 20)]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [Length(6, 100)]
        public string FIO { get; set; }
        [Required]
        [Length(10, 100)]
        public string Organization { get; set; }
        [Required]
        public string INN { get; set; }
        [Required]
        public int Role { get; set; }
        [Required]
        public int State { get; set; }
        [Required]
        [MinLength(10), MaxLength(256)]
        public string Address { get; set; }
    }
}