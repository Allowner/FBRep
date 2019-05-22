using System.Collections.Generic;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using Windows.Security.Authentication.Web;
using Microsoft.Toolkit.Uwp.Services.Facebook;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Globalization.DateTimeFormatting;
using Windows.UI.Popups;

namespace FirstUWP
{
    public sealed partial class MainPage : Page
    {
        private static string serverUrl;

        private static string pageId;

        private static string accessToken;

        private static readonly StorageFolder appFolder = 
            Windows.ApplicationModel.Package.Current.InstalledLocation;

        private static readonly string rootPath = 
            AppDomain.CurrentDomain.BaseDirectory;


        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void PageLoaded(object sender, RoutedEventArgs e)
        {
            StorageFolder folder = await appFolder.GetFolderAsync("Configuration");
            StorageFile file = await folder.GetFileAsync("data.txt");
            List<string> config = new List<string>(await FileIO.ReadLinesAsync(file));
            pageId = config[0];
            serverUrl = config[1];
            accessToken = config[2];
        }

        private async void ButtonPostAsync(object sender, RoutedEventArgs e)
        {
            Error.Visibility = Visibility.Collapsed;
            MessageDialog dialog = null;

            try
            {
                await PostImageAsync();
                dialog = new MessageDialog("Image was successfully posted.");
            }
            catch (HttpRequestException)
            {
                dialog = new MessageDialog("Can't post image, server not available.");
            }
            catch (ArgumentException)
            {
                dialog = new MessageDialog("Server received wrong parameters.");
            }

            await dialog.ShowAsync();
        }

        /*private async Task LogInAsync()
        {
            string urlString = null;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(serverUrl);
            urlString = await client.GetStringAsync("fb");

            Uri startUri = new Uri(urlString);
            Uri endUri = new Uri(serverUrl);

            WebAuthenticationResult WebAuthenticationResult =
                await WebAuthenticationBroker.AuthenticateAsync(
                    WebAuthenticationOptions.None, startUri, endUri);

            if (WebAuthenticationResult.ResponseStatus ==
                WebAuthenticationStatus.Success)
            {
                access_token = OutputToken(WebAuthenticationResult.ResponseData);
            }
            else
            {
                throw new AuthenticationException("Error trying to log in.");
            }

            access_token = await client.GetStringAsync(
                $"fb/ptoken?userToken={access_token}&pagename={tb1.Text}");

            client.Dispose();
        }*/

        private async Task PostImageAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(serverUrl);

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("PageNameOrId", pageId),
                    new KeyValuePair<string, string>("PageAccessToken", accessToken),
                    new KeyValuePair<string, string>("DescriptionAndHashtags",
                        desription.Text),
                    new KeyValuePair<string, string>("FilePath", $"{rootPath}{GetImageName()}")
                });

                var result = await client.PostAsync("fb/post", content);
                string resultContent = result.StatusCode.ToString();
                if (resultContent == "InternalServerError")
                {
                    throw new ArgumentException("Wrong arguments.");
                }
            }
        }

        private void SelfieClick(object sender, RoutedEventArgs e)
        {
            post.Visibility = Visibility.Visible;
            desription.Visibility = Visibility.Visible;
            image.Visibility = Visibility.Visible;
        }

        private string GetImageName()
        {
            BitmapImage bitMap = image.Source as BitmapImage;
            Uri uri = bitMap?.UriSource;
            string path = bitMap.UriSource.AbsolutePath.
                Substring(1).Replace("/", "\\");

            return path;
        }

        /*private string OutputToken(String stringUri)
        {
            int first = stringUri.IndexOf('=') + 1;
            int last = stringUri.IndexOf('&');
            return stringUri.Substring(first, last - first);
        }*/
    }
}
