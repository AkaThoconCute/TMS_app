namespace back_end_for_TMS.Common;

public class PaginatedResult<T>
{
  public List<T> Data { get; set; } = [];
  public int TotalCount { get; set; }
  public int PageSize { get; set; }
  public int PageNumber { get; set; }
  public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
}
