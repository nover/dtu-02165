using Bowling.Entity.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Entity.Queries
{
    public static class MemberQueryExtension
    {
        /// <summary>
        /// Find a member by email and password
        /// </summary>
        /// <remarks>
        /// If the email and password does not match any entries, a null instance is returned.
        /// </remarks>
        /// <param name="members">A member queryable member instance</param>
        /// <param name="email">The email to search for</param>
        /// <param name="password">The password</param>
        /// <returns></returns>
        public static Member FindByEmailAndPassword(this IQueryable<Member> members, string email, string password)
        {
            var member = (from y in members
                          where y.Email == email 
                                && y.Password == password
                          select y
                              ).SingleOrDefault<Member>();
            
            return member;
        }

        /// <summary>
        /// Find a member only by email
        /// </summary>
        /// <remarks>
        /// If the email does not match any entries, a null instance is returned.
        /// </remarks>
        /// <param name="members">A member queryable member instance</param>
        /// <param name="email">The email to search for</param>
        /// <returns></returns>
        public static Member FindMemberByEmail(this IQueryable<Member> members, string email)
        {
            var member = (from y in members
                          where y.Email == email
                          select y
                              ).SingleOrDefault<Member>();

            return member;
        }
    }
}
