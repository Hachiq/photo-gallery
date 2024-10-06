namespace Core.Responses;

public record PagedResponse<T>(IEnumerable<T> List, int TotalRecords);
