using HomeWorkApp.Models;
using HomeWorkApp.Util.IoC_Interfaces;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeWorkApp.Util
{
    // Класс, который имплементирует интерфейс ICatalogOfArticles для выполнения различных операций с БД и статьями
    // В дальнейшем с помощью IoC контейнера Ninject эта реализация будет подставляться вместо интерфейса ICatalogOfArticles
    public class CatalogOfArticles : IDisposable, ICatalogOfArticles
    {
        IDataBaseConnection connection;
        ICatalogOfSubcatalogs subCatalog;
        public CatalogOfArticles(IDataBaseConnection connection, ICatalogOfSubcatalogs subCatalog)
        {
            this.connection = connection;
            this.subCatalog = subCatalog;
        }

        public async Task<Article> GetOneArticleAsync(int id)
        {
            Article result = null;
            using (ISession session = connection.CreateSession())
                result = await session.GetAsync<Article>(id);
            return result;
        }

        public Article GetOneArticle(int id)
        {
            Article result = null;
            using (ISession session = connection.CreateSession())
                result = session.Get<Article>(id);
            return result;
        }

        public async Task<IEnumerable<Article>> ArticlesList()
        {
            IEnumerable<Article> result = null;
            using (ISession session = connection.CreateSession())
                result = await session.QueryOver<Article>().ListAsync();
            return result;
        }

        public IEnumerable<Article> ArticlesList(int subCatalogId)
        {
            IEnumerable<Article> result = null;
            using (ISession session = connection.CreateSession())
                result = session.QueryOver<Article>().Where(article => article.SubCatalog.Id == subCatalogId).List();
            return result;
        }

        public async Task AddArticle(ArticleModel articleModel)
        {
            using (ISession session = connection.CreateSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Article article = new Article() { Name = articleModel.Name, Text = articleModel.Text };
                    article.SubCatalog = await subCatalog.GetOneSubCatalogAsync(articleModel.SubCatalogId);
                    await session.SaveOrUpdateAsync(article);
                    await transaction.CommitAsync();
                }
            }
        }

        public async Task DeleteArticle(int id)
        {
            using (ISession session = connection.CreateSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Article article = await GetOneArticleAsync(id);
                    await session.DeleteAsync(article);
                    await transaction.CommitAsync();
                }
            }
        }

        public IEnumerable<SubCatalog> getSubCatalogs()
        {
            IEnumerable<SubCatalog> result = null;
            using (ISession session = connection.CreateSession())
            {
                result = session.QueryOver<SubCatalog>().List();
            }
            return result;
        }

        public async Task UpdateArticle(ArticleModel articleModel)
        {
            using (ISession session = connection.CreateSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Article article = new Article() { Id = articleModel.Id,
                        Name = articleModel.Name, Text = articleModel.Text,
                        SubCatalog = await subCatalog.GetOneSubCatalogAsync(articleModel.SubCatalogId)};
                    await session.SaveOrUpdateAsync(article);
                    await transaction.CommitAsync();
                }
            }
        }

        public void Dispose() { }
    }
}