using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using System.Net.Http;
using Newtonsoft.Json;

namespace PasswordlessLogin
{
    public partial class TokenPage : ContentPage
    {
        HttpClient client;

        public TokenPage(HttpClient myclient)
        {
            client = myclient;
            InitializeComponent();
            NextButton.Clicked += NextButton_Clicked;
        }

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            var content = new FormUrlEncodedContent(new[] 
        {
            new KeyValuePair<string, string>("token", TokenEntry.Text)
        });

            var result = await client.PostAsync("*** Your backend base URL ***/user/auth", content);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var definition = new { Success = true };
                var response = JsonConvert.DeserializeAnonymousType(result.Content.ReadAsStringAsync().Result, definition);

                if (response.Success)
                {
                    await DisplayAlert("Successful Auth", "You have authenticated successfully!", "OK");
                }
            }
            else
            {
                await DisplayAlert("Backend Problem", "Did not receive successful response from backend.", "OK");
            }
        }
    }
}
