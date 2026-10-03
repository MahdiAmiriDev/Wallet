using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;

namespace Yashil.Infrastructure
{
    public class YashilLogger<T> : IYashilLogger<T>
    {
        private readonly IUserPrincipal _userPrincipal;
        private readonly ILogger<T> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IWebHelper _webHelper;
        public YashilLogger(IUserPrincipal userPrincipal, ILogger<T> logger, IHttpContextAccessor httpContextAccessor, ApplicationConfiguration applicationConfiguration, IWebHelper webHelper)
        {
            _userPrincipal = userPrincipal;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _applicationConfiguration = applicationConfiguration;
            _webHelper = webHelper;
        }
        public virtual void InsertLog(LogLevel logLevel, EventId eventId, string objectName, string shortMessage, Exception exception, LogRecord logRecord = null)
        {
            var userName = _userPrincipal?.UserName;
            InsertLog(logLevel, eventId, objectName, shortMessage, exception, userName, null, logRecord);
        }
        public void InsertLog(LogLevel logLevel, EventId eventId, string objectName, string shortMessage, Exception exception, string userName, int? applicationId = null, LogRecord logRecord = null)
        {
            if (string.IsNullOrEmpty(userName))
            {
                userName = _userPrincipal?.UserName;
            }

            if (!applicationId.HasValue && _userPrincipal != null && _userPrincipal.Id != 0 && _userPrincipal.ApplicationId != 0)
            {
                applicationId = _userPrincipal.ApplicationId;
            }
            else
            {
                if (eventId != (int)Events.SuccessFullLogin && eventId != (int)Events.FailLogin)
                {
                    applicationId = _applicationConfiguration.ApplicationId;
                }
            }
            var userAgent = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                userAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"];
            }
            string ipAddress = _webHelper.GetCurrentIpAddress();
            //if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Connection.RemoteIpAddress != null)
            //    ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            _logger.Log(logLevel, eventId, exception,
                "   {UserName}   {CreationDate}  {ApplicationId}  {ObjectName}  {Action}  {shortMessage}  {IpAddress} {UserAgent}",
                  userName, DateTime.Now, applicationId, objectName, eventId.Name, shortMessage, ipAddress, userAgent);
        }
    }
}
