using System;
using System.Linq;
using System.Linq.Expressions;
using FinlandiaHiihtoAPI;
using FinlandiaHiihtoAPI.Enums;
using FJ.DomainObjects.Filters;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Enums;
using FJ.DomainObjects.FinlandiaHiihto.Filters;

namespace FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices
{
    public class FinlandiaCompetitionYearsFilterSearchImpl
        : FinlandiaCompetitionYearsFilter.ExpandSearchArgsExpressionImplementation
    {
        protected override LambdaExpression GetExpression(FinlandiaCompetitionYearsFilter filter, Type parameterType)
        {
            Expression<Action<FilterSearchComposer<FinlandiaHiihtoAPISearchArgs>>> expr =
                c => c.EscalateBy(nameof(FinlandiaHiihtoAPISearchArgs.Year), filter.Items);
            return expr;
        }
    }

    public class FinlandiaHomeCitiesFilterImpl : FinlandiaHomeCitiesFilter.ExpandSearchArgsExpressionImplementation
    {
        protected override LambdaExpression GetExpression(FinlandiaHomeCitiesFilter filter, Type parameterType)
        {
            Expression<Action<FilterSearchComposer<FinlandiaHiihtoAPISearchArgs>>> expr =
                c => c.EscalateBy(nameof(FinlandiaHiihtoAPISearchArgs.CompetitorHomeTown), filter.SearchStringsList);
            return expr;
        }
    }
    
    public class FinlandiaFullNameFilterImpl : FinlandiaFullNameFilter.ReduceSearchResultsExpressionImplementation
    {
        protected override LambdaExpression GetExpression(FinlandiaFullNameFilter filter, Type parameterType)
        {
            Expression<Func<FinlandiaHiihtoAPISearchResultRow, bool>> expr =
                r => filter.Value == r.FullName;
            return expr;
        }
    }

    public class FinlandiaFirstNamesFilterImpl : FinlandiaFirstNamesFilter.ExpandSearchArgsExpressionImplementation
    {
        protected override LambdaExpression GetExpression(FinlandiaFirstNamesFilter filter, Type parameterType)
        {
            Expression<Action<FilterSearchComposer<FinlandiaHiihtoAPISearchArgs>>> expr =
                c => c.EscalateBy(nameof(FinlandiaHiihtoAPISearchArgs.FirstName), filter.SearchStringsList);
            return expr;
        }
    }
    
    public class FinlandiaLastNamesFilterImpl : FinlandiaLastNamesFilter.ExpandSearchArgsExpressionImplementation
    {
        protected override LambdaExpression GetExpression(FinlandiaLastNamesFilter filter, Type parameterType)
        {
            Expression<Action<FilterSearchComposer<FinlandiaHiihtoAPISearchArgs>>> expr =
                c => c.EscalateBy(nameof(FinlandiaHiihtoAPISearchArgs.LastName), filter.SearchStringsList);
            return expr;
        }
    }

    public class FinlandiaResultTimeRangeFilterImpl
        : FinlandiaResultTimeRangeFilter.ReduceSearchResultsExpressionImplementation
    {
        protected override LambdaExpression GetExpression(FinlandiaResultTimeRangeFilter filter, Type parameterType)
        {
            Expression<Func<FinlandiaHiihtoAPISearchResultRow, bool>> expr =
                r => (!filter.HasMin || filter.Min <= r.Result) && (!filter.HasMax || filter.Max >= r.Result);
            return expr;
        }
    }

    public class FinlandiaPositionRangeGeneralFilterImpl
        : FinlandiaPositionRangeGeneralFilter.ReduceSearchResultsExpressionImplementation
    {
        protected override LambdaExpression GetExpression(FinlandiaPositionRangeGeneralFilter filter, Type parameterType)
        {
            Expression<Func<FinlandiaHiihtoAPISearchResultRow, bool>> expr =
                r => filter.Min <= r.Position && filter.Max >= r.Position;
            return expr;
        }
    }
    
    public class FinlandiaPositionRangeMenFilterFilterImpl
        : FinlandiaPositionRangeMenFilter.ReduceSearchResultsExpressionImplementation
    {
        protected override LambdaExpression GetExpression(FinlandiaPositionRangeMenFilter filter, Type parameterType)
        {
            Expression<Func<FinlandiaHiihtoAPISearchResultRow, bool>> expr =
                r => filter.Min <= r.PositionMen && filter.Max >= r.PositionMen;
            return expr;
        }
    }
    
    public class FinlandiaPositionRangeWomenFilterImpl
        : FinlandiaPositionRangeWomenFilter.ReduceSearchResultsExpressionImplementation
    {
        protected override LambdaExpression GetExpression(FinlandiaPositionRangeWomenFilter filter, Type parameterType)
        {
            Expression<Func<FinlandiaHiihtoAPISearchResultRow, bool>> expr =
                r => filter.Min <= r.PositionWomen && filter.Max >= r.PositionWomen;
            return expr;
        }
    }
    
    public class FinlandiaAgeGroupsFilterImpl : FinlandiaAgeGroupsFilter.ExpandSearchArgsExpressionImplementation
    {
        protected override LambdaExpression GetExpression(FinlandiaAgeGroupsFilter filter, Type parameterType)
        {
            Expression<Action<FilterSearchComposer<FinlandiaHiihtoAPISearchArgs>>> expr =
                c => c.EscalateBy(nameof(FinlandiaHiihtoAPISearchArgs.AgeGroup),
                    filter.EnumValues.Select(x => (FinlandiaAgeGroup)(int)x));
            return expr;
        }
    }
    
    public class FinlandiaYearsOfBirthFilterImpl : FinlandiaYearsOfBirthFilter.ReduceSearchResultsExpressionImplementation
    {
        protected override LambdaExpression GetExpression(FinlandiaYearsOfBirthFilter filter, Type parameterType)
        {
            Expression<Func<FinlandiaHiihtoAPISearchResultRow, bool>> expr =
                r => filter.Items.Any(x => r.BornYear == x);
            return expr;
        }
    }
    
    public class FinlandiaTeamsFilterImpl : FinlandiaTeamsFilter.ExpandSearchArgsExpressionImplementation
    {
        protected override LambdaExpression GetExpression(FinlandiaTeamsFilter filter, Type parameterType)
        {
            Expression<Action<FilterSearchComposer<FinlandiaHiihtoAPISearchArgs>>> expr =
                c => c.EscalateBy(nameof(FinlandiaHiihtoAPISearchArgs.Team), filter.SearchStringsList);
            return expr;
        }
    }
    
    public class FinlandiaCompetitionClassesFilterImpl : FinlandiaCompetitionClassesFilter.ExpandSearchArgsExpressionImplementation
    {
        protected override LambdaExpression GetExpression(FinlandiaCompetitionClassesFilter filter, Type parameterType)
        {
            Expression<Action<FilterSearchComposer<FinlandiaHiihtoAPISearchArgs>>> expr =
                c => c.EscalateBy(nameof(FinlandiaHiihtoAPISearchArgs.CompetitionType), 
                    filter.Items.Select(CompetitionClassToApiEnum));
            return expr;
        }

        private static FinlandiaCompetitionType? CompetitionClassToApiEnum(FinlandiaHiihtoCompetitionClass competitionClass)
        {
            if (competitionClass.AdditionalDescription == ", juniorit alle 16v")
            {
                return FinlandiaCompetitionType.V20jun;
            }
            return (competitionClass.Style, competitionClass.Distance) switch
            {
                (FinlandiaSkiingStyle.Classic, FinlandiaSkiingDistance.Fifty) => FinlandiaCompetitionType.P50,
                (FinlandiaSkiingStyle.Skate, FinlandiaSkiingDistance.Fifty) => FinlandiaCompetitionType.V50,
                (FinlandiaSkiingStyle.Classic, FinlandiaSkiingDistance.Hundred) => FinlandiaCompetitionType.P100,
                (FinlandiaSkiingStyle.Classic, FinlandiaSkiingDistance.ThirtyTwo) => FinlandiaCompetitionType.P32, 
                (FinlandiaSkiingStyle.Classic, FinlandiaSkiingDistance.Twenty) => FinlandiaCompetitionType.P20, 
                (FinlandiaSkiingStyle.Skate, FinlandiaSkiingDistance.Twenty) => FinlandiaCompetitionType.V20, 
                (FinlandiaSkiingStyle.Skate, FinlandiaSkiingDistance.ThirtyTwo) => FinlandiaCompetitionType.V32,
                (FinlandiaSkiingStyle.Classic, FinlandiaSkiingDistance.FortyTwo) => FinlandiaCompetitionType.P42,
                (FinlandiaSkiingStyle.Skate, FinlandiaSkiingDistance.FortyTwo) => FinlandiaCompetitionType.V42,
                (FinlandiaSkiingStyle.Classic, FinlandiaSkiingDistance.Thirty) => FinlandiaCompetitionType.P30,
                (FinlandiaSkiingStyle.Classic, FinlandiaSkiingDistance.FortyFour) => FinlandiaCompetitionType.P44,
                (FinlandiaSkiingStyle.Classic, FinlandiaSkiingDistance.Sixty) => FinlandiaCompetitionType.P60,
                (FinlandiaSkiingStyle.Classic, FinlandiaSkiingDistance.SixtyTwo) => FinlandiaCompetitionType.P62,
                (FinlandiaSkiingStyle.Classic, FinlandiaSkiingDistance.TwentyFive) => FinlandiaCompetitionType.P25,
                (FinlandiaSkiingStyle.Classic, FinlandiaSkiingDistance.ThirtyFive) => FinlandiaCompetitionType.P35,
                (FinlandiaSkiingStyle.Classic, FinlandiaSkiingDistance.FortyFive) => FinlandiaCompetitionType.P45,
                (FinlandiaSkiingStyle.Classic, FinlandiaSkiingDistance.FiftyTwo) => FinlandiaCompetitionType.P52,
                (FinlandiaSkiingStyle.Classic, FinlandiaSkiingDistance.FiftyThree) => FinlandiaCompetitionType.P53,
                (FinlandiaSkiingStyle.Classic, FinlandiaSkiingDistance.SeventyFive) => FinlandiaCompetitionType.P75,
                (FinlandiaSkiingStyle.Skate, FinlandiaSkiingDistance.Thirty) => FinlandiaCompetitionType.V30,
                (FinlandiaSkiingStyle.Skate, FinlandiaSkiingDistance.FortyFive) => FinlandiaCompetitionType.V45,
                (FinlandiaSkiingStyle.Skate, FinlandiaSkiingDistance.FiftyThree) => FinlandiaCompetitionType.V53,
                _ => null
            };
        }
    }
}
