using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FinlandiaHiihtoAPI;
using FJ.DomainObjects.Filters;
using FJ.DomainObjects.Filters.CommonFilters;
using FJ.DomainObjects.FinlandiaHiihto;
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

    public class FinlandiaFullNameFilterImp : FinlandiaFullNameFilter.ReduceSearchResultsExpressionImplementation
    {
        protected override LambdaExpression GetExpression(FinlandiaFullNameFilter filter, Type parameterType)
        {
            Expression<Func<FinlandiaHiihtoAPISearchResultRow, bool>> expr =
                r => filter.Value == r.FullName;
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
}
