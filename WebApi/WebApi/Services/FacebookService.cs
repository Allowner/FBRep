using Facebook;
using System.IO;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public class FacebookService : IService
    {
        private string possibleExtensions = "jpg|png|jpeg";
        public async Task PostPhotoAsync(PostBody info, FacebookClient client)
        {
            if (!System.IO.File.Exists(info.FilePath))
            {
                throw new FileNotFoundException("Filepath is invalid.");
            }

            string extension = Path.GetExtension(info.FilePath).Substring(1);
            if (!possibleExtensions.Contains($"{extension}"))
            {
                throw new FileLoadException("File extension is invalid.");
            }

            string attachementPath = info.FilePath;
            using (var file = new FacebookMediaStream
            {
                ContentType = $"image/{extension}",
                FileName = System.IO.Path.GetFileName(attachementPath)
            }.SetValue(System.IO.File.OpenRead(attachementPath)))
            {
                dynamic result = await client.PostTaskAsync("me/photos",
                new
                {
                    message = info.DescriptionAndHashtags == null ? "" :
                        info.DescriptionAndHashtags,
                    file
                });
            }
        }
    }
}
