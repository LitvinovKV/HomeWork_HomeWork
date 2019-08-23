using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.AspNet.Identity.Entities;
using Microsoft.AspNet.Identity;
using NHibernate.Linq;
using Snork.FluentNHibernateTools;

namespace FluentNHibernate.AspNet.Identity.Repositories
{
    public class UserLoginRepository : RepositoryBase
    {
        internal UserLoginRepository(string sessionFactoryKey) : base(sessionFactoryKey)
        {
        }

        public UserLoginRepository(ProviderTypeEnum providerType, string nameOrConnectionString,
            FluentNHibernatePersistenceBuilderOptions options) : base(providerType, nameOrConnectionString, options)
        {
        }

        public void Insert(IdentityUser user, UserLoginInfo login)
        {
            using (var session = GetStatelessSession())
            {
                session.Insert(new AspNetUserLogin
                {
                    LoginProvider = login.LoginProvider,
                    ProviderKey = login.ProviderKey,
                    User = new AspNetUser {Id = user.Id}
                });
            }
        }

        public void Delete(IdentityUser user, UserLoginInfo login)
        {
            using (var session = GetStatelessSession())
            {
                var qry = string.Format(
                    @"DELETE FROM {0} WHERE {3}.{1} = :Id AND {2} = :loginprovider AND {4} = :providerkey",
                    nameof(AspNetUserLogin), nameof(AspNetUserLogin.Id), nameof(AspNetUserLogin.LoginProvider),
                    nameof(AspNetUserLogin.User), nameof(AspNetUserLogin.ProviderKey));
                session.CreateQuery(qry)
                    .SetParameter("id", user.Id)
                    .SetParameter("loginprovider", login.LoginProvider)
                    .SetParameter("providerkey", login.ProviderKey)
                    .ExecuteUpdate();
            }
        }

        public string GetByUserLoginInfo(UserLoginInfo login)
        {
            using (var session = GetStatelessSession())
            {
                return session.Query<AspNetUserLogin>()
                    .Where(i => i.LoginProvider == login.LoginProvider && i.ProviderKey == login.ProviderKey)
                    .Select(i => i.User.Id)
                    .FirstOrDefault();
            }
        }

        public List<UserLoginInfo> PopulateLogins(string userId)
        {
            using (var session = GetStatelessSession())
            {
                return session.Query<AspNetUserLogin>()
                    .Where(i => i.User.Id == userId)
                    .Select(i => new UserLoginInfo(i.LoginProvider, i.ProviderKey))
                    .ToList();
            }
        }
    }
}