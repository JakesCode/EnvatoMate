using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using RestSharp;
using System.Runtime.InteropServices;
using Envato_API;
using System.Web;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Envato_API
{
    public partial class PhotoDuneImageSelect : Form
    {
        
        public PhotoDuneImageSelect(JObject ENVATO_RESPONSE)
        {
            InitializeComponent();
            loadImages(ENVATO_RESPONSE);
        }

        public JToken CLICKED { get; set; }
        public JObject NEWRESONSE { get; set; }
        public void loadImages(JObject ENVATO_RESPONSE)
        {
            NEWRESONSE = ENVATO_RESPONSE;

            int root = 0;

            Dictionary<string, string> VALUES = new Dictionary<string, string>();
            IMG_BOX_1.ImageLocation = ENVATO_RESPONSE["matches"][0]["previews"]["thumbnail_preview"]["large_url"].ToString();
            IMG_BOX_2.ImageLocation = ENVATO_RESPONSE["matches"][root+1]["previews"]["thumbnail_preview"]["large_url"].ToString();
            IMG_BOX_3.ImageLocation = ENVATO_RESPONSE["matches"][root+2]["previews"]["thumbnail_preview"]["large_url"].ToString();
            IMG_BOX_4.ImageLocation = ENVATO_RESPONSE["matches"][root+3]["previews"]["thumbnail_preview"]["large_url"].ToString();
            IMG_BOX_5.ImageLocation = ENVATO_RESPONSE["matches"][root+4]["previews"]["thumbnail_preview"]["large_url"].ToString();
            IMG_BOX_6.ImageLocation = ENVATO_RESPONSE["matches"][root+5]["previews"]["thumbnail_preview"]["large_url"].ToString();
            IMG_BOX_7.ImageLocation = ENVATO_RESPONSE["matches"][root+6]["previews"]["thumbnail_preview"]["large_url"].ToString();
            IMG_BOX_8.ImageLocation = ENVATO_RESPONSE["matches"][root+7]["previews"]["thumbnail_preview"]["large_url"].ToString();
            IMG_BOX_9.ImageLocation = ENVATO_RESPONSE["matches"][root+8]["previews"]["thumbnail_preview"]["large_url"].ToString();
            IMG_BOX_10.ImageLocation = ENVATO_RESPONSE["matches"][root+9]["previews"]["thumbnail_preview"]["large_url"].ToString();
            IMG_BOX_11.ImageLocation = ENVATO_RESPONSE["matches"][root+10]["previews"]["thumbnail_preview"]["large_url"].ToString();
            IMG_BOX_12.ImageLocation = ENVATO_RESPONSE["matches"][root+11]["previews"]["thumbnail_preview"]["large_url"].ToString();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void IMG_BOX_1_Click(object sender, EventArgs e)
        {
            CLICKED = NEWRESONSE["matches"][0]["id"];
        }

        private void IMG_BOX_2_Click(object sender, EventArgs e)
        {
            CLICKED = NEWRESONSE["matches"][1]["id"];
        }
    }
}
