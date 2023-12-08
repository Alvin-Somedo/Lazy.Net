using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tea;

namespace Lazy.Net.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
    {

        [HttpPost("/AliAccessTicket")]
        public string CreateAliQuickBiTicket()
        {
            var ALIBABA_CLOUD_ACCESS_KEY_ID = "ALIBABA_CLOUD_ACCESS_KEY_ID";
            var ALIBABA_CLOUD_ACCESS_KEY_SECRET = "ALIBABA_CLOUD_ACCESS_KEY_SECRET";
            AlibabaCloud.SDK.Quickbi_public20220101.Client client = CreateClient(ALIBABA_CLOUD_ACCESS_KEY_ID, ALIBABA_CLOUD_ACCESS_KEY_SECRET);
            AlibabaCloud.SDK.Quickbi_public20220101.Models.CreateTicketRequest createTicketRequest = new AlibabaCloud.SDK.Quickbi_public20220101.Models.CreateTicketRequest
            {
                WorksId = "111111",
                GlobalParam = "['userid':'grzhang','userarea':['n']]",
            };
            AlibabaCloud.TeaUtil.Models.RuntimeOptions runtime = new AlibabaCloud.TeaUtil.Models.RuntimeOptions();
            try
            {
                // 复制代码运行请自行打印 API 的返回值
               var res = client.CreateTicketWithOptions(createTicketRequest, runtime);
                return res.Body.Result.ToString();
            }
            catch (TeaException error)
            {
                // 错误 message
                Console.WriteLine(error.Message);
                // 诊断地址
                Console.WriteLine(error.Data["Recommend"]);
                AlibabaCloud.TeaUtil.Common.AssertAsString(error.Message);
            }
            catch (Exception _error)
            {
                TeaException error = new TeaException(new Dictionary<string, object>
                {
                    { "message", _error.Message }
                });
                // 错误 message
                Console.WriteLine(error.Message);
                // 诊断地址
                Console.WriteLine(error.Data["Recommend"]);
                AlibabaCloud.TeaUtil.Common.AssertAsString(error.Message);
            }
            return "";
        }

        public AlibabaCloud.SDK.Quickbi_public20220101.Client CreateClient(string accessKeyId, string accessKeySecret)
        {
            AlibabaCloud.OpenApiClient.Models.Config config = new AlibabaCloud.OpenApiClient.Models.Config
            {
                // 必填，您的 AccessKey ID
                AccessKeyId = accessKeyId,
                // 必填，您的 AccessKey Secret
                AccessKeySecret = accessKeySecret,
            };
            // Endpoint 请参考 https://api.aliyun.com/product/quickbi-public
            config.Endpoint = "quickbi-public.cn-hangzhou.aliyuncs.com";
            return new AlibabaCloud.SDK.Quickbi_public20220101.Client(config);
        }




    }


}
