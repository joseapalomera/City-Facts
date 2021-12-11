using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Program2
{

    class Program
    {
        
        //FOR THE PURPOSES OF GITHUB, THE API KEYS HAVE BEEN REMOVED. IF DESIRED, PLEASE USE YOUR OWN
        //PUBLIC KEYS AND REPLACE THEM INTO THE GLOBAL VARIABLES.

        //Open Weather API details
        private static string openWeatherSearch;
        private static string key = "INSERT OPENWEATHER KEY";

        //Yelp API details
        private static string yelpSearch;
        private static string yelpkey = "INSERT YELP KEY";


        //Http Response
        private static HttpResponseMessage response = new HttpResponseMessage();
        private static string result;
        private static bool success = true;
        private static bool yelp = false;

        //Retry Logic
        private static readonly int retry = 3;

        //Root Objects
        private static WeatherAPI.Rootobject myWeather;
        private static YelpAPI.Rootobject myBus;


        static void Main(string[] args)
        {
            string city;
            
            if(args.Length == 0)
            {
                throw new ArgumentException("Nothing was entered into the console");
            }

            //Check and see if there is a string in the console
            if(args.Length == 1)
            {
                city = args[0];
            }
            //Runs this if the city has a space in between, eg. "Las Vegas"
            else
            {
                city = args[0];
                for(int i = 1; i < args.Length;i++)
                {
                    city += " " + args[i];
                }
            }

            Console.WriteLine("------------------------------------------------");
            printWeather(city);
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine();

            Console.WriteLine("------------------------------------------------");
            yelp = true;
            printInterestingFact(city);
            Console.WriteLine("------------------------------------------------");

            Console.ReadKey();
        }

        private static void weatherWeb(string city)
        {
            openWeatherSearch = "http://api.openweathermap.org/data/2.5/weather?q=" + city + "&units=metric&mode=json&appid=" + key;

            return;

        }
        private static void printWeather(string city)
        {
            Console.WriteLine("Getting Weather from " + city.ToUpper() + ".");
            //Init the API call
            weatherWeb(city);
            Console.WriteLine("-Connecting...-\n");

            getData(openWeatherSearch, retry).Wait();

            String currentLoc;

            if(success)
            {
                myWeather = JsonConvert.DeserializeObject<WeatherAPI.Rootobject>(result);
                currentLoc = myWeather.Sys.Country;

                Console.WriteLine("Current weather for " + myWeather.Name + ", " + myWeather.Sys.Country + " shown below.");
                Console.WriteLine("Weather forecast today for " + myWeather.Name + " is " + myWeather.Weather[0].Description + ".");
                Console.WriteLine("Current temperature " + myWeather.Name + " is " + myWeather.Main.Temp + " °C.");
                Console.WriteLine("Max temperature of " + myWeather.Name + " is " + myWeather.Main.TempMax + " °C.");
                Console.WriteLine("Minimum temperature of " + myWeather.Name + " is " + myWeather.Main.TempMin + " °C.");
                Console.WriteLine("Currently, temperature of " + myWeather.Name + " feels like " + myWeather.Main.FeelsLike + " °C");
                Console.WriteLine("Currently, humidity of " + myWeather.Name + " is " + myWeather.Main.Humidity + "%.");
                Console.WriteLine("Wind Speed of " + myWeather.Name + " is " + +myWeather.Wind.Speed + "m/s");
            }
            else
            {
                currentLoc = null;
                Console.WriteLine("Something went wrong with the Weather information about " + city.ToUpper());
            }

        }

        private static void factWeb(string city)
        {
            //combine api with link/city
            yelpSearch = "https://api.yelp.com/v3/businesses/search?location=" + city + "&sort_by=rating&limit=1";

            return;
        }

        private static void printInterestingFact(string city)
        {
            Console.WriteLine("Getting Businesses from " + city.ToUpper() + ".");
            //Init the API call
            factWeb(city);
            Console.WriteLine("-Connecting...-\n");

            getData(yelpSearch, retry).Wait();

            String currentLoc;

            if(success)
            {
                myBus = JsonConvert.DeserializeObject<YelpAPI.Rootobject>(result);
                currentLoc = myBus.Region.ToString();

                Console.WriteLine("Best business nearby " + city.ToUpper() + " is:\n");
                Console.WriteLine("Name: " + myBus.Businesses[0].Name);

                Console.WriteLine("Address: " + myBus.Businesses[0].Location.Address1 + " ");
                Console.Write(myBus.Businesses[0].Location.Address2 + " ");
                Console.Write(myBus.Businesses[0].Location.Address3 + " ");
                Console.Write(myBus.Businesses[0].Location.City);
                Console.Write(", " + myBus.Businesses[0].Location.State + " ");
                Console.Write(myBus.Businesses[0].Location.ZipCode);
                Console.WriteLine();

                Console.WriteLine("Rating: " + myBus.Businesses[0].Rating.ToString());
                Console.WriteLine("Phone Number: " + myBus.Businesses[0].Phone.ToString());
                Console.WriteLine("URL: " + myBus.Businesses[0].Url.ToString());

                
            }
            else
            {
                currentLoc = null;
                Console.WriteLine("Something went wrong with finding a business near " + city.ToUpper());
            }

        }

        private static bool connectToWebsite(string url)
        {
            HttpClient client = new HttpClient();

            if (yelp == true)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", yelpkey);
            }


            response = client.GetAsync(url).Result;
            success = response.IsSuccessStatusCode;

            return success;
        }

        private static async Task getData(string webApi, int retryCount)
        {
            //Method that will help return the Json string
            //will also try to retry any retrieving any data and just exit if it doesn't happen

            int cur_retry = 0;
            int delayTime = 100;
            for(;;)
            {
                if(!connectToWebsite(webApi))
                {
                    cur_retry++;
                    if(retryCount > cur_retry)
                    {
                        return;
                    }

                    int delay = delayTime;
                    delayTime += (delay ^ 2) - 2;

                    Console.WriteLine("Retrying in " + delayTime);
                    await Task.Delay(delayTime);

                }
                else
                {
                    result = response.Content.ReadAsStringAsync().Result;
                    return;
                }
            }

           
        }


    }
}
