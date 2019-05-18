using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PostApp.Models;
using PostApp.Service;

namespace PostApp.Controllers
{
    [Route("fb")]
    public class PostController : Controller
    {
        private readonly IService service;
        private string accessToken;

        public PostController(IService service)
        {
            this.service = service ?? throw new ArgumentException(nameof(service));
        }

        public string Index()
        {
            FacebookClient fb = new FacebookClient();
            Dictionary<string,object> parameters = new Dictionary<string,object>();
            parameters["client_id"] = "646694885777388";
            parameters["redirect_uri"] = "http://localhost:62058/";
            parameters["response_type"] = "token";
            parameters["scope"] = "manage_pages,publish_pages";

            return fb.GetLoginUrl(parameters).OriginalString;
        }

        [Route("post")]
        public async Task CheckAsync(PostBody info)
        {
            string token = await AuthenticationHttpContextExtensions.GetTokenAsync(HttpContext, "access_token");
            string pageToken = GetBusinessPageAccessToken(token, info.PageNameOrId);
            if (pageToken == null)
            {
                throw new ArgumentException(nameof(pageToken));
            }

            FacebookClient client = new FacebookClient(pageToken);
            await service.PostPhotoAsync(info, client);
        }

        [Route("ptoken")]
        private string GetBusinessPageAccessToken(string userToken, string pagename)
        {
            FacebookClient fb = new FacebookClient(userToken);
            var result = fb.Get("/me/accounts");
            JObject json = JObject.Parse(result.ToString());
            int length = json["data"].Count();
            string token = null;
            for (int i = 0; i < length; i++)
            {
                if (pagename == json["data"][i]["id"].ToString())
                {
                    token = json["data"][i]["access_token"].ToString();
                    break;
                }
            }
            
            return token;
        }
    }
}