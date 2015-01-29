using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Data
{
    public enum MemberOffices
    {
        None,
        Choir,
        Secretary,
        Los,
        Svf
    }

    public enum MemberStatus
    {
        NotRegistered,
        Registered,
        Inactive,
        Deleted
    }

    [TableName("member")]
    [PrimaryKey("id")]
    public class Member
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("incid")]
        public string IncId { get; set; }

        [Column("localid")]
        public string LocalId { get; set; }

        [Column("area")]
        public int Area { get; set; }

        [Column("group")]
        public int Group { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("middlename")]
        public string MiddleName { get; set; }

        [Column("birthdate")]
        public DateTime BirthDate { get; set; }

        [Column("office")]
        public MemberOffices Office { get; set; }

        [Column("id")]
        public MemberStatus Status { get; set; }
    }
}
