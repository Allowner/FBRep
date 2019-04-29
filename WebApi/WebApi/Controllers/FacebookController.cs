using System;
using Facebook;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Options;
using WebApi.Services;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacebookController : ControllerBase
    {
        private readonly FacebookClient client;
        private readonly Connection access_token;
        private readonly IService service;

        public FacebookController(IOptions<Connection> options, IService service)
        {
            this.service = service ?? throw new ArgumentException(nameof(service));
            access_token = options.Value ?? throw new ArgumentException(nameof(options));
            client = new FacebookClient(access_token.PageAccessToken);
        }

        [HttpPost]
        public async Task PostAsync([FromBody] PostBody info)
        {
            await service.PostPhotoAsync(info, client);
        }
    }
}















/*string accessToken = "EAAflPqbmppQBALjNSdzqOmkyUSPqbFlZBbH1qRTLRqyBz1sxzZAsLoGIHFndmJT0yz9ayrJYiy5ZA5YPchscCrfamGWHi3LOlVkh1sbkYC4eEc2VzPQjPh1suy7j8bC5X7p3UwmYiZCtSpdrD9V5iik7oo1z4VY0b2GHouoDuPnEgnGqN57MtGE4kMf20q5c1E1YCTWobuf0IrGeJE9ArjiWvGJsOOoZD";
            client = new FacebookClient(accessToken)
            {
                AppId = "2222382087841428",
                AppSecret = "67a0199255c508464ab5450ab7c5ef25"
            };

            if (!System.IO.File.Exists(info.FilePath))
            {
                throw new ArgumentException("Filepath is invalid.");
            }

            byte[] photo = System.IO.File.ReadAllBytes(info.FilePath);
            dynamic parameters = new ExpandoObject();
            parameters.message = info.DescriptionAndHashtags;
            var mediaObject = new FacebookMediaObject
            {
                FileName = info.FileName, 
                ContentType = "image/jpeg"
            };
            mediaObject.SetValue(photo);

            IDictionary<string, object> upload = new Dictionary<string, object>();
            upload.Add("name", info.Caption);
            upload.Add(info.FileName, mediaObject);

            dynamic res = client.Post("me/feed", upload) as JsonObject;
            
     
     
     
     
     
     
     client = new FacebookClient();
            var result = client.Get("oauth/", new
            {
                client_id = access_token.AppId,
                redirect_uri = "https://localhost:44313/signin-facebook/",
                state = "{st=state123abc,ds=123456789}"
            });

            var pAccess = client.Get($"veryunusualname?fields={access_token.AppId}", new
            {
                client_id = access_token.AppId,
                client_secret = access_token.AppSecret
            });*/
