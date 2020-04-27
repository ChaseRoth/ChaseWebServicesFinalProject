using ChaseWebServicesFinalProject.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace ChaseWebServicesFinalProject.ViewModels
{
    public class PeriodicTableVM : NotifyClass
    {
        private List<Element> queryResults = new List<Element>();
        public List<Element> QueryResults
        {
            get => queryResults;
            set
            {
                if (queryResults == value) return;

                queryResults = value;
                NotifyPropertyChanged();
            }
        }

        public QueryParameter AtomicNumber { get; set; } = new QueryParameter("atomic_number");
        public QueryParameter Name { get; set; } = new QueryParameter("name");
        public QueryParameter Symbol { get; set; } = new QueryParameter("symbol");
        public QueryParameter YearDiscovered { get; set; } = new QueryParameter("year_discovered");

        public string AtomicNumberTxt
        {
            get => AtomicNumber.ParameterValue;
            set => AtomicNumber.ParameterValue = value;
        }
        public string NameTxt
        {
            get => Name.ParameterValue;
            set => Name.ParameterValue = value;
        }
        public string SymbolTxt
        {
            get => Symbol.ParameterValue;
            set => Symbol.ParameterValue = value;
        }
        public string YearDiscoveredTxt
        {
            get => YearDiscovered.ParameterValue;
            set => YearDiscovered.ParameterValue = value;
        }

        public ICommand GetQueryCMD => new Xamarin.Forms.Command(async () => await GetQueryDataFromServerAsync(CreateQueryString()));

        /// <summary>
        ///     Creates a query string from our query properties.
        /// </summary>
        public string CreateQueryString()
        {
            // Const that define the index value of the first parameter.
            const sbyte IS_FIRST_PARAMETER = 0;
            // Collection of parameters.
            var parameters = new List<QueryParameter>();
            string queryStr = string.Empty;

            // Check if the property is null or whitespace, if it is not either then add to parameter collection.
            if (!string.IsNullOrWhiteSpace(AtomicNumber.ParameterValue)) parameters.Add(AtomicNumber);
            if (!string.IsNullOrWhiteSpace(Name.ParameterValue)) parameters.Add(Name);
            if (!string.IsNullOrWhiteSpace(Symbol.ParameterValue)) parameters.Add(Symbol);
            if (!string.IsNullOrWhiteSpace(YearDiscovered.ParameterValue)) parameters.Add(YearDiscovered);
          
            // Iterate parameter collection and append parameters.
            for (int i = 0; i < parameters.Count; i++)
            {
                // First parameter begins with '?'
                // Other paramters begins with '&'
                queryStr += i == IS_FIRST_PARAMETER ? '?' + parameters[i].UrlParameterIdentifier + '=' + parameters[i].ParameterValue : '&' + parameters[i].UrlParameterIdentifier + '=' + parameters[i].ParameterValue;
            }

            // Return full parameter string.
            return queryStr;
        }

        /// <summary>
        ///     Queries the rapidapi endpoint for getting our desired data.
        /// </summary>
        public async Task GetQueryDataFromServerAsync(string _queryString)
        {
            await Task.Run(async () =>
            {
                var client = new HttpClient();
                var jsonStr = await client.GetStringAsync($"https://people.rit.edu/zc2985/WebService/API/element.php{_queryString}");

                if (string.IsNullOrWhiteSpace(jsonStr) || jsonStr.Equals("null")) return;

                // Parsing the json data into C# objects before assigning it to our QueryResults on the mainthread.
                var tempResults = new List<Element>(JsonConvert.DeserializeObject<List<Element>>(jsonStr));
                // Updating QueryResults safely on the mainthread.
                MainThread.BeginInvokeOnMainThread(() => QueryResults = tempResults);
            });
        }
    }

    public class QueryParameter
    {
        /// <summary>
        ///     The idenifier in the url used by the server for your query.
        /// </summary>
        public readonly string UrlParameterIdentifier;
        /// <summary>
        ///     The value in the url used by the server for your query.
        /// </summary>
        public string ParameterValue;

        // Locking the QueryParameter default constructor because you will always have a idenifier and parameter value together as a pair.
        private QueryParameter()
        {
            UrlParameterIdentifier = string.Empty;
            ParameterValue = string.Empty;
        }
        public QueryParameter(string _urlParameterIdentifier)
        {
            UrlParameterIdentifier = _urlParameterIdentifier;
            //ParameterValue = string.Empty;
        }
    }
}
