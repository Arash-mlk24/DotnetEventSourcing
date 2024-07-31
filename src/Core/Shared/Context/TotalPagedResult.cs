namespace DotnetEventSourcing.src.Core.Shared.Context;

public class TotalPagedResult<T>(IQueryable<T> items) : PagedList<T>([.. items], 0, items.Count(), 0), ITotalPagedResult<T>
{
}