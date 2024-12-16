using SchoolApp.Helpers;

namespace SchoolApp.Services;

public abstract class BaseService<T>(IConfiguration configuration)
    where T : class
{
    protected async Task<PaginatedListHelper<T>> GetPaginatedAsync(
        IQueryable<T> query,
        string sortOrder,
        Dictionary<string, Func<IQueryable<T>, IQueryable<T>>> sortOptions,
        int pageIndex,
        int pageSize)
    {
        query = SortingHelper.ApplySorting(query, sortOrder, sortOptions);
        return await PaginatedListHelper<T>.CreateAsync(query, pageIndex, pageSize);
    }

    public string GeneratePageSizeDropdownHtml(int pageSize)
    {
        return DropdownHtmlHelper.GenerateDropdownHtml(pageSize);
    }

    public int GetPageSize(int pageSize)
    {
        return pageSize == 0 ? int.Parse(configuration["PageSize"] ?? string.Empty) : pageSize;
    }
}