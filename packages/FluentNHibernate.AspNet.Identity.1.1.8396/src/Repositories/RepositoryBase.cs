using FluentNHibernate.AspNet.Identity.Entities;
using NHibernate;
using Snork.FluentNHibernateTools;

namespace FluentNHibernate.AspNet.Identity.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly SessionFactoryInfo _info;

        protected internal RepositoryBase(string sessionFactoryKey)
        {
            _info = SessionFactoryBuilder.GetByKey(sessionFactoryKey);
        }

        protected RepositoryBase(ProviderTypeEnum providerType, string nameOrConnectionString,
            FluentNHibernatePersistenceBuilderOptions options)
        {
            _info =
                SessionFactoryBuilder.GetFromAssemblyOf<AspNetUser>(providerType,
                    nameOrConnectionString, options);
        }

        public string SessionFactoryKey => _info.Key;

        public IStatelessSession GetStatelessSession()
        {
            return _info.SessionFactory.OpenStatelessSession();
        }

        public ISession GetSession()
        {
            return _info.SessionFactory.OpenSession();
        }
    }
}