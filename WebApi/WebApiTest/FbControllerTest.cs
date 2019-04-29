using Facebook;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using WebApi;
using WebApi.Controllers;
using WebApi.Services;
using Xunit;

namespace WebApiTest
{
    public class FbControllerTest
    {
        FacebookService service = new FacebookService();
        FacebookClient client = new FacebookClient("EAAflPqbmppQBAJbGa8gjZCkNaEMGCrJW9IRkhGTmSohRUjG7eQfbWDjZAhQyVi1VhlCvjflkQy69p4pWFemGBoprEZBV2dBZARBwZAodIXk6udFcRrjMynDXi2jtEPQgEQQ9HeUpUeCnxbYW85ZAKyr0w7mhZC3XiELgJq6s4Pizv6M9dEVgRmVgGzCvOxy6gQZD");

        [Fact]
        public async Task TestWrongFilePath()
        {
            PostBody post = new PostBody()
            {
                FilePath = "wrong file",
                DescriptionAndHashtags = "some text"
            };

            await Assert.ThrowsAsync<FileNotFoundException>(() => service.PostPhotoAsync(post, client));
        }

        [Fact]
        public async Task TestWrongFileExtensionAsync()
        {
            PostBody post = new PostBody()
            {
                FilePath = "C:\\Users\\Usevalad_Hardziyenka\\Pictures\\desktop.ini",
                DescriptionAndHashtags = "some text"
            };

            await Assert.ThrowsAsync<FileLoadException>(() => service.PostPhotoAsync(post, client));
        }

        [Fact]
        public async Task TestJpgAsync()
        {
            PostBody post = new PostBody()
            {
                FilePath = "C:\\Users\\Usevalad_Hardziyenka\\Pictures\\images.jpg",
                DescriptionAndHashtags = "some text"
            };

            await service.PostPhotoAsync(post, client);
        }

        [Fact]
        public async Task TestOnlyPictureAsync()
        {
            PostBody post = new PostBody()
            {
                FilePath = "C:\\Users\\Usevalad_Hardziyenka\\Pictures\\water.png"
            };

            await service.PostPhotoAsync(post, client);
        }

        [Fact]
        public async Task TestPngAsync()
        {
            PostBody post = new PostBody()
            {
                FilePath = "C:\\Users\\Usevalad_Hardziyenka\\Pictures\\water.png",
                DescriptionAndHashtags = "some text"
            };

            await service.PostPhotoAsync(post, client);
        }
    }
}
