using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Yashil.Core
{
    /// <summary>
    /// for create post remote we use this class
    /// </summary>
    public class RemotePost
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// input parameters as key value 
        /// </summary>
        private System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();

        /// <summary>
        /// the action url
        /// </summary>
        public string Url = "";

        /// <summary>
        /// methot of request can be POST and GET
        /// </summary>
        public string Method = "post";

        /// <summary>
        /// the name of the form for requesting
        /// </summary>
        public string FormName = "form1";

        /// <summary>
        /// constructor and initializing
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public RemotePost(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        /// <summary>
        ///  use this method to add parameter 
        /// </summary>
        /// <param name="name">the key parameter name</param>
        /// <param name="value">the valuse parameter</param>
        public void Add(string name, string value)
        {
            Inputs.Add(name, value);
        }
        /// <summary>
        /// create post request 
        /// </summary>
        public Task PostAsync()
        {
            var sb = new StringBuilder();
            //create beginning tag
            //sb.Append("<html><head>");
            //sb.Append($"</head><body onload=\"document.getElementById('{FormName}').submit()\">");
            // add formname and method and action url
            sb.Append($"<form id=\"{FormName}\" name=\"{FormName}\" method=\"{Method}\" action=\"{Url}\" accept-charset=\"UTF-8\" >");


            //add parameter
            for (int i = 0; i < Inputs.Keys.Count; i++)
            {
                sb.Append($"<input name=\"{WebUtility.HtmlEncode(Inputs.Keys[i])}\" type=\"hidden\" value=\"{WebUtility.HtmlEncode(Inputs[Inputs.Keys[i]])}\">");
            }
            // add end tag
            sb.Append("</form>");
            //sb.Append("</body></html>");

            sb.Append("<script language=\"javascript\">");
            sb.Append($"document.{FormName}.submit();");
            sb.Append("</script>");

            var httpContext = _httpContextAccessor.HttpContext;
            var response = httpContext.Response;

            //clear response before running the request
            response.Clear();
            response.ContentType = "text/html; charset=utf-8";
            //run post request
            return response.WriteAsync(sb.ToString());

        }

    }

}
