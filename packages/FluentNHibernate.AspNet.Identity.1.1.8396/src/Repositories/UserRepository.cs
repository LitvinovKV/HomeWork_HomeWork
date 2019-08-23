using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.AspNet.Identity.Entities;
using NHibernate.Linq;
using Snork.FluentNHibernateTools;

namespace FluentNHibernate.AspNet.Identity.Repositories
{
    public class UserRepository<TUser> : RepositoryBase where TUser : IdentityUser
    {
        public UserRepository(ProviderTypeEnum providerType, string nameOrConnectionString,
            FluentNHibernatePersistenceBuilderOptions options) : base(providerType, nameOrConnectionString, options)
        {
        }

        internal UserRepository(string sessionFactoryKey) : base(sessionFactoryKey)
        {
        }

        public void Insert(TUser user)
        {
            using (var session = GetStatelessSession())
            {
                var aspNetUser = new AspNetUser();
                CopyFromTUser(user, aspNetUser);
                session.Insert(aspNetUser);
            }
        }

        public void Delete(TUser user)
        {
            using (var session = GetStatelessSession())
            {
                var qry = string.Format("delete from {0} where {1} = :id", nameof(AspNetUser), nameof(AspNetUser.Id));
                session.CreateQuery(qry).SetParameter("id", user.Id).ExecuteUpdate();
            }
        }


        public IQueryable<TUser> GetAll()
        {
            var users = new List<TUser>();

            using (var session = GetStatelessSession())
            {
                var list = session.Query<AspNetUser>().ToList();
                foreach (var user in list)
                {
                    users.Add(CopyToTUser(user));
                }
            }
            return users.AsQueryable();
        }

        private static void CopyFromTUser(TUser source, AspNetUser destination)
        {
            destination.Id = source.Id;
            destination.Email = source.Email;
            destination.EmailConfirmed = source.EmailConfirmed;
            destination.UserName = source.UserName;
            destination.PasswordHash = source.PasswordHash;
            destination.SecurityStamp = source.SecurityStamp;
            destination.PhoneNumber = source.PhoneNumber;
            destination.PhoneNumberConfirmed = source.PhoneNumberConfirmed;
            destination.TwoFactorEnabled = source.TwoFactorEnabled;
            destination.LockoutEndDateUtc = source.LockoutEndDate;
            destination.LockoutEnabled = source.LockoutEnabled;
            destination.AccessFailedCount = source.AccessFailedCount;
        
        }

        private static TUser CopyToTUser(AspNetUser source)
        {
            var user = (TUser) Activator.CreateInstance(typeof(TUser));
            user.Id = source.Id;
            user.Email = source.Email;
            user.EmailConfirmed = source.EmailConfirmed;
            user.UserName = source.UserName;
            user.PasswordHash = source.PasswordHash;
            user.SecurityStamp = source.SecurityStamp;
            user.PhoneNumber = source.PhoneNumber;
            user.PhoneNumberConfirmed = source.PhoneNumberConfirmed;
            user.TwoFactorEnabled = source.TwoFactorEnabled;
            user.LockoutEndDate = source.LockoutEndDateUtc;
            user.LockoutEnabled = source.LockoutEnabled;
            user.AccessFailedCount = source.AccessFailedCount;
            user.UserName = source.UserName;
            return user;
        }

        public TUser GetById(string userId)
        {
            using (var session = GetStatelessSession())
            {
                var user = session.Query<AspNetUser>().FirstOrDefault(i => i.Id == userId);
                return user == null ? null : CopyToTUser(user);
            }
        }

        public TUser GetByName(string userName)
        {
            using (var session = GetStatelessSession())
            {
                var user = session.Query<AspNetUser>().FirstOrDefault(i => i.UserName == userName);
                return user == null ? null : CopyToTUser(user);
            }
        }

        public TUser GetByEmail(string email)
        {
            using (var session = GetStatelessSession())
            {
                var user = session.Query<AspNetUser>().FirstOrDefault(i => i.Email == email);
                return user == null ? null : CopyToTUser(user);
            }
        }

        public void Update(TUser user)
        {
            using (var session = GetStatelessSession())
            {
                var aspNetUser = session.Query<AspNetUser>().FirstOrDefault(i => i.Id == user.Id);
                if (aspNetUser != null)
                {
                    CopyFromTUser(user, aspNetUser);
                    session.Update(aspNetUser);
                }
            }
        }
    }
}