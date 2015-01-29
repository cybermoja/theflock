using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMS.Data.Repository;
using FMS.Data;

namespace FMS.Service
{
    public interface IUserService
    {
        User GetLoginDetail(string username, string password);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        
        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public User GetLoginDetail(string username, string password)
        {
            return _userRepo.GetLoginDetail(username, password);
        }
    }
}
