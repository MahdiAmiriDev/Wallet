using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Yashil.WebHost.Filters
{
    public class AddHeaderFilter : Attribute, IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            /*
             * add some header in response
             */
            //// Protects against XSS injections.
            //context.HttpContext.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
            //// Protects against Clickjacking attacks. ref.
            //// If you are using the AntiForgery token in your application then this token by
            //// default sets X-Frame-Option with value SAMEORIGIN to prevent the site from ClickJacking.
            //context.HttpContext.Response.Headers.Add("X-Frame-Options", "DENY"); // This
            //if (!context.HttpContext.Response.Headers.ContainsKey("X-Frame-Options"))
            //{
            //    context.HttpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");// Or this
            //}
            //context.HttpContext.Response.Headers.Add("Referrer-Policy", "same-origin");
            //context.HttpContext.Response.Headers.Add("Cache-Control", "public; max-age=0; s-maxage=0; must-revalidate; pre-check=0; post-check=0;");
            //context.HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            //context.HttpContext.Response.Headers.Add("Strict-Transport-Security", "preload; max-age=31536000;");

        }
    }
}
