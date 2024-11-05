using System.Net;

namespace NetAuthAssignment.Helpers
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred: " + ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }
    }
}
