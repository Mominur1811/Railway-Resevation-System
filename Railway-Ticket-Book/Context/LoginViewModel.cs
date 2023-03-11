using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Railway_Ticket_Book.Context
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email is required")]
        [DataType(DataType.EmailAddress)]
        public string nam { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(4,ErrorMessage ="Minimum 4 characters"), MaxLength(8,ErrorMessage ="Maximum 8 characters")]
        public string pass { get; set; }
    }
}