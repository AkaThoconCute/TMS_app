using back_end_for_TMS.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace back_end_for_TMS.Infrastructure.Normalizer;

public class ResultNormalizer : IAsyncResultFilter
{
  public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
  {
    bool isClassifyHttpStatus = false;

    // Khi Controller trả về thẳng AppResult<T>, ASP.NET Core tự chuyển context.Result thành ObjectResult
    if (isClassifyHttpStatus && context.Result is ObjectResult objectResult && objectResult.Value is IAppResult appResult)
    {
      // Set Status Code duy nhất ở đây
      objectResult.StatusCode = appResult.Success
          ? StatusCodes.Status200OK
          : (appResult.Error?.Status ?? StatusCodes.Status400BadRequest);
    }

    await next();
  }
}


