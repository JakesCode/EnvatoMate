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

namespace Envato_API
{
    public partial class PhotoDune : Form
    {
        public PhotoDune()
        {
            InitializeComponent();
            populateWindow();
        }


        public class Rating
        {
            public double rating { get; set; }
            public int count { get; set; }
        }

        public class ThumbnailPreview
        {
            public string small_url { get; set; }
            public string large_url { get; set; }
            public int large_width { get; set; }
            public int large_height { get; set; }
        }

        public class IconWithThumbnailPreview
        {
            public string icon_url { get; set; }
            public string thumbnail_url { get; set; }
            public int thumbnail_width { get; set; }
            public int thumbnail_height { get; set; }
        }

        public class Previews
        {
            public ThumbnailPreview thumbnail_preview { get; set; }
            public IconWithThumbnailPreview icon_with_thumbnail_preview { get; set; }
        }

        public class Match
        {
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string site { get; set; }
            public string classification { get; set; }
            public string classification_url { get; set; }
            public object price_cents { get; set; }
            public int number_of_sales { get; set; }
            public string author_username { get; set; }
            public string author_url { get; set; }
            public string author_image { get; set; }
            public string url { get; set; }
            public string summary { get; set; }
            public Rating rating { get; set; }
            public string updated_at { get; set; }
            public string published_at { get; set; }
            public bool trending { get; set; }
            public Previews previews { get; set; }
            public List<object> attributes { get; set; }
            public List<string> tags { get; set; }
        }

        public class Links
        {
            public string next_page_url { get; set; }
            public object prev_page_url { get; set; }
            public string first_page_url { get; set; }
            public string last_page_url { get; set; }
        }

        public class PhotoDune_Tag
        {
            public string key { get; set; }
            public int count { get; set; }
            public object description { get; set; }
        }

        public class Color
        {
            public string key { get; set; }
            public int count { get; set; }
            public object description { get; set; }
        }

        public class Date
        {
            public string key { get; set; }
            public int count { get; set; }
            public object description { get; set; }
        }

        public class PhotoDune_Size
        {
            public string key { get; set; }
            public int count { get; set; }
            public object description { get; set; }
        }

        public class SalesCount
        {
            public string key { get; set; }
            public int count { get; set; }
            public object description { get; set; }
        }

        public class Aggregations
        {
            public object category_root_count { get; set; }
            public object category { get; set; }
            public object platform_root_count { get; set; }
            public object platform { get; set; }
            public object file_formats { get; set; }
            public List<PhotoDune_Tag> tags { get; set; }
            public List<Color> colors { get; set; }
            public object rating { get; set; }
            public List<Date> date { get; set; }
            public List<Size> size { get; set; }
            public List<SalesCount> sales_count { get; set; }
            public object cost { get; set; }
            public object length { get; set; }
            public object tempo { get; set; }
            public object alpha { get; set; }
            public object looped { get; set; }
            public object resolution { get; set; }
            public object vocals_in_audio { get; set; }
            public object frame_rate { get; set; }
            public object compatible_with { get; set; }
        }

        public class RootObject
        {
            public int took { get; set; }
            public List<Match> matches { get; set; }
            public bool timed_out { get; set; }
            public int total_hits { get; set; }
            public Links links { get; set; }
            public object author_exists { get; set; }
            public Aggregations aggregations { get; set; }
        }


        public string ENVATO_RESPONSE_BEFORE { get; set; }
        private async void populateWindow()
        {
            textBox1.ScrollBars = ScrollBars.Vertical;
            string longurl = "https://api.envato.com/v1/discovery/search/search/item";
            var uriBuilder = new UriBuilder(longurl);
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"site", "photodune.net"},
                {"username", "samberson"},
                {"size", "m"}
            };
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            // Iteration time! //
            foreach (KeyValuePair<string, string> entry in parameters)
            {
                query[entry.Key] = entry.Value;
            }
            // End of iteration! //
            uriBuilder.Query = query.ToString();
            longurl = uriBuilder.ToString();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + MainWindow.accessToken);
                var response = await client.GetStringAsync(longurl);
                ENVATO_RESPONSE_BEFORE = response;
            }

            var ENVATO_RESPONSE = Newtonsoft.Json.Linq.JObject.Parse(ENVATO_RESPONSE_BEFORE);
            var thumbnailArray = ENVATO_RESPONSE.Children<Newtonsoft.Json.Linq.JProperty>().FirstOrDefault(x => x.Name == "thumbnail").Value;
            textBox1.Text = thumbnailArray[1].ToString();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
