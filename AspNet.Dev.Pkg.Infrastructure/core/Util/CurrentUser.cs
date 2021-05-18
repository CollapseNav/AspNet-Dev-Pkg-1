using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace AspNet.Dev.Pkg.Infrastructure.Util
{
    public class CurrentUser
    {
        private static IHttpContextAccessor _httpContext;
        private IdentityUser<Guid> _user;
        public IdentityUser<Guid> User
        {
            get
            {
                var context = _httpContext.HttpContext;
                var claims = context.User.Claims;
                if (_user != null)
                    return _user;
                if (claims == null || !claims.Any()) return null;
                var user = new IdentityUser<Guid>
                {
                    UserName = claims.FirstOrDefault(item => item.Type == ClaimTypes.Name)?.Value,
                    Id = Guid.TryParse(claims.FirstOrDefault(item => item.Type == ClaimTypes.Sid)?.Value, out Guid id) ? id : Guid.Empty,
                };
                _user = user;
                return _user;
            }
        }
        public static void Init(IHttpContextAccessor httpContext) => _httpContext = httpContext;
    }
}
