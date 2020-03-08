﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Enums;
using FJ.ServiceInterfaces.FinlandiaHiihto;

namespace FJ.Client.Models
{
    public class ResultRegisterModel
    {
        private readonly ILatestFinlandiaResultsService m_latestFinlandiaResultsService;

        public ResultRegisterModel(ILatestFinlandiaResultsService latestFinlandiaResultsService)
        {
            m_latestFinlandiaResultsService = latestFinlandiaResultsService;
        }

        public async Task<IEnumerable<ResultRegisterItemModel>> GetLatestFinlandiaResultsAsync()
        {
            // TODO this is just a proof of concept
            var collection = await m_latestFinlandiaResultsService.GetLatestFinlandiaResultsAsync();
            var res = collection.Results
                .Where(x => x.Distance == FinlandiaSkiingDistance.Fifty && x.Style == FinlandiaSkiingStyle.Classic)
                .OrderBy(x => x.PositionGeneral)
                .Take(100)
                .Select(x => new ResultRegisterItemModel
                {
                    Name = x.FullName,
                    Position = x.PositionGeneral,
                    StyleAndDistance = x.StyleAndDistanceString,
                    ResultTime = x.Result.ToString(@"hh\:mm\:ss\.ff"),
                    Year = x.Year
                });

            return res;
        }
    }
}
