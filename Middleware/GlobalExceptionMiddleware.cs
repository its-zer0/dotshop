using System.Net;
using System.Text.Json;
using DotShop.Exceptions;

namespace DotShop.API.Middleware
{

  public static class GlobalExceptionMiddlewareExtensions
  {
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
      return app.UseMiddleware<GlobalExceptionMiddleware>();
    }
  }
  public class GlobalExceptionMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
      _next = next;
      _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        await HandlExceptionAsync(context, ex);
      }
    }

    private async Task HandlExceptionAsync(HttpContext context, Exception ex)
    {
      _logger.LogError(ex, " Unhandled exception");
      context.Response.ContentType = "application/problem+json";

      var (status, title, detail) = ex switch
      {
        ProductNotFoundException p => ((int)HttpStatusCode.NotFound, "Product not found", p.Message),
        OrderNotFoundException o => ((int)HttpStatusCode.NotFound, "Order not found", o.Message),
        BusinessException b => ((int)HttpStatusCode.BadRequest, "Business error", b.Message),
        DataAccessException d => ((int)HttpStatusCode.ServiceUnavailable, "Data access error", "Temporary data error. Try again later."),
        AppException a => ((int)HttpStatusCode.BadRequest, "Application error", a.Message),
        _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred", "Internal server error")
      };

      context.Response.StatusCode = status;

      var problem = new
      {
        type = $"https://httpstatuses.com/{status}",
        title,
        status,
        detail,
        instance = context.Request.Path
      };
      var json = JsonSerializer.Serialize(problem);
      await context.Response.WriteAsync(json);
    }

  }
}