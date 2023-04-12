using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repository.Repository.Interface
{
    public interface IAdminRepository
    {
        public Boolean validateEmail(string email);
        public Boolean validateUser(string email, string password);
        public List<User> getUsers(string? searchText);
        public Admin findAdmin(string email);
    }
}
