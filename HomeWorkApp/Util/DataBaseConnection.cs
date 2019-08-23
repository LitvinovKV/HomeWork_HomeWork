using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using HomeWorkApp.Models;
using HomeWorkApp.Util.IoC_Interfaces;
using Microsoft.AspNet.Identity;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Configuration;

namespace HomeWorkApp.Util
{
    // Реализация интерфейса IDataBaseConnection Для создания сессии и подключения к СУБД Ms SQL 2008 (localDB)
    // В дальнейшем вместо интерфейса IDataBaseConnection, IoC контейнер Ninject будет подставлять эту реализацию
    public class DataBaseConnection : IDataBaseConnection
    {
        public ISession CreateSession()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["IdentityHomeWorkDB"].ConnectionString;
            ISessionFactory sessionFactory = Fluently.Configure()
                // Настройка БД, строка подключения к БД MS SQL Server 2008
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString))
                // Маппинг. NHibernate будем пытаться мапить каждый класс в сборке
                .Mappings(cfg => cfg.FluentMappings.AddFromAssemblyOf<DataBaseConnection>())
                
                .ExposeConfiguration(cfg => {
                    // ShemaUpdate позволяет создавать/обновлять в БД таблицы и поля
                    new SchemaUpdate(cfg).Execute(false, true);
                    // Кэширование второго уровня
                    cfg.SetProperty("cache.provider_class", "NHibernate.Cache.HashtableCacheProvider");
                    cfg.SetProperty("cache.use_second_level_cache", "true");
                    cfg.SetProperty("cache.use_query_cache", "true");
                })
                .BuildSessionFactory();
            return sessionFactory.OpenSession();
        }

        public IUserStore<User, int> CreateIdentityStoreUsers
        {
            get { return new IdentityStore(CreateSession()); }
        }

        public static void InitDataDB()
        {
            using (ISession session = new DataBaseConnection().CreateSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Random random = new Random();
                    #region Главные каталоги
                    for (int i = 1; i < 6; i++)
                    {
                        SubCatalog newCatalog1 = new SubCatalog();
                        newCatalog1.ParentCatalog = null;
                        newCatalog1.Name = "Каталог №" + i;
                        session.SaveOrUpdate(newCatalog1);
                    }
                    #endregion

                    #region Подкаталоги главных каталогов
                    for (int i = 1; i < 6; i++)
                    {
                        for (int j = 1; j < random.Next(1, 6); j++)
                        {
                            SubCatalog newSubCatalog = new SubCatalog();
                            newSubCatalog.Name = "Подкаталог №" + i + "." + j;
                            newSubCatalog.ParentCatalog = session.Get<SubCatalog>(i);
                            session.SaveOrUpdate(newSubCatalog);
                        }
                    }
                    #endregion

                    #region Статьи
                    for (int i = 1; i < 40; i++)
                    {
                        Article newArticle = new Article();
                        newArticle.Name = "Статья №" + i;
                        newArticle.Text = "Текст статьи №" + i;
                        newArticle.SubCatalog = session.Get<SubCatalog>(random.Next(1, session.QueryOver<SubCatalog>().List().Count + 1));
                        session.SaveOrUpdate(newArticle);
                    }
                    #endregion

                    transaction.Commit();
                }
            }
        }
    }
}