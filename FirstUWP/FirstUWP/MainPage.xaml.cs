using System.Collections.Generic;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using Windows.Security.Authentication.Web;
using Windows.Foundation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FirstUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private static string serverUrl = "http://localhost:62058/";
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void button_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                string urlString = null;
                
                using (HttpClient client = new HttpClient())
                {
                    urlString = client.GetStringAsync($"{serverUrl}fb").Result;
                }

                Uri startUri = new Uri(urlString);
                Uri endUri = new Uri(serverUrl);

                WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                                                        WebAuthenticationOptions.None,
                                                        startUri,
                                                        endUri);
                string token = null;
                if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    token = OutputToken(WebAuthenticationResult.ResponseData);
                }
                else if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                {
                    throw new Exception("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
                }
                else
                {
                    OutputToken("Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
                }

                using (HttpClient client = new HttpClient())
                {
                    urlString = client.GetStringAsync($"{serverUrl}fb/ptoken?userToken={token}&pagename=683744845377993").Result;
                    client.BaseAddress = new Uri("http://localhost:6740");
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("PageNameOrId", "login"),
                        new KeyValuePair<string, string>("PageAccessToken", "login"),
                        new KeyValuePair<string, string>("DescriptionAndHashtags", "login"),
                        new KeyValuePair<string, string>("FilePath", "login")
                    });

                    var result = await client.PostAsync("/api/Membership/exists", content);
                    string resultContent = await result.Content.ReadAsStringAsync();
                    Console.WriteLine(resultContent);
                }
            }
            catch (Exception)
            {
            }
        }

        private string OutputToken(String stringUri)
        {
            int first = stringUri.IndexOf('=') + 1;
            int last = stringUri.IndexOf('&');
            return stringUri.Substring(first, last - first);
        }
    }
}
