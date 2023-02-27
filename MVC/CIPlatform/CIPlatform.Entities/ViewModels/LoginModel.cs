using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class LoginModel
    {
        [Required]
        [RegularExpression("^[a-z]{1}[a-z0-9]+@[a-z]+\\.+[a-z]{2,3}$", ErrorMessage = "Please enter valid e-mail address")]
        public string EmailId { get; set; }


        [Required]
        [MinLength(8, ErrorMessage = "Please enter more than 8 characters")]
        public string Password { get; set; }
    }
}
