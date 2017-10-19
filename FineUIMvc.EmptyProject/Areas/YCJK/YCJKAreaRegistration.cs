using System.Web.Mvc;

namespace FineUIMvc.PumpMVC.Areas.YCJK
{
    public class YCJKAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "YCJK";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "YCJK_default",
                "YCJK/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}