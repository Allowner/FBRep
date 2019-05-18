using Facebook;
using PostApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostApp.Service
{
    public interface IService
    {
        Task PostPhotoAsync(PostBody info, FacebookClient client);
    }
}
