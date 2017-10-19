using System.Web.Mvc;

namespace FineUIMvc.PumpMVC.Areas.JiYaZhanInfo
{
    public class JiYaZhanInfoAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "JiYaZhanInfo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "JiYaZhanInfo_default",
                "JiYaZhanInfo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}