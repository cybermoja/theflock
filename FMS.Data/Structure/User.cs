using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Data
{
    [TableName("user")]
    [PrimaryKey("id,memberid")]
    public class User
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("memberid")]
        public int MemberId { get; set; }

        [Column("username")]
        public string UserName { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("roles")]
        public string Roles { get; set; }

        [Column("groups")]
        public string Groups { get; set; }
    }
}
