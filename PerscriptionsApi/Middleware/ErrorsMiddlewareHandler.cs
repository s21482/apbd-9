namespace PerscriptionsApi.Middlewares
{
    public class ErrorsMiddlewareHandler
    {
        private readonly RequestDelegate _next;

        public ErrorsMiddlewareHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                using StreamWriter streamWriter = new StreamWriter("logs.txt", true);
                await streamWriter.WriteLineAsync($"{DateTime.Now}: {e}");
                await _next(httpContext);
            }
        }

        
    }
}
