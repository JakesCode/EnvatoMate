using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using RestSharp;

namespace Envato_API
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public System.Windows.Forms.NotifyIcon ico { get; set; }
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

        private void makeRequest()
        {
            var param = File.ReadAllLines("auth.dat");
            Newtonsoft.Json.Linq.JObject RESPONSE = Newtonsoft.Json.Linq.JObject.Parse(param[0]);
            var client = new RestClient("https://api.envato.com/v1/market/private/user/account.json");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + RESPONSE.GetValue("access_token"));
            IRestResponse response = client.Execute(request);

            // Check Response //
            Newtonsoft.Json.Linq.JObject ENVATO_RESPONSE = Newtonsoft.Json.Linq.JObject.Parse(response.Content);
            var fail = ENVATO_RESPONSE.GetValue("error_description");
            // End of checking //
        }
    }
}
