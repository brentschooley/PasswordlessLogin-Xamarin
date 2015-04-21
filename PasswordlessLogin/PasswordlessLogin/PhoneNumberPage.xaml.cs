using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

using Xamarin.Forms;

namespace PasswordlessLogin
{
    public partial class PhoneNumberPage : ContentPage
    {
        HttpClient client;

        public PhoneNumberPage()
        {
            InitializeComponent();
            NextButton.Clicked += NextButton_Clicked;
        }

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("phone_num", PhoneNumberEntry.Text)
        });
            var result = await client.PostAsync("*** Your backend base URL ***/user/validate", content);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var definition = new { Success = true };
                var response = JsonConvert.DeserializeAnonymousType(result.Content.ReadAsStringAsync().Result, definition);

                if (response.Success)
                {
                    await Navigation.PushAsync(new TokenPage(client));
                }
                else
                {
                    await DisplayAlert("Phone Number Invalid", "You entered an invalid phone number, try again.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Backend Problem", "Did not receive successful response from backend.", "OK");
            }
        }
    }
}
