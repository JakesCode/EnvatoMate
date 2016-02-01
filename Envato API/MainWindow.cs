using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using RestSharp;
using System.Runtime.InteropServices;

namespace Envato_API
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public System.Windows.Forms.NotifyIcon ico { get; set; }
        public static string accessToken { get; set; }
        private void MainWindow_Load(object sender, EventArgs e)
        {
            // Little bit of background colouring work //
            this.BackColor = Color.FromArgb(34, 34, 34);
            
            // Ok, let's go! //
            var sysTrayMenu = new ContextMenu();
            sysTrayMenu.MenuItems.Add("Open EnvatoMate", showMainWindow);
            sysTrayMenu.MenuItems.Add("Quit EnvatoMate", quitEnvatoMate);
            var sysTrayIcon = new NotifyIcon();
            sysTrayIcon.Text = "EnvatoMate";
            sysTrayIcon.Icon = new Icon("ico.ico", 40, 40);   
            sysTrayIcon.ContextMenu = sysTrayMenu;
            sysTrayIcon.Visible = true;
            ico = sysTrayIcon;

            makeRequest();
        }

        private void showMainWindow(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Normal;
        }

        private void quitEnvatoMate(object sender, EventArgs e)
        {
            this.Dispose();
            ico.Visible = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
            ico.Visible = false;
        }

        private Newtonsoft.Json.Linq.JObject refreshedResponse { get; set; }
        public async void makeRequest()
        {
            var param = File.ReadAllLines("auth.dat");
            Newtonsoft.Json.Linq.JObject RESPONSE = Newtonsoft.Json.Linq.JObject.Parse(param[0]);
            using (var newClient = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    {"grant_type", "refresh_token"},
                    {"refresh_token", (string)RESPONSE["refresh_token"]},
                    {"client_id", "envatomate-3ur0bxxw"},
                    {"client_secret", "hhlB6MdIT5xCEy3wlwwbOaEK2XryGYqe"}
                };
                var content = new FormUrlEncodedContent(values);
                var response = await newClient.PostAsync("https://api.envato.com/token", content);
                var responseString = await response.Content.ReadAsStringAsync();
                Newtonsoft.Json.Linq.JObject NEWRESPONSE = Newtonsoft.Json.Linq.JObject.Parse(responseString);
                refreshedResponse = NEWRESPONSE;
                accessToken = (string)refreshedResponse["access_token"];
            }

            var client = new RestClient("https://api.envato.com/v1/market/private/user/account.json");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + refreshedResponse.GetValue("access_token"));
            IRestResponse newResponse = client.Execute(request);


            // Check Response //
            Newtonsoft.Json.Linq.JObject ENVATO_RESPONSE = Newtonsoft.Json.Linq.JObject.Parse(newResponse.Content);
            //DEBUG - MessageBox.Show(ENVATO_RESPONSE.ToString());//
            // End of checking //

            // Populate the form //
            welcomeLabel.Text = ("Welcome, " + (string)ENVATO_RESPONSE["account"]["firstname"] + ".");
            profileImg.ImageLocation = (string)ENVATO_RESPONSE["account"]["image"];
            UI_BALANCE.Text = "£" + (string)ENVATO_RESPONSE["account"]["balance"];
            // End of populating //
        }

        private void label1_Click(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText(@"auth.dat", string.Empty);
            this.Dispose();
        }

        private void photoduneImage_Click(object sender, EventArgs e)
        {
            if (!(welcomeLabel.Text == "USER"))
            {
                PhotoDune photoduneWindow = new PhotoDune();
                photoduneWindow.ShowDialog();
            } else
            {
                MessageBox.Show("NvatoMate is currently conversing with Envato's Servers." + Environment.NewLine + "Please try again shortly.", "Background work in progress", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void profileImg_Click(object sender, EventArgs e)
        {

        }

        private void UI_BALANCE_Click(object sender, EventArgs e)
        {

        }
    }
}
