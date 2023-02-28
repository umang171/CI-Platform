using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class NewPasswordModel
    {
        [Required]
        [MinLength(8, ErrorMessage = "Please enter more than 8 characters")]
        public string NewPassword { get; set; }
        [Required]

        [MinLength(8, ErrorMessage = "Please enter more than 8 characters")]
        public string ConfirmPassword { get; set; }
        public string userId;
    }
}
