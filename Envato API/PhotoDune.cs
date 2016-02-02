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
    public partial class PhotoDune : Form
    {
        public PhotoDune()
        {
            InitializeComponent();
            populateWindow();
        }

        public string ENVATO_RESPONSE_BEFORE { get; set; }
        public JToken value { get; set; }
        public JObject ENVATO_RESPONSE { get; set; }
        public Dictionary<string, string> parameters { get; set; }
        private async void populateWindow()
        {
            // Misc stuff //
            UI_WEB.Url = new Uri("http://photodune.net/");
            // End of misc stuff //

            string longurl = "https://api.envato.com/v1/discovery/search/search/item";
            var uriBuilder = new UriBuilder(longurl);
            Dictionary<string, string> parameters_1 = new Dictionary<string, string>
            {
                {"site", "photodune.net"}
            };

            parameters = parameters_1;

            // Now let's fill up the parameters list with the list of search terms //
            foreach(KeyValuePair<string, string> VALUE in search)
            {
                parameters_1.Add(VALUE.Key, VALUE.Value);
            }
            // End of populating parameters dictionary //

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            // Iteration time! //
            foreach (KeyValuePair<string, string> entry in parameters_1)
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

            var ENVATO_RESPONSE_1 = Newtonsoft.Json.Linq.JObject.Parse(ENVATO_RESPONSE_BEFORE);
            ENVATO_RESPONSE = ENVATO_RESPONSE_1;
        }

        public void makeCall()
        {
            int counter = 0;

            foreach (var x in ENVATO_RESPONSE)
            {
                if (x.Key == "matches")
                {
                    // We've hit the jackpot! //
                    if (counter == 0)
                    {
                        value = x.Value.First;
                    }
                    else
                    {
                        value = x.Value.Next;
                    }

                    UI_NO_RESULTS.Visible = false;

                    try
                    {
                        var ERROR_CHECK = value["url"];
                    }
                    catch
                    {
                        // No matches found for the given search query! //
                        UI_NO_RESULTS.Visible = true;
                    }

                    if (!UI_NO_RESULTS.Visible)
                    {
                        // Populate the UI //
                        BOX_1.ImageLocation = value["previews"]["thumbnail_preview"]["large_url"].ToString();
                        imgURL = value["url"].ToString();
                        authorURL = value["author_url"].ToString();

                        TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
                        UI_NAME.Text = textInfo.ToTitleCase(value["name"].ToString());
                        UI_DESC.Text = value["description"].ToString();
                        UI_SALES.Text = "Category: " + value["classification"] + "| Sales: " + value["number_of_sales"];
                        UI_USER.Text = value["author_username"].ToString();
                        UI_USER_PIC.ImageLocation = value["author_image"].ToString();

                        UI_TAGS.Text = "";
                        var tags = value["tags"].ToString();
                        foreach (var tag in tags)
                        {
                            var baseString = (tag.ToString()).Replace(",", "").Replace("\"", "").Replace("[", "").Replace("]", "");
                            var baseString2 = Regex.Replace(baseString, @"\t|\n|\r", "");
                            UI_TAGS.Text += Regex.Replace(baseString2, @"\s+", " ");
                        }

                        if (Boolean.Parse(value["trending"].ToString()))
                        {
                            UI_TRENDING.Visible = true;
                            UI_TRENDING_LABEL.Visible = true;
                        }
                        else
                        {
                            UI_TRENDING.Visible = false;
                            UI_TRENDING_LABEL.Visible = false;
                        }

                        SRCH_SIZE.SelectedItem = SRCH_SIZE.Items[0];
                        SRCH_DATE.SelectedItem = SRCH_DATE.Items[0];
                        SRCH_ORTN.SelectedItem = SRCH_ORTN.Items[0];
                        // End of populating UI //

                        UI_LOADING.Visible = false;
                        UI_LOADING_GIF.Visible = false;

                        if (parameters.Count == 1)
                        {
                            // This bit checks the length of the parameters list.
                            // If it is only 1, then it must only contain the "site: photodune" line,
                            // which is there by default.
                            // This means that the user must have hit Search and not specified anything.
                            UI_NO_SEARCH.Visible = true;
                        }
                        else
                        {
                            UI_NO_SEARCH.Visible = false;
                        }

                        counter += 1;
                    }

                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public string imgURL { get; set; }
        public string authorURL { get; set; }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UI_WEB.Url = new Uri(imgURL);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UI_WEB.Url = new Uri("http://photodune.net/cart");
        }

        private void UI_TAGS_TextChanged(object sender, EventArgs e)
        {

        }

        private void UI_USER_Click(object sender, EventArgs e)
        {
            UI_WEB.Url = new Uri(authorURL);
        }

        private void UI_LOADING_Click(object sender, EventArgs e)
        {

        }

        private void UI_LOADING_GIF_Click(object sender, EventArgs e)
        {

        }

        private void PhotoDune_Load(object sender, EventArgs e)
        {

        }

        public Dictionary<string, string> searchTerms = new Dictionary<string, string>();
        public Dictionary<string, string> search = new Dictionary<string, string>();
        public void UI_SBTN_Click(object sender, EventArgs e)
        {
            // You just hit "Search" //
            searchTerms.Clear();
            search.Clear();

            searchTerms.Add("term", SRCH_TERM.Text.ToLower());
            searchTerms.Add("tags", SRCH_TAGS.Text.ToLower());
            searchTerms.Add("colors", SRCH_COLS.Text.ToLower());
            searchTerms.Add("size", SRCH_SIZE.SelectedItem.ToString().ToLower());
            searchTerms.Add("price_min", SRCH_MINP.Text.ToString().ToLower());
            searchTerms.Add("price_max", SRCH_MAXP.Text.ToString().ToLower());
            searchTerms.Add("upload_date", SRCH_DATE.SelectedItem.ToString().ToLower());
            searchTerms.Add("orientation", SRCH_ORTN.SelectedItem.ToString().ToLower());
            searchTerms.Add("username", SRCH_UNAM.Text.ToLower());

            foreach (KeyValuePair<string, string> ITEM in searchTerms)
            {
                if (!(ITEM.Value.Substring(Math.Max(0, ITEM.Value.Length - 4)) == "...."))
                {
                    search.Add(ITEM.Key, ITEM.Value);
                }
            }
            PhotoDuneImageSelect photoSelectWindow = new PhotoDuneImageSelect(ENVATO_RESPONSE);
            photoSelectWindow.ShowDialog();

            makeCall();
            populateWindow();
        }
    }
}
