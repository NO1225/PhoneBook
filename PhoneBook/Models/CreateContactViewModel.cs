using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PhoneBook.Models
{
    public class CreateContactViewModel 
    {
        [Required]
        [MaxLength(30)]
        [Display(Name ="First Name:")]
        public string FirstName { get; set; }

        [MaxLength(30)]
        [Display(Name = "Last Name:")]
        public string LastName { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number:")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Personal Image:")]
        public IFormFile Image { get; set; }
    }
}
