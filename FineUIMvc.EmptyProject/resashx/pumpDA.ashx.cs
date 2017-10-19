using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Data.Entity;
using Newtonsoft.Json.Linq;
using FineUIMvc;
using FineUIMvc.PumpMVC.Controllers;
using FineUIMvc.PumpMVC.Models;

namespace FineUIMvc.PumpMVC.resashx
{
    /// <summary>
    /// pumpDA 的摘要说明
    /// </summary>
    public class pumpDA : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (context.Request.Files.Count > 0)
            {
                string owner = context.Request.Form["owner"];

                if (!String.IsNullOrEmpty(owner))
                {
                    HttpPostedFile postedFile = context.Request.Files[0];
                    JObject fileObj = new JObject();

                    // 文件名完整路径
                    string fileName = postedFile.FileName;

                    // 文件名保存的服务器路径
                    string savedFileName = GetSavedFileName(fileName);
                    string path = string.Empty;

                    JObject jo = JObject.Parse(owner);

                    string uploadPageType = jo["uploadPageType"].ToString();
                    string baseId = jo["baseId"].ToString();
                    string pageType = jo["pageType"].ToString();
                    string filetype = string.Empty; ;
                    switch (uploadPageType)
                    {
                        case "pumpattach":
                            path = "~/PublicFile/PumpAttch/";
                            filetype = context.Request.Form["filetype1"].ToString();
                            if(filetype.Equals("0"))
                            {
                                context.Response.Write("No file");
                                return;
                            }
                            break;
                        case "pumppic":
                            path = "~/PublicFile/PumpPic/";
                            filetype = context.Request.Form["filetype2"].ToString();
                            if (filetype.Equals("0"))
                            {
                                context.Response.Write("No file");
                                return;
                            }
                            break;

                        default:

                            break;
                    }

                    if (!Directory.Exists(context.Server.MapPath(path)))//判断是否存在
                    {
                        Directory.CreateDirectory(context.Server.MapPath(path));//创建新路径
                    }
                    postedFile.SaveAs(context.Server.MapPath(path) + savedFileName);
                    fileObj.Add("FileName", GetFileName(fileName));
                    fileObj.Add("FileType", GetFileType(fileName));
                    fileObj.Add("FileType2", filetype);
                    fileObj.Add("FilePath", path + savedFileName);
                    fileObj.Add("FileSize", postedFile.ContentLength);
                    fileObj.Add("BaseId", baseId);
                    fileObj.Add("FPageSource", pageType);
                    fileObj.Add("uploadPageType", uploadPageType);
                    SaveToDatabase(context, baseId, fileObj);

                    context.Response.Write("Success");
                    return;
                }
            }
            // 出错了
            context.Response.StatusCode = 500;
            context.Response.Write("No file");
        }
        //// 模拟在服务器端保存数据
        //// 特别注意：在真实的开发环境中，不要在Session放置大量数据，否则会严重影响服务器性能
        private void SaveToDatabase(HttpContext context, string PumpId, JObject fileObj)
        {
            DBController db = new DBController();
            Panda_PumpDA item = new Panda_PumpDA();

            item.BaseId = fileObj["BaseId"].ToString();
            item.FPageSource = fileObj["FPageSource"].ToString();

            item.FileName = fileObj["FileName"].ToString();

            item.FileType = fileObj["FileType"].ToString();

            item.FileType2 = fileObj["FileType2"].ToString();

            item.uploadPageType = fileObj["uploadPageType"].ToString();

            item.FilePath = fileObj["FilePath"].ToString();

            item.FileSize = fileObj["FileSize"].ToString();

            item.UpDateTime = DateTime.Now;

            db.Panda_PumpDA.Add(item);
            db.SaveChanges();

        }


        private string GetFileType(string fileName)
        {
            string fileType = String.Empty;
            int lastDotIndex = fileName.LastIndexOf(".");
            if (lastDotIndex >= 0)
            {
                fileType = fileName.Substring(lastDotIndex + 1).ToLower();
            }

            return fileType;
        }

        private string GetFileName(string fileName)
        {
            string shortFileName = fileName;
            int lastSlashIndex = shortFileName.LastIndexOf("\\");
            if (lastSlashIndex >= 0)
            {
                shortFileName = shortFileName.Substring(lastSlashIndex + 1);
            }

            return shortFileName;
        }

        private string GetSavedFileName(string fileName)
        {
            fileName = fileName.Replace(":", "_").Replace(" ", "_").Replace("\\", "_").Replace("/", "_");
            fileName = DateTime.Now.Ticks.ToString() + "_" + fileName;

            return fileName;
        }



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}