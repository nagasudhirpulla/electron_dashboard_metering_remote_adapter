using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AdapterUtils;
using IdentityModel.Client;
using Newtonsoft.Json;
using Npgsql;

namespace ElectronDashboardMeteringDataRemoteAdapter
{
    public class DataFetcher
    {
        public ConfigurationManager Config_ { get; set; } = new ConfigurationManager();
        public async Task<string> GetIdentityServerToken()
        {
            // discover endpoints from metadata
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = Config_.IdentityServerHost,
                Policy =
                {
                    RequireHttps = false
                }
            });
            if (disco.IsError)
            {
                // Console.WriteLine(disco.Error);
                return null;
            }

            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = Config_.ClientId,
                ClientSecret = Config_.ClientPassword,

                Scope = "scada_archive"
            });

            if (tokenResponse.IsError)
            {
                // Console.WriteLine(tokenResponse.Error);
                return null;
            }
            // Console.WriteLine(tokenResponse.Json);
            return tokenResponse.AccessToken;
        }

        public async Task FetchAndFlushData(AdapterParams prms)
        {
            // get measurement id from command line arguments
            string measId = prms.MeasId;
            // get the start and end times
            DateTime startTime = prms.FromTime;
            DateTime endTime = prms.ToTime;

            List<double> measData;

            // get token
            string token = await GetIdentityServerToken();

            // call api            
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(token);

            string outStr = "";
            var response = await apiClient.GetAsync($"{Config_.DataHost}/api/fictdata/{measId}/{startTime.ToString("yyyy-MM-dd")}/{endTime.ToString("yyyy-MM-dd")}");
            if (!response.IsSuccessStatusCode)
            {
                //Console.WriteLine(response.StatusCode);
            }
            else
            {
                // we get the result in the form of ts1,val1,ts2,val2,...
                string content = await response.Content.ReadAsStringAsync();
                if (prms.IncludeQuality)
                {
                    measData = JsonConvert.DeserializeObject<List<double>>(content);
                    outStr = "";
                    // add good quality in the middle of each point
                    for (int pntIter = 0; pntIter < measData.Count; pntIter += 2)
                    {
                        outStr += $"{((pntIter > 0) ? "," : "")}{measData[pntIter]},{measData[pntIter + 1]}";
                    }
                }
                else
                {
                    if (content.Length > 4)
                    {
                        outStr = content.Remove(content.Length - 1, 1).Substring(1);
                    }
                }
            }
            ConsoleUtils.FlushChunks(outStr);
        }

        public async Task<List<FictMeasurement>> FetchFictMeasList()
        {
            List<FictMeasurement> measList = new List<FictMeasurement>();
            // get token
            string token = await GetIdentityServerToken();

            // call api            
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(token);

            var response = await apiClient.GetAsync($"{Config_.DataHost}/api/fictdata/getMeasurements");
            if (!response.IsSuccessStatusCode)
            {
                //Console.WriteLine(response.StatusCode);
            }
            else
            {
                string content = await response.Content.ReadAsStringAsync();
                measList = JsonConvert.DeserializeObject<List<FictMeasurement>>(content);
            }

            return measList;
        }
    }
}