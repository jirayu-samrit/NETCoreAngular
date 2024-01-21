using Microsoft.AspNetCore.Mvc.Filters;
using NETCoreAngular.API.Extensions;
using NETCoreAngular.API.Interfaces;

namespace NETCoreAngular.API.Heplers;
public class LogUserActivity : IAsyncActionFilter
{
	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
        var resultContext = await next();

        if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

        var userId = resultContext.HttpContext.User.GetUserId();
        var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserReposioty>();
        var user = await repo.GetUserByIdAsync(userId);
        user.LastActive = DateTime.UtcNow;
        await repo.SaveAllAsync();
	}
}