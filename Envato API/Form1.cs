using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;

namespace Envato_API
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string text = File.ReadAllText("auth.dat");
            if (text == "")
            {
                OAUTH.Url = new System.Uri("https://api.envato.com/authorization?response_type=code&client_id=envatomate-3ur0bxxw&redirect_uri=https://www.envato.com/");
                OAUTH.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(OAUTH_DocumentCompleted);
            } else
            {
                makeNewWindow();
            }
        }

        private async void OAUTH_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string OAUTHUrl = OAUTH.Url.ToString().Substring(0, 15);
            if (OAUTHUrl.Substring(0, 15) == "https://www.env")
            {
                string param = HttpUtility.ParseQueryString(OAUTH.Url.Query).Get("code");
                string URLStub = "https://api.envato.com/token&grant_type=authorization_code&code=";
                string URLStub2 = "&client_id=envatomate-3ur0bxxw&client_secret=hhlB6MdIT5xCEy3wlwwbOaEK2XryGYqe";
                string URL = URLStub + param + URLStub2;
                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
                    {
                        { "grant_type", "authorization_code" },
                        { "code", param },
                        { "client_id", "envatomate-3ur0bxxw" },
                        { "client_secret", "hhlB6MdIT5xCEy3wlwwbOaEK2XryGYqe" }
                    };
                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync("https://api.envato.com/token", content);
                    var responseString = await response.Content.ReadAsStringAsync();
                    System.IO.File.WriteAllText(@"auth.dat", string.Empty);
                    File.AppendAllText(@"auth.dat", responseString);
                    makeNewWindow();
                }
            }
        }

        private void makeNewWindow()
        {
            this.Hide();
            var main = new MainWindow();
            main.Closed += (s, args) => this.Close();
            main.ShowDialog();
        }
    }
}
