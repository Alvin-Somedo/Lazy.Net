using Lazy.Net.Extension.JwtBearer;
using Lazy.Net.WebApi.Const;
using Microsoft.AspNetCore.Mvc;

namespace Lazy.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SysController : Controller
    {


        [HttpPost("/loginTest")]
        public string Login()
        {
            var claims = new Dictionary<string, object> {
                    {ClaimConst.UserId, "user.Id"},
                    {ClaimConst.TenantId, "user.TenantId"},
                    {ClaimConst.UserName, "user.UserName"},
                    {ClaimConst.RealName, "user.RealName"},
                    {ClaimConst.SuperAdmin, "user.UserType"},
                    {ClaimConst.OrgId, "user.OrgId"},
                    {ClaimConst.OrgName, "user.SysOrg?.Name"},
                    {ClaimConst.OrgLevel, "user.SysOrg?.Level"},
                };
            return JWTEncryption.Encrypt(claims);
        }
    }
}
