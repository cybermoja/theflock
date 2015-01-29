using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPoco;

namespace FMS.Data.Repository
{
    public class BaseRepository
    {
        static BaseRepository()
        {
            //declare custom mapping

        }

        //Dapper uses plain SQL connection
        public Database PocoDb
        {
            get
            {
                return new Database(System.Configuration.ConfigurationManager.ConnectionStrings["FMS_DB"].ToString());
            }
        }
    }
}
