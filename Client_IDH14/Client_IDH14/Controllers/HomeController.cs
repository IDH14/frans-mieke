using Client_IDH14.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;

namespace Client_IDH14.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new List<FileHandler>();

            DirectoryInfo c = new DirectoryInfo(@"C:\Users\Mieke\Desktop\IDH14_Client");//Assuming Test is your Folder

            FileInfo[] Files2 = c.GetFiles("*.*"); //Getting Text files

            foreach (FileInfo file in Files2)
            {
                FileHandler tempFile = new FileHandler();
                tempFile.FileName = file.Name;

                model.Add(tempFile);
            }

            return View(model);
        }


        public ActionResult Connect(String server, String port)
        {
            //String server = "127.0.0.1";
            //String message = "Hello world";
            ServerHandler.Connect(server, port);

            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your about page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}
