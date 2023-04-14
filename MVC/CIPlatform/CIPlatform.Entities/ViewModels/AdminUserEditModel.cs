using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class AdminUserEditModel
    {
        public AdminHeader adminHeader { get; set; }=new AdminHeader();
    }
}
