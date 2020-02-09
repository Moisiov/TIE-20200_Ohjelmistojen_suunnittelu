using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;

using HtmlAgilityPack;
// TODO: Logging

namespace FJ.FinlandiaHiihtoAPI
{
    internal class FinlandiaHiihtoScraper : IFinlandiaHiihtoScraper
    {
        private static readonly HttpClient s_client = new HttpClient();

        public async Task<Dictionary<string, string>> GetRequestBaseData()
        {
            // Load the result archive page with get request.
            HttpResponseMessage response;
            try
            {
                response = await s_client.GetAsync(Resources.C_Url);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                // TODO: Throw custom exception about not having access to the site? Or do custom logging here?
                throw;
            }

            // Pass the response to a html parser.
            string html = await response.Content.ReadAsStringAsync();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

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

                string val = input.Attributes.Contains("value") ? input.Attributes["value"].Value : "";
                formValues.Add(input.Attributes["name"].Value, val);
            }

            foreach (var select in selects)
            {
                if (formValues.Keys.Contains(select.Attributes["name"].Value))
                {
                    continue;
                }

                string val = select.Attributes.Contains("value") ? select.Attributes["value"].Value : "kaikki";
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

            return formValues;
        }

        public async Task<IEnumerable<Dictionary<string, string>>> FetchData(
            Dictionary<string, string> form,
            IEnumerable<string> constraints)
        {
            // Edit the form data with wanted constraints.
            AddConstraints(form, constraints.ToList());

            // Send POST-request to the archive page with the form data.
            HttpResponseMessage response;
            try
            {
                var data = new FormUrlEncodedContent(form);
                response = await s_client.PostAsync(Resources.C_Url, data);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                // TODO: Throw custom exception about not having access to the site? Or do custom logging here?
                throw;
            }

            // Pass the response to a html parser.
            string html = await response.Content.ReadAsStringAsync();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            // TODO: Check if too much data or something went wrong with request.

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
            var parsedData = new List<Dictionary<string, string>>();
            foreach (var row in tableRows)
            {
                var cellValues = row.Descendants("td").Select(x => x.InnerText).ToList();
                var rowData = new Dictionary<string, string>();

                for (var i = 0; i < cellValues.Count; i++)
                {
                    var value = cellValues[i] != "&nbsp;" ? cellValues[i] : "";
                    rowData.Add(Resources.S_DataHeaders[i], value);
                }

                parsedData.Add(rowData);
            }

            return parsedData;
        }

        private void AddConstraints(
            Dictionary<string, string> form,
            List<string> constraints)
        {
            for (var i = 0; i < constraints.Count; i++)
            {
                string constraintValue = constraints[i];
                if (constraintValue == null)
                {
                    continue;
                }

                form[Resources.S_RequestFieldNames[i]] = constraintValue;
            }
        }
    }
}
