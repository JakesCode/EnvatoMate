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
        public PhotoDuneImageSelect(JToken value)
        {
            InitializeComponent();
            loadImages(value);
        }

        public void loadImages(JToken value)
        {
            //IMG_BOX_1.ImageLocation = value["previews"]["thumbnail_preview"]["large_url"].ToString();
            MessageBox.Show(value.ToString());
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
