using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class AdminUserAddModel
    {
        public AdminHeader adminHeader { get; set; } = new AdminHeader();

        [Required(ErrorMessage = "Please Enter your First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please Enter your Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MinLength(10)]
        [MaxLength(10)]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Please enter valid phone no!")]
        public string PhoneNo { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^[a-z]{1}[a-z0-9]+@[a-z]+\\.+[a-z]{2,3}$", ErrorMessage = "Please enter valid e-mail address")]
        public string EmailId { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Enter more than 8 characters")]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string EmployeeId { get; set; } = string.Empty;
        [Required]
        public string Department { get; set; } = string.Empty;
        [Required]
        public string MyProfile { get; set; } = string.Empty;
        [Required]
        public string WhyIVolunteer { get; set; } = string.Empty;
        public string Profile { get; set; } = string.Empty;
        public long UserId { get; set; }
    }
}
