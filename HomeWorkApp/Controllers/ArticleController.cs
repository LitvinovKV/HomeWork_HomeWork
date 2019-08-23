using HomeWorkApp.Models;
using HomeWorkApp.Util.IoC_Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;

namespace HomeWorkApp.Controllers
{
    public class ArticleController : Controller
    {
        ICatalogOfArticles catalog;

        // подгрузить IoC интерфейс каталога для управления статьями
        public ArticleController(ICatalogOfArticles catalog)
        {
            this.catalog = catalog;
        }

        // Получить список статей подкаталога и вернуть частичное представление
        public ActionResult CatalogArticles(int? catalogId)
        {
            if (catalogId == null)
                return RedirectToAction("Index", "Error", new { statusCode = 404, msg = "Вы не выбрали каталог" });

            IEnumerable<Article> articles = catalog.ArticlesList((int)catalogId);
            return PartialView(articles);
        }

        // Получить статью по идентфиикатору и вернуть представление с ее информацией
        public async Task<ActionResult> ArticleInfo(int? articleId)
        {
            if (articleId == null)
                return RedirectToAction("Index", "Error", new { statusCode = 404, msg = "Вы не выбрали статью" });

            Article article = await catalog.GetOneArticleAsync((int)articleId);
            return View(new ArticleModel() { Text = article.Text, Name = article.Name, Id = article.Id });
        }

        [HttpGet]
        [Authorize]
        // Создать промежуточную модель статьи, чтобы взаимодействовал с ней, а не с моделью. 
        // Вернуть страницу с формой для ввода информации по статье
        public ActionResult AddArticle(int? catalogId)
        {
            if (catalogId == null)
                return RedirectToAction("Index", "Error", new { statusCode = 404, msg = "Вы не выбрали каталог" });

            TempData["subCatalogId"] = (int)catalogId;
            return View(new ArticleModel());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        // Получить введенную информацию пользователем и добавить статью в БД
        public async Task<ActionResult> AddArticle(ArticleModel articleModel)
        {
            if(ModelState.IsValid)
            {
                if (TempData["subCatalogId"] == null)
                        return RedirectToAction("Index", "Error", new { statusCode = 404, msg = "Вы не выбрали каталог" });

                articleModel.SubCatalogId = (int)TempData["subCatalogId"];
                TempData["subCatalogId"] = null;
                await catalog.AddArticle(articleModel);
                return RedirectToAction("Index", "Home");
            }
            return View(articleModel);
        }

        [Authorize]
        // Удалить статью по ее идентификатору
        public async Task<ActionResult> DeleteArticle(int? articleId)
        {
            if (articleId == null)
                return RedirectToAction("Index", "Error", new { statusCode = 404, msg = "Вы не выбрали статью" });

            await catalog.DeleteArticle((int)articleId);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        // Получить идентификатор статьи. 
        // Сформировать все необходимые данные для формирования страницы - редактирования статьи: модель статьи, SelectList
        // Вернуть страницу для изменения данных статьи
        public ActionResult UpdateArticle(int? articleId)
        {
            if (articleId == null)
                return RedirectToAction("Index", "Error", new { statusCode = 404, msg = "Вы не выбрали статью" });

            Article article = catalog.GetOneArticle((int)articleId);
            List<SubCatalog> subarticles = catalog.getSubCatalogs().ToList();
            SelectList selectList = new SelectList(subarticles, "Id", "Name", article.SubCatalog.Id);
            TempData["items"] = selectList;

            return View(new ArticleModel() { Id = (int)articleId, Name = article.Name,
                Text = article.Text, SubCatalogId = article.SubCatalog.Id });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        // На вход поступает измененная статья пользователем. Потом вызывается метод IoC каталога для обновления информации
        // В результате редирект на начальную страницу, а если что-то было не заполнено но обратно на форму
        public async Task<ActionResult> UpdateArticle(ArticleModel articleModel)
        {
            if (ModelState.IsValid)
            {
                await catalog.UpdateArticle(articleModel);
                return RedirectToAction("Index", "Home");
            }
            return View(articleModel);
        }
    }
}