namespace DotnetEventSourcing.src.Core.Shared.Context;

public class PagedList<T> : IPagedList<T>
{
    public const int InfinitePageNumber = -1;

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public int IndexFrom { get; set; }

    public IList<T> Items { get; set; }

    public bool HasPreviousPage => PageIndex - IndexFrom > 0;

    public bool HasNextPage => PageIndex - IndexFrom + 1 < TotalPages;

    internal PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int indexFrom)
    {
        if (indexFrom > pageIndex && pageIndex > InfinitePageNumber)
        {
            throw new ArgumentException($"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");
        }

        if (source is IQueryable<T> querable)
        {
            PageIndex = pageIndex > InfinitePageNumber ? pageIndex : 0;
            PageSize = pageSize;
            IndexFrom = indexFrom;
            TotalCount = querable.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            if (pageIndex == InfinitePageNumber)
            {
                Items = [.. querable];
            }
            else
            {
                Items = [.. querable.Skip((PageIndex - IndexFrom) * PageSize).Take(PageSize)];
            }
        }
        else
        {
            PageIndex = pageIndex > InfinitePageNumber ? pageIndex : 0;
            PageSize = pageSize;
            IndexFrom = indexFrom;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            if (pageIndex == InfinitePageNumber)
            {
                Items = source.ToList();
            }
            else
            {
                Items = source.Skip((PageIndex - IndexFrom) * PageSize).Take(PageSize).ToList();
            }
        }
    }

    public static IPagedList<T> GetPagedList(IEnumerable<T> source, double total, double pageIndex, double pageSize, double indexFrom)
    {
        var paged = new PagedList<T>();

        if (indexFrom > pageIndex && pageIndex > InfinitePageNumber)
        {
            throw new ArgumentException($"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");
        }

        paged.PageIndex = (int)(pageIndex > InfinitePageNumber ? pageIndex : 0);
        paged.PageSize = (int)pageSize;
        paged.IndexFrom = (int)indexFrom;
        paged.TotalCount = (int)total;
        paged.TotalPages = (int)Math.Ceiling(total / (double)pageSize);
        paged.Items = source.ToList();

        return paged;
    }

    public PagedList() => Items = [];
}

internal class PagedList<TSource, TResult> : IPagedList<TResult>
{
    public const int InfinitePageNumber = -1;

    public int PageIndex { get; }

    public int PageSize { get; }

    public int TotalCount { get; }

    public int TotalPages { get; }

    public int IndexFrom { get; }

    public IList<TResult> Items { get; }

    public bool HasPreviousPage => PageIndex - IndexFrom > 0;

    public bool HasNextPage => PageIndex - IndexFrom + 1 < TotalPages;

    public PagedList(IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter, int pageIndex, int pageSize, int indexFrom)
    {
        if (indexFrom > pageIndex && pageIndex > InfinitePageNumber)
        {
            throw new ArgumentException($"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");
        }

        if (source is IQueryable<TSource> querable)
        {
            PageIndex = pageIndex > InfinitePageNumber ? pageIndex : 0;
            PageSize = pageSize;
            IndexFrom = indexFrom;
            TotalCount = querable.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            var items = pageIndex == InfinitePageNumber ?
            [.. querable] : querable.Skip((PageIndex - IndexFrom) * PageSize).Take(PageSize).ToArray();

            Items = new List<TResult>(converter(items));
        }
        else
        {
            PageIndex = pageIndex > InfinitePageNumber ? pageIndex : 0;
            PageSize = pageSize;
            IndexFrom = indexFrom;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            var items = pageIndex == InfinitePageNumber ?
            source.ToArray() : source.Skip((PageIndex - IndexFrom) * PageSize).Take(PageSize).ToArray();

            Items = new List<TResult>(converter(items));
        }
    }

    public PagedList(IPagedList<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter)
    {
        PageIndex = source.PageIndex;
        PageSize = source.PageSize;
        IndexFrom = source.IndexFrom;
        TotalCount = source.TotalCount;
        TotalPages = source.TotalPages;

        Items = new List<TResult>(converter(source.Items));
    }
}

public static class PagedList
{
    public const int InfinitePageNumber = -1;

    public static IPagedList<T> Empty<T>() => new PagedList<T>();

    public static IPagedList<T> Create<T>(IEnumerable<T> items, int pI = 0, int pS = 15) => new PagedList<T>(items, pI, pS, pI);

    public static IPagedList<TResult> From<TResult, TSource>(
        IPagedList<TSource> source,
        Func<IEnumerable<TSource>, IEnumerable<TResult>> converter
    ) => new PagedList<TSource, TResult>(source, converter);
}