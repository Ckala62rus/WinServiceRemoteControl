using Hangfire.Dashboard;
using Hangfire.Annotations;

namespace HangFire.Filters
{
    public class HangFireAuthorizationFilter: IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            bool flag = httpContext.User.Identity.IsAuthenticated && httpContext.User.IsInRole("HangfireAdmin");
            return flag;
            return httpContext.User.Identity.IsAuthenticated && httpContext.User.IsInRole("HangfireAdmin");
        }
    }
}
