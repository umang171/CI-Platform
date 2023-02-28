using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class RegistrationModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(10)]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Please enter valid phone no!")]
        public string PhoneNo { get; set; }

        [Required]
        [RegularExpression("^[a-z]{1}[a-z0-9]+@[a-z]+\\.+[a-z]{2,3}$", ErrorMessage = "Please enter valid e-mail address")]
        public string EmailId { get; set; }

        [Required]
        [MinLength(8, ErrorMessage="Enter more than 8 characters")]
        public string Password { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Enter more than 8 characters")]
        public string ConfirmPassword { get; set; }
    }
}
