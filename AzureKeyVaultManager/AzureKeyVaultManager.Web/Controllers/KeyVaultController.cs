using AzureKeyVaultManager.Web.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureKeyVaultManager.Web.Controllers
{
    public class KeyVaultController : Controller
    {
        public ActionResult Index(FormCollection collection)
        {
            if (collection.Keys.Count == 4)
            {
                try
                {
                    Session["vaulturl"] = collection["VaultURl"];
                    Session["appid"] = collection["ClientAppId"];
                    Session["appsecret"] = collection["ClientAppSecretKey"];

                    return RedirectToAction("Menu");
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public ActionResult Menu()
        {
            return View();
        }

        #region Secrets

        public ActionResult Secrets()
        {
            try
            {
                var service = CreateVaultService();
                var collection = service.GetSecrets();
                return View(collection);
            }
            catch(Exception ex)
            {
                ViewBag.Exception = ex.Message;
                ViewBag.InnerException = ex.InnerException == null ? String.Empty : ex.InnerException.Message;
                return View("Error");
            }
        }

        public ActionResult SecretDetails(string secretName)
        {
            try
            {
                var service = CreateVaultService();
                var collection = service.GetVersionsOfSecret(secretName);
                return View(collection);
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex.Message;
                ViewBag.InnerException = ex.InnerException == null ? String.Empty : ex.InnerException.Message;
                return View("Error");
            }
        }

        public ActionResult CreateSecret(FormCollection collection)
        {
            try
            {
                if (collection.Keys.Count > 2)
                {
                    var service = CreateVaultService();
                    var name = collection["SecretName"];
                    var value = collection["SecretValue"];
                    var secret = service.CreateSecret(name, value);
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex.Message;
                ViewBag.InnerException = ex.InnerException == null ? String.Empty : ex.InnerException.Message;
                return View("Error");
            }
        }

        public ActionResult DeleteSecret(string secretName)
        {
            try
            {
                var service = CreateVaultService();
                service.DeleteSecret(secretName);
                return RedirectToAction("Secrets");
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex.Message;
                ViewBag.InnerException = ex.InnerException == null ? String.Empty : ex.InnerException.Message;
                return View("Error");
            }
        }

        #endregion

        #region Keys

        public ActionResult Keys()
        {
            try
            {
                var service = CreateVaultService();
                var colection = service.GetKeys();
                return View(colection);
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex.Message;
                ViewBag.InnerException = ex.InnerException == null ? String.Empty : ex.InnerException.Message;
                return View("Error");
            }
        }

        #endregion

        private KeyVaultService CreateVaultService()
        {
            string url = Session["vaulturl"].ToString();
            string appId = Session["appid"].ToString();
            string appSecret = Session["appsecret"].ToString();

            return new KeyVaultService(url, appId, appSecret);
        }
    }
}
