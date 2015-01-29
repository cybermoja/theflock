using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FMS.Data.Repository
{
    public interface IMemberRepository
    {
        Member FindMemberById(int id);
        Member FindMemberByIncId(long id);
        bool UpdateMember(Member member);
        bool DeleteMember(Member member);
        NPoco.Page<Member> SearchMembers(object query, long page, long items);
    }

    public class MemberRepository : BaseRepository, IMemberRepository
    {
        public Member FindMemberById(int id)
        {
            using(var conn = this.PocoDb)
            {
                return conn.SingleById<Member>(id);
            }
        }

        public Member FindMemberByIncId(long id)
        {
            using (var conn = this.PocoDb)
            {
                return conn.Single<Member>("incid = @id", new { id });
            }
        }

        public bool UpdateMember(Member member)
        {
            using (var conn = this.PocoDb)
            {
                return conn.Update(member) > 0;
            }
        }

        public bool DeleteMember(Member member)
        {
            member.Status = MemberStatus.Deleted;

            using (var conn = this.PocoDb)
            {
                return conn.Update(member) > 0;
            }
        }

        public NPoco.Page<Member> SearchMembers(object query, long page, long items)
        {
            var localIdRegex = new Regex(@"^L?\d{1,5}$");
            var incIdRegex = new Regex(@"^\d{10,}$");
            using (var conn = this.PocoDb)
            {
                if(localIdRegex.Match(query.ToString()).Success)
                {
                    return conn.Page<Member>(page, items, "localid like @num", new { num = query.ToString() + "%" });
                }
                if (incIdRegex.Match(query.ToString()).Success)
                {
                    return conn.Page<Member>(page, items, "incid = @num", new { num = query.ToString() });
                }

                return conn.Page<Member>(page, items, "lastname like @name or firstname like @name", new { name = "%" + query.ToString() + "%" });
            }
        }
    }
}
