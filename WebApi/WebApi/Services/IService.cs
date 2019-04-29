using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface IService
    {
        Task PostPhotoAsync(PostBody info, FacebookClient client);
    }
}
