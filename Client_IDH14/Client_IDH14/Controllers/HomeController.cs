﻿using Client_IDH14.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace Client_IDH14.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Make list so files can be shown on frontend
            var model = new List<FileHandler>();

            // Set c so folder can be checked by client
            DirectoryInfo c = new DirectoryInfo(@"C:\idh14Client\");

            //Get all files
            FileInfo[] Files2 = c.GetFiles("*.*");

            foreach (FileInfo file in Files2)
            {
                FileHandler tempFile = new FileHandler();
                tempFile.FileName = file.Name;

                string filePath = c + file.Name;

                //Show SHA1 hash of current version of the file
                tempFile.Checksum = FileHandler.GetSha1Hash(filePath);
                model.Add(tempFile);
            }

            //Update checksums.csv file
            FileHandler.UpdateChecksums();

            return View(model);
        }


        public ActionResult Connect(string server, string port)
        {
            ServerHandler.Connect(server, port);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GetListServer(string server, string port)
        {
            ServerHandler.GetList(server, port);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GetFile(string server, string port, string selectedFile)
        {
            if (selectedFile != null)
            {
                ServerHandler.GetFile(server, port, selectedFile);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult PutFile(string server, string port, string selectedFile)
        {
            if (selectedFile != null)
            {
                ServerHandler.PutFile(server, port, selectedFile);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteFile(string server, string port, string selectedFile)
        {
            if (selectedFile != null)
            {
                ServerHandler.DeleteFile(server, port, selectedFile);
            }
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
