using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Data
{
    public enum TransactionType
    {
        General,
        Incoming,
        Outgoing,
        IncomingHdb,
        OutgoingHdb
    }

    [TableName("transactions")]
    [PrimaryKey("id")]
    public class Transaction
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("historyid")]
        public int HistoryId { get; set; }

        [Column("type")]
        public TransactionType Type { get; set; }

        [Column("userid")]
        public int UserId { get; set; }

        [Column("datecreated")]
        public DateTime DateCreated { get; set; }
    }
}
