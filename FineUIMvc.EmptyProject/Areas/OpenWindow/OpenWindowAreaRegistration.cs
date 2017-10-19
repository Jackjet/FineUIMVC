using System.Web.Mvc;

namespace FineUIMvc.PumpMVC.Areas.OpenWindow
{
    public class OpenWindowAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "OpenWindow";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "OpenWindow_default",
                "OpenWindow/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}