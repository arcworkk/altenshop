namespace Api.Common;

public record PaginatedResult<T>(IReadOnlyList<T> Items, int Total, int Page, int PageSize);