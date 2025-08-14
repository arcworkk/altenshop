namespace Api.Common;

public record PaginedResult<T>(IReadOnlyList<T> Items, int Total, int Page, int PageSize);