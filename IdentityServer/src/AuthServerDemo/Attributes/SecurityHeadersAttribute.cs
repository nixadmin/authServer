using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthServerDemo.Attributes
{
    public class SecurityHeadersAttribute : ActionFilterAttribute
    {
        private const string X_CONTENT_TYPE_OPTIONS = "X-Content-Type-Options";
        private const string X_CONTENT_TYPE_OPTIONS_VALUE = "nosniff";

        private const string X_FRAME_OPTIONS = "X-Content-Type-Options";
        private const string X_FRAME_OPTIONS_VALUE = "SAMEORIGIN";

        private const string DEFAULT_CSP = "default-src 'self'";

        private const string CONTENT_SECURITY_POLICY = "Content-Security-Policy";
        private const string X_CONTENT_SECURITY_POLICY = "X-Content-Security-Policy";

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result;

            if (result is ViewResult)
            {
                if (!context.HttpContext.Response.Headers.ContainsKey(X_CONTENT_TYPE_OPTIONS))
                {
                    context.HttpContext.Response.Headers.Add(X_CONTENT_TYPE_OPTIONS, X_CONTENT_TYPE_OPTIONS_VALUE);
                }
                if (!context.HttpContext.Response.Headers.ContainsKey(X_FRAME_OPTIONS))
                {
                    context.HttpContext.Response.Headers.Add(X_FRAME_OPTIONS, X_FRAME_OPTIONS_VALUE);
                }

                if (!context.HttpContext.Response.Headers.ContainsKey(CONTENT_SECURITY_POLICY))
                {
                    context.HttpContext.Response.Headers.Add(CONTENT_SECURITY_POLICY, DEFAULT_CSP);
                }

                if (!context.HttpContext.Response.Headers.ContainsKey(X_CONTENT_SECURITY_POLICY))
                {
                    context.HttpContext.Response.Headers.Add(X_CONTENT_SECURITY_POLICY, DEFAULT_CSP);
                }
            }
        }
    }
}
