
using elr_API.Common;
using Newtonsoft.Json;

namespace elr_API.Service.Exception_handler
{
    public class CustomErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        public CustomErrorHandlingMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Intercept the response
            var originalBodyStream = context.Response.Body;

            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                try
                {
                    // Call the next middleware
                    await _next(context);

                    // Reset stream position to read from it
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    // If the response contains validation errors, reformat the body
                    if (context.Response.StatusCode == 400)
                    {
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
                        var errorResponse = TransformErrorResponse(responseBody);

                        // Write the modified response to the original stream
                        context.Response.Body = originalBodyStream;
                        await context.Response.WriteAsync(errorResponse);
                    }
                    else if (context.Response.StatusCode == 401)
                    {
                        var errorResponse = new
                        {
                            status = false,
                            errorCode = "Invalid Session Token",
                            data = ""
                        };

                        context.Response.Body = originalBodyStream;
                        await context.Response.WriteAsJsonAsync(errorResponse);
                    }
                    else if (context.Response.StatusCode == 404)
                    {
                        var errorResponse = new
                        {
                            status = false,
                            errorCode = "Resource not found",
                            data = ""
                        };

                        // Change the status code to 200
                        //context.Response.StatusCode = 200;
                        context.Response.Body = originalBodyStream;
                        await context.Response.WriteAsJsonAsync(errorResponse);
                    }
                    else
                    {
                        // Copy the original response to the response stream
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        await memoryStream.CopyToAsync(originalBodyStream);
                    }
                }
                catch (Exception ex)
                {
                    CommonFunctions.ILogging("", ex.ToString());

                    if (!context.Response.HasStarted)
                    {
                        context.Response.Clear();
                        context.Response.Body = originalBodyStream;
                        context.Response.StatusCode = 500;

                        await context.Response.WriteAsJsonAsync(new
                        {
                            status = false,
                            errorCode = ex.Message,
                            data = ""
                        });
                    }
                }
            }
        }

        private string TransformErrorResponse(string responseBody)
        {
            // Parse the original error response
            var originalErrorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseBody);

            // Construct the new error format
            var newErrorResponse = new
            {
                status = false,
                errorCode = originalErrorResponse.Errors?.FirstOrDefault().Value[0],
                data = ""
            };

            // Serialize the new error response
            return JsonConvert.SerializeObject(newErrorResponse);
        }

    }

    public class ErrorResponse
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}
