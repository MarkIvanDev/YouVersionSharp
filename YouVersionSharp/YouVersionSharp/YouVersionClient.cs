// MIT License

// Copyright(c) 2020 Mark Ivan Basto

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.


using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace YouVersionSharp
{
    public class YouVersionClient : IDisposable
    {
        private readonly HttpClient httpClient;

        public YouVersionClient(string apiKey)
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("x-youversion-developer-token", apiKey);
            httpClient.BaseAddress = new Uri("https://developers.youversionapi.com/1.0/");
        }

        public async Task<VersionCollection> GetVersions(int? page = null, int? pageSize = null)
        {
            try
            {
                var requestBuilder = new StringBuilder();
                requestBuilder.Append("versions?");
                if (page.HasValue) requestBuilder.Append($"&page={page}");
                if (pageSize.HasValue) requestBuilder.Append($"&page_size={pageSize}");

                var response = await httpClient.GetAsync(requestBuilder.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var versions = JsonConvert.DeserializeObject<VersionCollection>(json);
                    return versions;
                }
                return default;
            }
            catch (Exception)
            {
                return default;
            }
        }

        public async Task<VerseOfTheDay> GetVerseOfTheDay(int dayOfYear, int versionId, string language = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(language)) httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(language));

                var requestUri = $"verse_of_the_day/{dayOfYear}?version_id={versionId}";
                var response = await httpClient.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var votd = JsonConvert.DeserializeObject<VerseOfTheDay>(json);
                    return votd;
                }
                return default;
            }
            catch (Exception)
            {
                return default;
            }
        }

        public async Task<VerseOfTheDayCollection> GetVerseOfTheDayCollection(int versionId, int? page = null, int? pageSize = null, string language = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(language)) httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(language));

                var requestBuilder = new StringBuilder();
                requestBuilder.Append("verse_of_the_day?");
                requestBuilder.Append($"version_id={versionId}");
                if (page.HasValue) requestBuilder.Append($"&page={page}");
                if (pageSize.HasValue) requestBuilder.Append($"&page_size={pageSize}");

                var response = await httpClient.GetAsync(requestBuilder.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var votd = JsonConvert.DeserializeObject<VerseOfTheDayCollection>(json);
                    return votd;
                }
                return default;
            }
            catch (Exception)
            {
                return default;
            }
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
