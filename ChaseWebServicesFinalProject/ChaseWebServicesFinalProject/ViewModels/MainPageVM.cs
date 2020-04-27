using ChaseWebServicesFinalProject.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ChaseWebServicesFinalProject.ViewModels
{
    public class MainPageVM : NotifyClass
    {
        private ObservableCollection<RSSFeedObject> rssFeedObject;
        public ObservableCollection<RSSFeedObject> RSSFeedObjects
        {
            get => rssFeedObject;
            set
            {
                if (rssFeedObject == value) return;

                rssFeedObject = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand GetSourceCMD => new Command(async (e) =>
        {
            await Browser.OpenAsync((string)e);
        });

        public MainPageVM()
        {
            RSSFeedObjects = new ObservableCollection<RSSFeedObject>();
            RSSFeedObjects.CollectionChanged += delegate { NotifyPropertyChanged(); };


            // Getting the RSS feed information.
            GetRSSFeedAsync();
        }

        /// <summary>
        ///     Getting the covid 19 rss feed asynchronously.
        /// </summary>
        private async void GetRSSFeedAsync()
        {
            await Task.Run(async () =>
            {
                HttpClient client = new HttpClient();
                var result = await client.GetAsync("https://www.cnbc.com/id/100003114/device/rss/rss.html");

                var xml = await result.Content.ReadAsStringAsync();

                XmlDocument rssXmlDoc = new XmlDocument();

                // Load the RSS file from the RSS URL
                rssXmlDoc.LoadXml(xml);

                // Parse the Items in the RSS file
                XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");

                // Iterate through the items in the RSS file
                foreach (XmlNode rssNode in rssNodes)
                {
                    XmlNode rssSubNode = rssNode.SelectSingleNode("title");
                    string title = rssSubNode != null ? rssSubNode.InnerText : "";

                    rssSubNode = rssNode.SelectSingleNode("link");
                    string link = rssSubNode != null ? rssSubNode.InnerText : "";

                    rssSubNode = rssNode.SelectSingleNode("description");
                    string description = rssSubNode != null ? rssSubNode.InnerText : "";

                    // If this RSSFeedObject already exist (same link) don't add it and continue iterating.
                    if (!RSSFeedObjects.Any(x => x.Link == link))
                    {

                        RSSFeedObjects.Add(new RSSFeedObject
                        {
                            Title = title,
                            Link = link,
                            Description = description
                        });

                    }
                }
            });                     
        }
    }
}
