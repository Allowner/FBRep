using System;
using Facebook;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Options;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacebookController : ControllerBase
    {
        private readonly FacebookClient client;
        private readonly Connection access_token;

        public FacebookController(IOptions<Connection> options)
        {
            access_token = options.Value ?? throw new ArgumentException(nameof(options));
            client = new FacebookClient(access_token.PageAccessToken);
            Console.WriteLine(access_token.PageAccessToken);
        }

        [HttpPost]
        public void Post([FromBody] PostBody info)
        {
            if (!System.IO.File.Exists(info.FilePath))
            {
                throw new FileNotFoundException("Filepath is invalid.");
            }

            string attachementPath = info.FilePath;
            using (var file = new FacebookMediaStream
            {
                ContentType = "image/jpeg",
                FileName = System.IO.Path.GetFileName(attachementPath)
            }.SetValue(System.IO.File.OpenRead(attachementPath)))
            {
                dynamic result = client.Post("veryunusualname/photos",
                new { message = info.DescriptionAndHashtags, caption = info.Caption, file });
            }
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

            dynamic res = client.Post("me/feed", upload) as JsonObject;*/
