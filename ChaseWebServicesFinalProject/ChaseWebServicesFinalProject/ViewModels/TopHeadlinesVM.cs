using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChaseWebServicesFinalProject.ViewModels
{
    public class TopHeadlinesVM : NotifyClass
    {
        public ObservableCollection<Articles> topHeadlines = new ObservableCollection<Articles>();
        public ObservableCollection<Articles> TopHeadlines
        {
            get => topHeadlines;
            set
            {
                if (topHeadlines == value) return;

                topHeadlines = value;
                NotifyPropertyChanged();
            }
        }

        public TopHeadlinesVM()
        {
            TopHeadlines.CollectionChanged += delegate { NotifyPropertyChanged(); };
            GetTopHeadlines();
        }

        /// <summary>
        ///     Gets the news from our API endpoint and Deserializes it from JSON form to C# objects.
        ///         Then it is assigned to our collection.
        /// </summary>
        private async void GetTopHeadlines()
        {
            await Task.Run(async () =>
            {
                HttpClient client = new HttpClient();
                var resultString = await client.GetAsync($"https://newsapi.org/v2/top-headlines?country=us&apiKey={App.Token}");
                string jsonContent = await resultString.Content.ReadAsStringAsync();

                Application resultObject = JsonConvert.DeserializeObject<Application>(jsonContent);

                // If this result isn't already inside the collection the user sees, then add it.
                foreach (var result in resultObject.articles)
                {
                    if (!TopHeadlines.Contains(result))
                    {
                        TopHeadlines.Add(result);
                    }
                }
            });
        }
    }

    public class Source
    {
        public string id { get; set; }
        public string name { get; set; }
 
    }
    public class Articles
    {
        public Source source { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string urlToImage { get; set; }
        public DateTime publishedAt { get; set; }
        public string content { get; set; }

    }
    public class Application
    {
        public string status { get; set; }
        public int totalResults { get; set; }
        public IList<Articles> articles { get; set; }

    }
}
