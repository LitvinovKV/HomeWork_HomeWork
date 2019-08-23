using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.AspNet.Identity.Entities;
using NHibernate.Linq;
using Snork.FluentNHibernateTools;

namespace FluentNHibernate.AspNet.Identity.Repositories
{
    public class RoleRepository<TRole> : RepositoryBase where TRole : IdentityRole, new()
    {
        internal RoleRepository(string sessionFactoryKey) : base(sessionFactoryKey)
        {
        }

        public RoleRepository(ProviderTypeEnum providerType, string nameOrConnectionString,
            FluentNHibernatePersistenceBuilderOptions options) : base(providerType, nameOrConnectionString, options)
        {
        }

        public IQueryable<TRole> GetRoles()
        {
            var roles = new List<TRole>();
            using (var session = GetStatelessSession())
            {
                foreach (var role1 in session.Query<AspNetRole>())
                {
                    var role = new TRole {Id = role1.Id, Name = role1.Name};
                    roles.Add(role);
                }
            }

            return roles.AsQueryable();
        }

        public void Insert(IdentityRole role)
        {
            using (var session = GetStatelessSession())
            {
                session.Insert(new AspNetRole {Id = role.Id, Name = role.Name});
            }
        }

        public void Delete(string roleId)
        {
            using (var session = GetStatelessSession())
            {
                var qry = string.Format("delete from {0} where {1} = :id", nameof(AspNetRole), nameof(AspNetUser.Id));
                session.CreateQuery(qry).SetParameter("id", roleId).ExecuteUpdate();
            }
        }

        public TRole GetRoleById(string roleId)
        {
            using (var session = GetStatelessSession())
            {
                var aspNetRole = session.Query<AspNetRole>().FirstOrDefault(i => i.Id == roleId);
                return Convert(aspNetRole);
            }
        }

        private static TRole Convert(AspNetRole aspNetRole)
        {
            if (aspNetRole != null)
                return new TRole {Id = aspNetRole.Id, Name = aspNetRole.Name};
            return null;
        }

        public TRole GetRoleByName(string roleName)
        {
            using (var session = GetStatelessSession())
            {
                var aspNetRole = session.Query<AspNetRole>().FirstOrDefault(i => i.Name == roleName);
                return Convert(aspNetRole);
            }
        }


        public void Update(IdentityRole role)
        {
            using (var session = GetStatelessSession())
            {
                var qry = string.Format("update {0} set {1}=:name where {2}=:id", nameof(AspNetRole),
                    nameof(AspNetRole.Name), nameof(AspNetRole.Id));
                session.CreateQuery(qry).SetParameter("name", role.Name).SetParameter("id", role.Id).ExecuteUpdate();
            }
        }
    }
}