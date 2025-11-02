using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace eHealthFolderDev.Services
{
    public class AzureOcrService
    {
        private readonly string _endpoint;
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public AzureOcrService(string endpoint, string apiKey)
        {
            _endpoint = endpoint.TrimEnd('/');
            _apiKey = apiKey;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _apiKey);
        }

        public async Task<string> ExtractTextFromImageAsync(Stream imageStream)
        {
            try
            {
                // Convert stream to byte array
                using var memoryStream = new MemoryStream();
                await imageStream.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();

                // Call Azure Computer Vision Read API
                var analyzeUrl = $"{_endpoint}/vision/v3.2/read/analyze";
                
                using var content = new ByteArrayContent(imageBytes);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                var response = await _httpClient.PostAsync(analyzeUrl, content);
                
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"OCR API Error: {response.StatusCode} - {error}");
                }

                // Get the operation location from response headers
                var operationLocation = response.Headers.GetValues("Operation-Location").FirstOrDefault();
                
                if (string.IsNullOrEmpty(operationLocation))
                {
                    throw new Exception("No operation location returned from OCR service");
                }

                // Poll for results
                return await PollForResultsAsync(operationLocation);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"OCR Error: {ex.Message}");
                throw;
            }
        }

        private async Task<string> PollForResultsAsync(string operationLocation)
        {
            var maxAttempts = 10;
            var delay = TimeSpan.FromSeconds(1);

            for (int i = 0; i < maxAttempts; i++)
            {
                await Task.Delay(delay);

                var response = await _httpClient.GetAsync(operationLocation);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                using var document = JsonDocument.Parse(jsonResponse);
                var root = document.RootElement;

                if (root.TryGetProperty("status", out var status))
                {
                    var statusValue = status.GetString();

                    if (statusValue == "succeeded")
                    {
                        return ExtractTextFromResult(root);
                    }
                    else if (statusValue == "failed")
                    {
                        throw new Exception("OCR processing failed");
                    }
                    // If running, continue polling
                }
            }

            throw new Exception("OCR processing timeout");
        }

        private string ExtractTextFromResult(JsonElement root)
        {
            var textBuilder = new StringBuilder();

            if (root.TryGetProperty("analyzeResult", out var analyzeResult))
            {
                if (analyzeResult.TryGetProperty("readResults", out var readResults))
                {
                    foreach (var page in readResults.EnumerateArray())
                    {
                        if (page.TryGetProperty("lines", out var lines))
                        {
                            foreach (var line in lines.EnumerateArray())
                            {
                                if (line.TryGetProperty("text", out var text))
                                {
                                    textBuilder.AppendLine(text.GetString());
                                }
                            }
                        }
                    }
                }
            }

            return textBuilder.ToString().Trim();
        }
    }
}
