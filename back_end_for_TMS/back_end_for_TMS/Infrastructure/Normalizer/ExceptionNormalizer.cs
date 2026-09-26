using back_end_for_TMS.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace back_end_for_TMS.Infrastructure.Normalizer;

public class ExceptionNormalizer(ILogger<ExceptionNormalizer> logger) : IExceptionHandler
{
  public async ValueTask<bool> TryHandleAsync(
      HttpContext httpContext,
      Exception exception,
      CancellationToken cancellationToken)
  {
    logger.LogError(exception, ">>> HIT THE GLOBAL EXCEPTION NORMALIZER <<<");

    var error = new AppError(StatusCodes.Status500InternalServerError, 1, exception.Message);

    var problemDetails = new ProblemDetails();
    problemDetails.Extensions.Add("success", false);
    problemDetails.Extensions.Add("result", null);
    problemDetails.Extensions.Add("error", error);

    httpContext.Response.StatusCode = error.Status;
    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

    return true;
  }
}