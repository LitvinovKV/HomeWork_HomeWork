using System.Web.Mvc;

namespace HomeWorkApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Инициализация начальных данных
            //DataBaseConnection.InitDataDB();
            return View();
        }
        
    }
}