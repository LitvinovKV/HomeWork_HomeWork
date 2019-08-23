using HomeWorkApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeWorkApp.Util.IoC_Interfaces
{
    // Интерфейс для управления статьями
    public interface ICatalogOfArticles
    {
        // Асинхронный метод для получения статьи по идентификатору
        Task<Article> GetOneArticleAsync(int id);
        // Синхронный метод для получения статьи по идентификатору
        Article GetOneArticle(int id);
        // Асинхронный метод для получения списка статей
        Task<IEnumerable<Article>> ArticlesList();
        // Синхронный метол для получения спика статей у определенной подкатегории
        IEnumerable<Article> ArticlesList(int subCatalogId);

        // Асинхронный метод для добавления статьи по ее модели
        Task AddArticle(ArticleModel articleModel);
        // Асинхронный метол для удаления статьи по идентификатору
        Task DeleteArticle(int id);
        // Асинхронный метод для обновления статьи по ее модели
        Task UpdateArticle(ArticleModel articleModel);

        // Синхронный метод для получения списка всех подкаталогов
        IEnumerable<SubCatalog> getSubCatalogs();
    }
}