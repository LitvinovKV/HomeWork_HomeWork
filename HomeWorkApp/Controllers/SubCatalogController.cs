using HomeWorkApp.Models;
using HomeWorkApp.Util.IoC_Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HomeWorkApp.Controllers
{
    public class SubCatalogController : Controller
    {
        ICatalogOfSubcatalogs catalog;

        // Формирование интерфейса с помощью IoC контейнера, 
        // чтобы в дальнешйем использовать методы для управления подкаталогами
        public SubCatalogController(ICatalogOfSubcatalogs catalog)
        {
            this.catalog = catalog;
        }
        
        [HttpGet]
        // Вернуть частичное представление с подкаталогами родителя
        public ActionResult SubCatalogCatalogs(int? parentId)
        {
            IEnumerable<SubCatalog> result = catalog.SubCatalogList(parentId);
            return PartialView(result);
        }

        [HttpGet]
        [Authorize]
        // Добавить новый подкаталог к родительскому каталогу по идентификатору
        // Возвращается представление с формой для заполнения модели подкаталога
        public ActionResult AddSubCatalog(int? parentId)
        {
            return View(new SubCatalogModel() { ParentId = parentId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        // На вход поступает модель с заполненными данными из представления
        // Если все свйоства модели валидны, до добавить модель в БД и редирект на начальную страницу
        // Иначе вернуть снова представление - форму 
        public async Task<ActionResult> AddSubCatalog(SubCatalogModel subCatalogModel)
        {
            if (ModelState.IsValid)
            {
                await catalog.AddSubCatalog(subCatalogModel);
                return RedirectToAction("Index", "Home");
            }
            return View(subCatalogModel);
        }

        [HttpGet]
        [Authorize]
        // Удалить подкаталог по его идентификатору
        public async Task<ActionResult> DeleteSubCatalog(int? catalogId)
        {
            if (catalogId == null)
                return RedirectToAction("Index", "Error", new { statusCode = 404, msg = "Вы не выбрали подкаталог" });

            await catalog.DeleteSubCatalog((int)catalogId);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        // По входному параметру (идентификатор подкаталога) настроить все необходимые параметры для формирования представления
        // редактирования подкаталога - модель подкаталога, SelectList для выбора нового родительского каталога
        // Вернуть представление с формой для изменения данных
        public ActionResult UpdateSubCatalog(int? idSubCatalog)
        {
            if (idSubCatalog == null)
                return RedirectToAction("Index", "Error", new { statusCode = 404, msg = "Вы не выбрали подкаталог" });

            List<SubCatalog> listItems = catalog.SubCatalogListWithoutSubs((int)idSubCatalog).ToList();
            SubCatalog subCatalog = catalog.GetOneSubCatalog((int)idSubCatalog);
            int parentId = subCatalog.ParentCatalog == null ? 0 : subCatalog.ParentCatalog.Id;
            listItems.Insert(0, new SubCatalog() { Name = "Корень" });

            SelectList selectList = new SelectList(listItems, "Id", "Name", parentId);
            TempData["items"] = selectList;

            return View(new SubCatalogModel() { Id = (int)idSubCatalog, Name = subCatalog.Name, ParentId = parentId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        // Измененные данные передать в IoC интерфейс для изменений
        public async Task<ActionResult> UpdateSubCatalog(SubCatalogModel subCatalogModel)
        {
            if (ModelState.IsValid)
            {
                await catalog.UpdateSubCatalog(subCatalogModel);
                return RedirectToAction("Index", "Home");
            }
            return View(subCatalogModel);
        }
    }
}