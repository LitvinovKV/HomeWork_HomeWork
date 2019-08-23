using HomeWorkApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HomeWorkApp.Util.IoC_Interfaces
{
    // Интерфейс для реализации управления подкаталогами
    public interface ICatalogOfSubcatalogs
    {
        // Асинхронный метод для получения полного списка подкаталогов
        Task<IEnumerable<SubCatalog>> SubCatalogListAsync();
        // Синхронный метод для получения полного списка подкаталогов
        IEnumerable<SubCatalog> SubCatalogList();
        // Синхронный метод для получения списка подкаталогов у определенного родителя
        IEnumerable<SubCatalog> SubCatalogList(int? parentId);
        // Синхронный метод для получения списка подкаталогов с исключением дочерних
        IEnumerable<SubCatalog> SubCatalogListWithoutSubs(int catalogId);

        // Асинхронный метод для получения экземпляра подкаталога по его идентификатору
        Task<SubCatalog> GetOneSubCatalogAsync(int catalogId);
        // Синхронный метод для получения экземпляра подкаталога по его идентификатору
        SubCatalog GetOneSubCatalog(int id);

        // Асинхронный метод добавления нового подкаталога по его модели
        Task AddSubCatalog(SubCatalogModel catalogModel);
        // Асинхронный методы обновления подкаталога по его модели
        Task UpdateSubCatalog(SubCatalogModel catalogModel);
        // Асинхронный метод для удаления подкаталога по его идентификатору
        Task DeleteSubCatalog(int catalogId);
    }
}