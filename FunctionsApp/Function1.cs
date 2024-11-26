using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FunctionsApp
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                // Чтение JSON-данных из тела запроса
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var requestData = JsonSerializer.Deserialize<RequestData>(requestBody);

                if (requestData == null || string.IsNullOrWhiteSpace(requestData.name))
                {
                    return new BadRequestObjectResult("Invalid input: name, a, and b are required.");
                }

                string name = requestData.name;
                int a = requestData.a;
                int b = requestData.b;

                int sum = a + b;

                string responseMessage = $"Aglaia Prokhorova: Hei {name}. Lukujen {a} ja {b} summa on {sum}.";
                return new OkObjectResult(responseMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing request: {ex.Message}");
                return new BadRequestObjectResult("An error occurred while processing the request.");
            }
        }
    }
    public class RequestData
    {
        public string name { get; set; }
        public int a { get; set; }
        public int b { get; set; }
    }
}

