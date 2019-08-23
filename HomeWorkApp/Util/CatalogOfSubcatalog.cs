using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWorkApp.Models;
using HomeWorkApp.Util.IoC_Interfaces;
using NHibernate;

namespace HomeWorkApp.Util
{
    // Класс, который имплементирует интерфейс ICatalogOfSubcatalogs для выполнения различных операций с БД и подкаталогами
    // В дальнейшем с помощью IoC контейнера Ninject эта реализация будет подставляться вместо интерфейса ICatalogOfSubcatalogs
    public class CatalogOfSubcatalog : IDisposable, ICatalogOfSubcatalogs
    {
        IDataBaseConnection connection;
        public CatalogOfSubcatalog(IDataBaseConnection connection)
        {
            this.connection = connection;
        }

        public async Task<SubCatalog> GetOneSubCatalogAsync(int id)
        {
            SubCatalog result = null;
            using (ISession session = connection.CreateSession())
                result = await session.GetAsync<SubCatalog>(id);
            return result;
        }

        public SubCatalog GetOneSubCatalog(int id)
        {
            SubCatalog result = null;
            using (ISession session = connection.CreateSession())
                result = session.Get<SubCatalog>(id);
            return result;
        }

        public async Task<IEnumerable<SubCatalog>> SubCatalogListAsync()
        {
            IEnumerable<SubCatalog> result = null;
            using (ISession session = connection.CreateSession())
                result = await session.QueryOver<SubCatalog>().ListAsync();
            return result;
        }

        public IEnumerable<SubCatalog> SubCatalogList()
        {
            IEnumerable<SubCatalog> result = null;
            using (ISession session = connection.CreateSession())
                result = session.QueryOver<SubCatalog>().List();
            return result;
        }

        public IEnumerable<SubCatalog> SubCatalogList(int? parentId)
        {
            IEnumerable<SubCatalog> result = null;
            using (ISession session = connection.CreateSession())
            {
                if (parentId == null)
                    result = session.QueryOver<SubCatalog>()
                        .Where(subCatalog => subCatalog.ParentCatalog == null).List();
                else
                    result = session.QueryOver<SubCatalog>()
                        .Where(subCatalog => subCatalog.ParentCatalog.Id == parentId).List();
            }
            return result;
        }

        public async Task AddSubCatalog(SubCatalogModel catalogModel)
        {
            using (ISession session = connection.CreateSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    SubCatalog catalog = new SubCatalog() { Name = catalogModel.Name };
                    if (catalogModel.ParentId != null)
                        catalog.ParentCatalog = await GetOneSubCatalogAsync((int)catalogModel.ParentId);
                    await session.SaveOrUpdateAsync(catalog);
                    await transaction.CommitAsync();
                }
            }
        }

        public async Task DeleteSubCatalog(int catalogId)
        {
            using (ISession session = connection.CreateSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    SubCatalog subCatalog = await GetOneSubCatalogAsync(catalogId);
                    await session.DeleteAsync(subCatalog);
                    await transaction.CommitAsync();
                }
            }
        }


        public async Task UpdateSubCatalog(SubCatalogModel catalogModel)
        {
            using (ISession session = connection.CreateSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    SubCatalog subCatalog = await GetOneSubCatalogAsync(catalogModel.Id);
                    SubCatalog parentSubCatalog = null;
                    if (catalogModel.ParentId != null)
                        parentSubCatalog = await GetOneSubCatalogAsync((int)catalogModel.ParentId);
                    subCatalog.Name = catalogModel.Name;
                    subCatalog.ParentCatalog = parentSubCatalog;

                    await session.SaveOrUpdateAsync(subCatalog);
                    await transaction.CommitAsync();
                }
            }
        }

        public IEnumerable<SubCatalog> SubCatalogListWithoutSubs(int catalogId)
        {
            IEnumerable<SubCatalog> result = null;
            using (ISession session = connection.CreateSession())
            {
                // Сделать так, чтобы при выборе подкаталогов для изменения не было дочерних подкаталогов и его самого
                List<int> parentCatalogsId = new List<int>() { catalogId };
                result = SubCatalogList()
                    .Where(subCatalog =>
                    {
                        SubCatalog parentCatalog = subCatalog.ParentCatalog;
                        //  Если родителский Id подкаталога есть в списке, 
                        //  то этот подкаталог не подходит (запомнить его) или это он сам (то пропустить)
                        if ((parentCatalog != null && parentCatalogsId.Contains(parentCatalog.Id) == true) || 
                            subCatalog.Id == catalogId)
                        {
                            parentCatalogsId.Add(subCatalog.Id);
                            return false;
                        }
                        else
                            return true;
                    });
            }
            return result;
        }

        public void Dispose() { }
    }
}