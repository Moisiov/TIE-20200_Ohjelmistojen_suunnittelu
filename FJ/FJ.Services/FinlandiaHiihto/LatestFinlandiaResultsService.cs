﻿using System;
using System.Threading.Tasks;
using FJ.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices;

namespace FJ.Services.FinlandiaHiihto
{
    public class LatestFinlandiaResultsService : ILatestFinlandiaResultsService
    {
        private readonly IDataFetchingService m_dataFetchingService;

        public LatestFinlandiaResultsService(IDataFetchingService dataFetchingService)
        {
            m_dataFetchingService = dataFetchingService;
        }

        public async Task<FinlandiaHiihtoResultsCollection> GetLatestFinlandiaResults()
        {
            var currenYear = DateTime.Today.Year;
            var args = new FinlandiaHiihtoSearchArgs
            {
                Years = currenYear.ToMany()
            };

            var result = await m_dataFetchingService.GetFinlandiaHiihtoResults(args);

            if (result.HasAnyResults)
            {
                return result;
            }

            args = new FinlandiaHiihtoSearchArgs
            {
                Years = (currenYear - 1).ToMany()
            };

            return await m_dataFetchingService.GetFinlandiaHiihtoResults(args);
        }
    }
}
