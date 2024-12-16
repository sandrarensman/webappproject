using Microsoft.EntityFrameworkCore;

namespace SchoolApp.Helpers;

public class PaginatedListHelper<T>
{
    public PaginatedListHelper(List<T> items, int count, int pageIndex, int pageSize)
    {
        if (pageIndex < 1)
            throw new ArgumentException("Page index must be greater than zero.");
        if (pageSize <= 0)
            throw new ArgumentException("Page size cannot be less than or equal to zero.");
        if (items == null)
            throw new ArgumentNullException(nameof(items), "Items cannot be null.");

        PageIndex = pageIndex;
        TotalCount = count;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        Items = items ?? new List<T>();
    }

    public List<T> Items { get; private set; }

    public int PageIndex { get; }
    public int TotalPages { get; }
    public int TotalCount { get; private set; }
    public int PageSize { get; private set; }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public static async Task<PaginatedListHelper<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source), "Source cannot be null.");

        if (pageIndex < 1)
            throw new ArgumentException("Page index must be greater than zero.");
        if (pageSize <= 0)
            throw new ArgumentException("Page size cannot be less than or equal to zero.");

        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedListHelper<T>(items, count, pageIndex, pageSize);
    }
}