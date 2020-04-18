using System;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Filters;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Utils;

namespace FJ.Client.CompetitionGeneral
{
    public class CompetitionGeneralModel
    {
        private readonly ICompetitionDataService m_competitionDataService;

        public Competition CompetitionInfo => CompetitionResultsCollection.Results.FirstOrDefault()?.CompetitionInfo;
        public int CompetitionParticipants => CompetitionResultsCollection.Results.Count();
        public FinlandiaHiihtoResultsCollection CompetitionResultsCollection { get; private set; }
        
        public CompetitionGeneralModel(ICompetitionDataService competitionDataService)
        {
            m_competitionDataService = competitionDataService;
        }
        
        public async Task GetCompetitionGeneralData(int compYear, FinlandiaHiihtoCompetitionClass compType)
        {
            var yearsFilter = new FinlandiaCompetitionYearsFilter(compYear.ToMany());
            var competitionClassesFilter = new FinlandiaCompetitionClassesFilter(compType.ToMany());
            var filters = new FilterCollection(yearsFilter, competitionClassesFilter);

            CompetitionResultsCollection = await m_competitionDataService.GetCompetitionData(filters);
        }
    }
}
