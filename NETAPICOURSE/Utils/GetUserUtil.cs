using System.Security.Claims;

namespace dotnet_rpg.Utils
{
    public class GetUserUtil : IGetUserUtil
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public GetUserUtil(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public int GetUserId() => int.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

    }
}
