using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao2
{
    public static class ClientHandler
    {
        public static async Task<int> getTotalScoredGoalsAsync(string team, int year, int teamNumber)
        {
            int goals = 0;
            using (HttpClient client = new HttpClient())
            {                                
                int page = 1;
                int totalPages = 0;
                do
                {
                    var requestUrl = Constants.resultsUrl + $"?year={year}&team{teamNumber}={team}";
                    if (page !=1)
                    {
                        requestUrl += $"&page={page}";
                    }                    
                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {                         
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var jsonResponse = JsonConvert.DeserializeObject<StatsResponse>(responseContent);


                        foreach (var item in jsonResponse.data)
                        {
                            if (teamNumber == 1)
                            {
                                int.TryParse(item.team1Goals, out var goalsQuantity);
                                goals += goalsQuantity;
                            }
                            else if (teamNumber == 2)
                            {
                                int.TryParse(item.team2Goals, out var goalsQuantity);
                                goals += goalsQuantity;
                            }

                        }                        
                        totalPages = jsonResponse.total_pages;
                        page ++;
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                        break;
                    }
                } while (page <= totalPages);                
            }            
            return goals;
        }
    }
}
