using ConsoleTables;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CodeChalange
{
    class Program
    {
        public static void GetNutriData()
        {

            Console.WriteLine("Please enter input");
            string input = Console.ReadLine();
            // user must enter something    
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Inavalid input");
                throw new Exception();
            }


            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add("x-app-id", Constants.APP_ID);
                client.DefaultRequestHeaders.Add("x-app-key", Constants.APP_KEY);
                client.DefaultRequestHeaders.Add("x-remote-user-id", Constants.User_ID);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                NutriInfoQuery query = new NutriInfoQuery { query = input };
                // Making call to API
                var response = client.PostAsJsonAsync(Constants.URL, query).Result;

                // If succeded
                if (response.IsSuccessStatusCode)
                {

                    var readTask = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var nutriResponse = readTask.GetAwaiter().GetResult();

                    // DeserializeObject to custom object
                    Results Nutrijson = JsonConvert.DeserializeObject<Results>(nutriResponse);

                    var table = new ConsoleTable(Constants.FOODNAME, Constants.QUANTITY, Constants.MEASURE, Constants.CALORIES,
                        Constants.PROTEIN, Constants.VITAMINA);
                    for (int i = 0; i < Nutrijson.Foods.Count; i++)
                    {
                        table.AddRow(Nutrijson.Foods[i].Food_Name, Nutrijson.Foods[i].Serving_Qty, Nutrijson.Foods[i].Serving_Unit,
                           Nutrijson.Foods[i].Nf_Calories, Nutrijson.Foods[i].Nf_Protein, Nutrijson.Foods[i].Nf_P);

                    }
                    table.Write();
                    Console.WriteLine();

                }
                else
                {
                    Console.WriteLine("Error occured");
                    Console.ReadLine(); // stop execution here
                }

            }
        }
        static void Main(string[] args)
        {
            // Endless loop for contineous execution
            while (1 == 1)
            {
                GetNutriData();
            }
        }
    }
}
