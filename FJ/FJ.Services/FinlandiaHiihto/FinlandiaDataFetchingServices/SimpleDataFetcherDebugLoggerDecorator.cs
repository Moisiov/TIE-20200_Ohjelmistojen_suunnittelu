using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using Newtonsoft.Json;

namespace FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices
{
    public class SimpleDataFetcherDebugLoggerDecorator : IDataFetchingService
    {
        private static string s_currentAssembly;
        private static string s_className;
        private static string LogRowBeginning(string logLevelStr)
            => $"{DateTime.Now:O} [{logLevelStr}] : @{s_currentAssembly}_{s_className}:";
        
        private readonly IDataFetchingService m_dataFetchingService;

        public SimpleDataFetcherDebugLoggerDecorator(IDataFetchingService dataFetchingService)
        {
            s_currentAssembly = GetType().Assembly.GetName().Name;
            s_className = GetType().FullName;
            
            m_dataFetchingService = dataFetchingService;
        }

        public async Task<FinlandiaHiihtoResultsCollection> GetFinlandiaHiihtoResultsAsync(FilterCollection filters)
        {
            Debug.WriteLine($"{LogRowBeginning("DEBUG")} {nameof(GetFinlandiaHiihtoResultsAsync)} " +
                            $"requested with{Environment.NewLine}{JsonConvert.SerializeObject(filters)}");
            
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                var res = await m_dataFetchingService.GetFinlandiaHiihtoResultsAsync(filters);
                sw.Stop();
                
                Debug.WriteLine($"{LogRowBeginning("DEBUG")} {nameof(GetFinlandiaHiihtoResultsAsync)} " +
                                $"finished returning {res.ResultsCount} results in {sw.ElapsedMilliseconds} ms");

                return res;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{LogRowBeginning("DEBUG")} {nameof(GetFinlandiaHiihtoResultsAsync)} " +
                                $"failed throwing exception of type {e.GetType().Name} {Environment.NewLine}{e.Message}");
                throw;
            }
        }
    }
}
