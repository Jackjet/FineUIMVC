using System.Web.Mvc;

namespace FineUIMvc.PumpMVC.Areas.ShuiChangInfo
{
    public class ShuiChangInfoAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ShuiChangInfo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ShuiChangInfo_default",
                "ShuiChangInfo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}