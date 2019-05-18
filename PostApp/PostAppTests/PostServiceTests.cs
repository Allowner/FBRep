using Facebook;
using PostApp.Models;
using PostApp.Service;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace PostAppTests
{
    public class PostServiceTests
    {
        PostService service = new PostService();
        FacebookClient client = new FacebookClient("EAAJMKmLx3ZBwBANzKxrHbWMSjWGEsKNswu3qNDnZBES9o4csvV5ZANQpfLPh20ZBxHbREqnijvcVQ7BNpUXhZBU4FrOrF9XJfFWY2ZAFBNnsVHLYAAluhAny4cr19YX4KxndvccVVqZAZAlxoCpNiRAnBI2PWpDVkv4MBq7K0p407jPWZBGu5TJxzw0mDNmMRZBYki96S11NEZBdAZDZD");

        [Fact]
        public async Task TestWrongFilePath()
        {
            PostBody post = new PostBody()
            {
                PageNameOrId = "683744845377993",
                FilePath = "wrong file",
                DescriptionAndHashtags = "some text"
            };

            await Assert.ThrowsAsync<FileNotFoundException>(() => service.PostPhotoAsync(post, client));
        }

        [Fact]
        public async Task TestWrongPageNameAsync()
        {
            PostBody post = new PostBody()
            {
                PageNameOrId = "8374487993",
                FilePath = "C:\\Users\\Usevalad_Hardziyenka\\Pictures\\water.png",
                DescriptionAndHashtags = "some text"
            };

            await Assert.ThrowsAsync<FacebookApiException>(() => service.PostPhotoAsync(post, client));
        }
        
        [Fact]
        public async Task TestWrongFileExtensionAsync()
        {
            PostBody post = new PostBody()
            {
                PageNameOrId = "683744845377993",
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
                PageNameOrId = "683744845377993",
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
                PageNameOrId = "683744845377993",
                FilePath = "C:\\Users\\Usevalad_Hardziyenka\\Pictures\\water.png"
            };

            await service.PostPhotoAsync(post, client);
        }

        [Fact]
        public async Task TestPngAsync()
        {
            PostBody post = new PostBody()
            {
                PageNameOrId = "683744845377993",
                FilePath = "C:\\Users\\Usevalad_Hardziyenka\\Pictures\\water.png",
                DescriptionAndHashtags = "some text"
            };

            await service.PostPhotoAsync(post, client);
        }
    }
}
