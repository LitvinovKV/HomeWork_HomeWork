using System;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.AspNet.Identity.Repositories;
using Microsoft.AspNet.Identity;
using Snork.FluentNHibernateTools;

namespace FluentNHibernate.AspNet.Identity
{
    public class FluentNHibernateRoleStore<TRole> : IQueryableRoleStore<TRole>
        where TRole : IdentityRole, new()
    {
        private readonly RoleRepository<TRole> _roleRepository;


        public FluentNHibernateRoleStore(ProviderTypeEnum providerType, string nameOrConnectionString,
            FluentNHibernatePersistenceBuilderOptions options = null)
        {
            _roleRepository = new RoleRepository<TRole>(providerType, nameOrConnectionString, options);
        }


        public IQueryable<TRole> Roles => _roleRepository.GetRoles();

        public Task CreateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            _roleRepository.Insert(role);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("user");
            }

            _roleRepository.Delete(role.Id);

            return Task.FromResult<object>(null);
        }

        public Task<TRole> FindByIdAsync(string roleId)
        {
            var result = _roleRepository.GetRoleById(roleId) as TRole;

            return Task.FromResult(result);
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            var result = _roleRepository.GetRoleByName(roleName) as TRole;
            return Task.FromResult(result);
        }

        public Task UpdateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("user");
            }

            _roleRepository.Update(role);

            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
            // connection is automatically disposed
        }
    }
}