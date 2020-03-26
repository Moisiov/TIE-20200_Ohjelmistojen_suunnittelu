using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FinlandiaHiihtoAPI.Exceptions;
using HtmlAgilityPack;
// TODO: Logging

namespace FinlandiaHiihtoAPI
{
    internal class FinlandiaHiihtoScraper : IFinlandiaHiihtoScraper
    {
        private enum HttpMethod
        {
            Get = 0,
            Post = 1,
        }
        
        private static readonly HttpClient s_client = new HttpClient();

        public async Task<Dictionary<string, string>> GetRequestBaseData()
        {
            var htmlDocument = await LoadHtmlDocument(HttpMethod.Get);

            // Search input and select elements from the html which can be used
            // in POST-request.
            var inputs = htmlDocument.DocumentNode
                .Descendants("input")
                .Where(elem =>
                    elem.Attributes["name"].Value.StartsWith("__", StringComparison.Ordinal)
                    || elem.Attributes["name"].Value.StartsWith("dnn", StringComparison.Ordinal))
                .ToList();
            
            var selects = htmlDocument.DocumentNode
                .Descendants("select")
                .Where(elem => elem.Attributes["name"].Value.StartsWith("dnn", StringComparison.Ordinal))
                .ToList();

            // Create a base for the POST-request's form data by collecting names 
            // and values from html elements.
            var formValues = new Dictionary<string, string>();
            foreach (var input in inputs)
            {
                if (formValues.Keys.Contains(input.Attributes["name"].Value))
                {
                    continue;
                }

                var val = input.Attributes.Contains("value") ? input.Attributes["value"].Value : "";
                formValues.Add(input.Attributes["name"].Value, val);
            }

            foreach (var select in selects)
            {
                if (formValues.Keys.Contains(select.Attributes["name"].Value))
                {
                    continue;
                }

                var val = select.Attributes.Contains("value") ? select.Attributes["value"].Value : "kaikki";
                formValues.Add(select.Attributes["name"].Value, val);
            }

            // __VIEWSTATE and __EVENTVALIDATION fields contain server generated
            // strings that need to be passed to the POST-request or else the data
            // fetching won't work.
            if (!formValues.ContainsKey("__VIEWSTATE") || !formValues.ContainsKey("__EVENTVALIDATION"))
            {
                // TODO: Custom Exception here?
                throw new Exception("Can't access site data properly.");
            }

            // These values needs to be set manually for the POST-request.
            formValues.Add("__EVENTTARGET", "dnn$ctr1025$Etusivu$cmdHaeTulokset");
            formValues["dnn$ctr1025$Etusivu$ddlKansalaisuus2x"] = "0";
            formValues["dnn$ctr1025$Etusivu$ddlVuosi2x"] = "kaikki";
            formValues["dnn$ctr1025$Etusivu$ddlMatka2x"] = "kaikki";
            formValues["dnn$ctr1025$Etusivu$chkLstSukupuoli2"] = "kaikki";
            formValues["dnn$ctr1025$Etusivu$ddlIkaluokka2"] = "kaikki";

            return formValues;
        }

        public async Task<IEnumerable<FinlandiaHiihtoAPISearchResultRow>> FetchData(
            Dictionary<string, string> form,
            IEnumerable<string> constraints)
        {
            // Edit the form data with wanted constraints.
            var constraintsList = constraints.ToList();
            AddConstraints(form, constraintsList);
            var htmlDocument = await LoadHtmlDocument(HttpMethod.Post, form);
            
            // Check for error message indicating that there were problems with the form.
            var errorElement = htmlDocument.DocumentNode
                .SelectNodes("//div[@class='dnnFormMessage dnnFormValidationSummary']");
            
            if (errorElement != null)
            {
                // Try getting the data again by refreshing the base form in case
                // __VIEWSTATE or __EVENTVALIDATION has expired.
                form = await GetRequestBaseData();
                AddConstraints(form, constraintsList.ToList());
                htmlDocument = await LoadHtmlDocument(HttpMethod.Post, form);
                
                errorElement = htmlDocument.DocumentNode
                    .SelectNodes("//div[@class='dnnFormMessage dnnFormValidationSummary']");
                
                // If still no success user's filter arguments are invalid. 
                if (errorElement != null)
                {
                    throw new ArgumentException("Invalid filter arguments.");
                }
            }
            
            // Check for over 10 000 results error message.
            var tooManyResultsErrorElement = htmlDocument
                .GetElementbyId("dnn_ctr1025_Etusivu_lblTulosError")
                .SelectNodes("*");

            if (tooManyResultsErrorElement != null)
            {
                throw new TooMuchResultsExceptions("Too many results to show.");
            }

            // From the html file try to find the result table rows.
            List<HtmlNode> tableRows;
            try
            {
                tableRows = htmlDocument.DocumentNode
                    .SelectNodes("//table[@id='dnn_ctr1025_Etusivu_dgrTulokset_ctl00']")
                    .First()
                    .Descendants("tbody")
                    .First()
                    .Descendants("tr")
                    .ToList();
            }
            catch (ArgumentNullException)
            {
                tableRows = new List<HtmlNode>();
            }
            
            // Get data from each html row element.
            var parsedData = new List<FinlandiaHiihtoAPISearchResultRow>();
            foreach (var row in tableRows)
            {
                var cellValues = row.Descendants("td").Select(x => x.InnerText).ToList();
                // No results in table
                if (cellValues.Count == 1) { continue; }
                
                var rowData = new FinlandiaHiihtoAPISearchResultRow(cellValues);
                parsedData.Add(rowData);
            }

            return parsedData;
        }

        private static void AddConstraints(IDictionary<string, string> form, IReadOnlyList<string> constraints)
        {
            for (var i = 0; i < constraints.Count; i++)
            {
                var constraintValue = constraints[i];
                if (constraintValue == null)
                {
                    continue;
                }

                form[Resources.S_RequestFieldNames[i]] = constraintValue;
            }
        }

        private static async Task<HtmlDocument> LoadHtmlDocument(HttpMethod method, Dictionary<string, string> form = null)
        {
            HttpResponseMessage response;
            // Send POST-request to the archive page with the form data.
            try
            {
                switch (method)
                {
                    case HttpMethod.Get:
                        response = await s_client.GetAsync(Resources.C_Url);
                        break;
                    case HttpMethod.Post:
                        var data = new FormUrlEncodedContent(form);
                        response = await s_client.PostAsync(Resources.C_Url, data);
                        break;
                    default:
                        response = await s_client.GetAsync(Resources.C_Url);
                        break;
                }
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
#if DEBUG
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
#endif
                // TODO: Throw custom exception about not having access to the site? Or do custom logging here?
                throw;
            }

            // Pass the response to a html parser.
            var html = await response.Content.ReadAsStringAsync();
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            
            return htmlDocument;
        }
    }
}
