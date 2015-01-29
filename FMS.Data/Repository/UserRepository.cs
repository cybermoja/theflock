using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Data.Repository
{
    public interface IUserRepository
    {
        User GetLoginDetail(string username, string password);
    }

    public class UserRepository : BaseRepository, IUserRepository
    {
        public User GetLoginDetail(string username, string password)
        {
            using(var conn = this.PocoDb)
            {
                return conn.Single<User>("username = @username and password = @password", new { username, password });
            }
        }
    }
}
