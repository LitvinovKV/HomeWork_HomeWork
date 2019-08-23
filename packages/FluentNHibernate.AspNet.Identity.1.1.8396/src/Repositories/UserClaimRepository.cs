using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using FluentNHibernate.AspNet.Identity.Entities;
using NHibernate.Linq;
using Snork.FluentNHibernateTools;

namespace FluentNHibernate.AspNet.Identity.Repositories
{
    public class UserClaimRepository<TUser> : RepositoryBase where TUser : IdentityUser
    {
        internal UserClaimRepository(string sessionFactoryKey) : base(sessionFactoryKey)
        {
        }

        public UserClaimRepository(ProviderTypeEnum providerType, string nameOrConnectionString,
            FluentNHibernatePersistenceBuilderOptions options) : base(providerType, nameOrConnectionString, options)
        {
        }

        public void Insert(TUser user, Claim claim)
        {
            using (var session = GetStatelessSession())
            {
                session.Insert(new AspNetUserClaim
                {
                    Type = claim.Type,
                    Value = claim.Value,
                    User = new AspNetUser {Id = user.Id}
                });
            }
        }

        public void Delete(TUser user, Claim claim)
        {
            using (var session = GetStatelessSession())
            {
                var qry = string.Format("delete from {0} where {1}=:id and {2}=:type and {3}=:value",
                    nameof(AspNetUserClaim), nameof(AspNetUserClaim.Id), nameof(AspNetUserClaim.Type),
                    nameof(AspNetUserClaim.Value));
                session.CreateQuery(qry)
                    .SetParameter("id", user.Id)
                    .SetParameter("value", claim.Value)
                    .SetParameter("type", claim.Type)
                    .ExecuteUpdate();
            }
        }

        public List<IdentityUserClaim> PopulateClaims(string userId)
        {
            using (var session = GetStatelessSession())
            {
                return session.Query<AspNetUserClaim>()
                    .Where(i => i.User.Id == userId)
                    .Select(i => new IdentityUserClaim {ClaimType = i.Type, ClaimValue = i.Value})
                    .ToList();
            }
        }
    }
}