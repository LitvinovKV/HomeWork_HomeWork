using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeWorkApp.Controllers
{
    // Элементарный вспомогательный класс для сообщений об ошибках
    public class ErrorController : Controller
    {
        public ActionResult Index(int statusCode, string msg)
        {
            Response.StatusCode = statusCode;
            TempData["msg"] = msg;
            return View();
        }
    }
}