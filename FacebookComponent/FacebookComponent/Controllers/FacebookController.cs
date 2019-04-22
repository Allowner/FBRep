using Microsoft.AspNetCore.Mvc;
using Facebook;
using System.Dynamic;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace FacebookComponent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacebookController : ControllerBase
    {
        private FacebookClient client;
        public FacebookController(IOptions<ConnectionConfig> config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            client = new FacebookClient(config.Value.Access_token)
            {
                AppId = config.Value.App_id,
                AppSecret = config.Value.App_secret
            };
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string filepath, string filename, string textAndHashtags)
        {
            if (!System.IO.File.Exists(filepath))
            {
                throw new ArgumentException("Filepath is invalid.");
            }

            byte[] photo = System.IO.File.ReadAllBytes(filepath);
            dynamic parameters = new ExpandoObject();
            parameters.message = textAndHashtags;
            var mediaObject = new FacebookMediaObject
            {
                FileName = filename
            };
            mediaObject.SetValue(photo);
            parameters.source = mediaObject;

            client.Post("/cognat.heptanirsson/feed", parameters);
        }
    }
}