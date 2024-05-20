using EventTicketAPI.Dtos;
using EventTicketAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace EventTicketAPI.Filter
{
    public class RoleFilter : ActionFilterAttribute, IAsyncActionFilter
    {
        private readonly string _allowedroles;
        public RoleFilter(string allowedroles)
        {
            _allowedroles = allowedroles;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var serviceprovider = context.HttpContext.RequestServices;
            using var scope = serviceprovider.CreateScope();
            var _context = scope.ServiceProvider.GetRequiredService<EventTicketContext>();

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var precision = configuration.GetValue<string>("Appsettings:Token");

            if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizenheader))
            {
                var token = authorizenheader.ToString().Replace("Bearer ", "");

                var tokenhandler = new JwtSecurityTokenHandler();
                var validateparameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(precision!)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
                try
                {
                    var principal = tokenhandler.ValidateToken(token, validateparameters, out var validatedToken);
                    var userIdClaim = principal.Claims.FirstOrDefault(x => x.Type.Contains("nameidentifier"));
                    if (userIdClaim != null)
                    {
                        var userid = userIdClaim.Value;

                        var rolesfromdb =_context.Users.Include(x => x.Role).Where(x => x.Id == int.Parse(userid)).Select(x => x.Role.Name.ToLower()).ToArray();

                        var normalizedAllowedRoles = _allowedroles.ToLower();
                        var filteredRolesArray = normalizedAllowedRoles.Split(",");

                        var matchcount = rolesfromdb.Intersect(filteredRolesArray).Count();
                        
                        if (matchcount == 0)
                        {
                            context.Result = new UnauthorizedResult();
                            return;
                        }

                    }
                    else
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }

                }
                catch
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

            }
            else
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            await next();
        }

    }
}
