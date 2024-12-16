namespace SchoolApp.Helpers;

public static class SortingHelper
{
    public static IQueryable<T> ApplySorting<T>(IQueryable<T> query, string sortOrder,
        Dictionary<string, Func<IQueryable<T>, IQueryable<T>>> sortOptions)
    {
        if (string.IsNullOrEmpty(sortOrder) || !sortOptions.TryGetValue(sortOrder, out var value))
            return sortOptions["default"](query);

        return value(query);
    }
}