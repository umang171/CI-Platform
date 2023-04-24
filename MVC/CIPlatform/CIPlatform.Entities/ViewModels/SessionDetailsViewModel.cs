using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class SessionDetailsViewModel
    {
        public string Email { get; set; } = String.Empty;
        public string Avatar { get; set; } = String.Empty;
        public long UserId { get; set; }
        public string FullName { get; set; } = String.Empty;
        public string Role { get; set; }= String.Empty;
    }
}
