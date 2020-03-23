using System;
﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects.FinlandiaHiihto.Enums;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Utils.FinlandiaUtils;

namespace FJ.Client.ResultRegister
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
                .Where(x => x.CompetitionClass.Distance == FinlandiaSkiingDistance.Fifty
                    && x.CompetitionClass.Style == FinlandiaSkiingStyle.Classic)
                .OrderBy(x => x.PositionGeneral)
                .Take(100)
                .Select(x => new ResultRegisterItemModel
                {
                    Name = x.Athlete.FullName,
                    Position = x.PositionGeneral,
                    StyleAndDistance = FinlandiaHelpers.GetDistanceAndStyleShortString(
                        x.CompetitionClass.Distance, x.CompetitionClass.Style),
                    ResultTime = x.Result.ToString(@"hh\:mm\:ss\.ff"),
                    FirstName = x.Athlete.FirstName,
                    LastName = x.Athlete.LastName,
                    Year = x.CompetitionInfo.Year
                });

            return res;
        }
    }
}
