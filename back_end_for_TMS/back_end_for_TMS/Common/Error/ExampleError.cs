namespace back_end_for_TMS.Common.Error
{
  public static class ExampleError
  {

    // Static readonly errors
    // C# creates static errors only one time when they are loaded into memory
    // When services call them 1 million time, C# only returns 1 reference
    public static readonly AppError NormalError = new(
      StatusCodes.Status400BadRequest,
      11001,
      "Normal exception.");
  }
}
