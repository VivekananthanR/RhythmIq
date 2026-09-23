using Microsoft.AspNetCore.Mvc;

namespace elr_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Mvc;
//using System.Text.Json.Nodes;

//namespace MyProject.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class WeatherForecastController : ControllerBase
//    {
//        [HttpGet("GeneratePdf")]
//        public async Task<IActionResult> GeneratePdf()
//        {
//            // 1. Construct the path to your HTML file
//            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "report.html");

//            if (!System.IO.File.Exists(filePath))
//            {
//                return NotFound($"The HTML template could not be found at: {filePath}");
//            }

//            // 2. Read the HTML file content asynchronously
//            string htmlContent = await System.IO.File.ReadAllTextAsync(filePath);

//            // ==========================================
//            // 3. NEW: READ YOUR JSON AND INJECT IT
//            // ==========================================
//            string ecgJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ecg.json");

//            // Safely read the ecg.json if it exists
//            JsonNode ecgNode = null;
//            if (System.IO.File.Exists(ecgJsonPath))
//            {
//                string ecgJsonContent = await System.IO.File.ReadAllTextAsync(ecgJsonPath);
//                ecgNode = JsonNode.Parse(ecgJsonContent);
//            }

//            // Create the dynamic data object you want to pass to the HTML
//            var reportData = new
//            {
//                Patient = new
//                {
//                    EnrollmentNo = "ENR-10029",
//                    PatientId = "PID-99381",
//                    MobileNo = "+1 555-0198",
//                    PatientName = "John Doe",
//                    Duration = "24",
//                    ReferringPhysician = "Dr. Sarah Smith",
//                    SupervisingPhysician = "Dr. Michael Jones",
//                    StartTime = "2023-10-24 08:00 AM",
//                    EndTime = "2023-10-25 08:00 AM"
//                },
//                Stats = new
//                {
//                    TotalEvents = 33
//                },
//                EcgData = ecgNode // Attach the ECG JSON data here
//            };

//            // Serialize the C# object to a JSON string and replace the placeholder in the HTML
//            string jsonString = System.Text.Json.JsonSerializer.Serialize(reportData);
//            htmlContent = htmlContent.Replace("/*{{REPORT_DATA}}*/ null", jsonString);
//            // ==========================================

//            // 4. Download Chromium
//            var browserFetcher = new BrowserFetcher();
//            await browserFetcher.DownloadAsync();

//            // 5. Launch headless browser
//            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
//            {
//                Headless = true
//            });

//            await using var page = await browser.NewPageAsync();

//            // ==========================================
//            // 6. CRITICAL FIX: Add WaitUntilNavigation
//            // ==========================================
//            // Without 'Networkidle0', Puppeteer will take the PDF picture BEFORE 
//            // your JavaScript finishes drawing the canvas charts.
//            await page.SetContentAsync(htmlContent, new NavigationOptions
//            {
//                WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }
//            });

//            // 7. PDF Export Options
//            var pdfOptions = new PdfOptions
//            {
//                Format = PaperFormat.A4,
//                PrintBackground = true,
//                MarginOptions = new MarginOptions
//                {
//                    Top = "8mm",
//                    Bottom = "8mm",
//                    Left = "8mm",
//                    Right = "8mm"
//                }
//            };

//            // 8. Generate PDF directly into memory
//            byte[] pdfBytes = await page.PdfDataAsync(pdfOptions);

//            // 9. Return the PDF file to the browser
//            return File(pdfBytes, "application/pdf");
//        }
//        [HttpGet("GeneratePdfFinal")]
//        public async Task<IActionResult> GeneratePdfFinal()
//        {
//            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "finalReport.html");

//            if (!System.IO.File.Exists(filePath))
//            {
//                return NotFound($"The HTML template could not be found at: {filePath}");
//            }

//            string htmlContent = await System.IO.File.ReadAllTextAsync(filePath);

//            var browserFetcher = new BrowserFetcher();
//            await browserFetcher.DownloadAsync();

//            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
//            {
//                Headless = true
//            });

//            await using var page = await browser.NewPageAsync();

//            // CRITICAL FIX: Wait for all JavaScript fetches and rendering to finish
//            await page.SetContentAsync(htmlContent, new NavigationOptions
//            {
//                WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }
//            });

//            var pdfOptions = new PdfOptions
//            {
//                Format = PaperFormat.A4,
//                PrintBackground = true,
//                MarginOptions = new MarginOptions
//                {
//                    Top = "8mm",
//                    Bottom = "8mm",
//                    Left = "8mm",
//                    Right = "8mm"
//                }
//            };

//            byte[] pdfBytes = await page.PdfDataAsync(pdfOptions);

//            return File(pdfBytes, "application/pdf");
//        }

//        [HttpGet("GeneratePdfUrgent")]
//        public async Task<IActionResult> GeneratePdfUrgent()
//        {
//            // 1. Construct the path to your HTML file
//            // This assumes you placed 'report.html' inside a 'Templates' folder in your project root.
//            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "urgent.html");

//            // 2. Check if the file exists to prevent crashing
//            if (!System.IO.File.Exists(filePath))
//            {
//                return NotFound($"The HTML template could not be found at: {filePath}");
//            }

//            // 3. Read the HTML file content asynchronously
//            string htmlContent = await System.IO.File.ReadAllTextAsync(filePath);

//            // 4. Download Chromium
//            var browserFetcher = new BrowserFetcher();
//            await browserFetcher.DownloadAsync();

//            // 5. Launch headless browser
//            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
//            {
//                Headless = true
//            });

//            await using var page = await browser.NewPageAsync();

//            // 6. Set HTML Content (now coming from your file)
//            await page.SetContentAsync(htmlContent);

//            // 7. PDF Export Options
//            var pdfOptions = new PdfOptions
//            {
//                Format = PaperFormat.A4,
//                PrintBackground = true,
//                MarginOptions = new MarginOptions
//                {
//                    Top = "8mm",
//                    Bottom = "8mm",
//                    Left = "8mm",
//                    Right = "8mm"
//                }
//            };

//            // 8. Generate PDF directly into memory
//            byte[] pdfBytes = await page.PdfDataAsync(pdfOptions);

//            // 9. Return the PDF file to the browser
//            return File(pdfBytes, "application/pdf");
//        }
//        [HttpGet("GetEcgData")]
//        public async Task<IActionResult> GetEcgData()
//        {
//            // 1. JSON file path
//            string filePath = Path.Combine(
//                Directory.GetCurrentDirectory(),
//                "wwwroot",
//                "ecg.json"
//            );

//            // 2. Check file exists
//            if (!System.IO.File.Exists(filePath))
//            {
//                return NotFound($"The JSON file could not be found at: {filePath}");
//            }

//            // 3. Read JSON
//            string jsonContent = await System.IO.File.ReadAllTextAsync(filePath);

//            // 4. Parse JSON
//            var jsonNode = JsonNode.Parse(jsonContent);
//            var waveform = jsonNode?["waveform_snippet"];

//            if (waveform == null)
//            {
//                return NotFound("waveform_snippet not found.");
//            }

//            // 5. Get timestamp_ms and voltage_mv arrays
//            var timestampsArray = waveform["timestamp_ms"] as JsonArray;
//            var voltageArray = waveform["voltage_mv"] as JsonArray;

//            if (timestampsArray == null || timestampsArray.Count == 0)
//            {
//                return BadRequest("timestamp_ms array is missing or empty.");
//            }

//            // 6. Get first timestamp
//            long startMs = timestampsArray[0]!.GetValue<long>();

//            // 10 seconds = 10,000 milliseconds
//            long thresholdMs = startMs + 10000;

//            // 7. Find number of points within first 10 seconds
//            int keepCount = 0;

//            foreach (var node in timestampsArray)
//            {
//                if (node != null && node.GetValue<long>() <= thresholdMs)
//                {
//                    keepCount++;
//                }
//                else
//                {
//                    break;
//                }
//            }

//            // 8. Create new arrays
//            var newTimestamps = new JsonArray();
//            var newVoltage = new JsonArray();

//            // 9. Copy timestamp_ms AND voltage_mv
//            for (int i = 0; i < keepCount; i++)
//            {
//                // timestamp_ms
//                newTimestamps.Add(
//                    timestampsArray[i]!.GetValue<long>()
//                );

//                // voltage_mv
//                if (voltageArray != null && i < voltageArray.Count)
//                {
//                    newVoltage.Add(
//                        voltageArray[i]!.GetValue<double>()
//                    );
//                }
//            }

//            // 10. Replace original arrays
//            waveform["timestamp_ms"] = newTimestamps;
//            waveform["voltage_mv"] = newVoltage;

//            // 11. Return modified JSON
//            return Content(
//                jsonNode?.ToJsonString() ?? "{}",
//                "application/json"
//            );
//        }
//    }
//}