using System;
using System.IO;
using CommandLine;
using System.Net.Http;
using System.Text.Json;
using System.Reflection;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;

[assembly: AssemblyVersionAttribute("0.0.5.5")]

namespace imdbLookup
{
    class Program
    {

        static async Task Main(string[] args)
        {


            Assembly thisAssem = typeof(Program).Assembly;
            AssemblyName program = thisAssem.GetName();
            System.Version ver = program.Version;



            string padding = "===========================";
            string apiKey = "87eca00a";
            string baseUrl = "http://www.omdbapi.com/";
            string imdbTitlePrefix = "t=";
            string imdbIdPrefix = "i=";
            string imdbID = "tt0460627";

            string imdbName = string.Empty;//args[0];
            string version = $" {program.Name} {ver}";
            string author = "@Tr4shL0rd";
            string github = "https:/github.com/tr4shl0rd";
            //bools
            //Console.WriteLine(args[1]);
            bool title = true;
            bool verbose = false;
            //bool versionBool = false;

            //ints
            int time = 1;
            //Arrays
            string[] commands = new string[5];
            commands[0] = "-h (help)";
            commands[1] = "-v (version)";
            commands[2] = "-V (Verbose)";
            commands[3] = "-c (commands)";
            commands[4] = "-s [--show] (name of show/movie)";
            //multiLine Strings
            //     string usage = @"usage: ./IMDBLookup Series/Movie name
            //./IMDBLookup help    (shows this message)
            //./IMDBLookup version (displays the program version)";

            string banner = @"
     ______  _______  ____     __                __             
    /  _/  |/  / __ \/ __ )   / /   ____  ____  / /____  ______ 
    / // /|_/ / / / / __  |  / /   / __ \/ __ \/ //_/ / / / __ \
   / // /  / / /_/ / /_/ /  / /___/ /_/ / /_/ / ,< / /_/ / /_/ /
 /___/_/  /_/_____/_____/  /_____/\____/\____/_/|_|\__,_/ .___/ 
                                                       /_/      
 " + "Written by " + author + "\n " +
  github + "\n" + "\n" + version + "\n\n" + padding;
            
            Parser.Default.ParseArguments<Options>(args)
                        .WithParsed(Run =>
                        {
                            imdbName = Run.show;
                            if (Run.Verbose)
                            {
                                verbose = true;
                            }
                            if (Run.version)
                            {
                                Console.WriteLine("VERISON HERE!");//docs autoVersion
                                return;
                            }
                            if (Run.commands)
                            {
                                foreach (string comms in commands)
                                {
                                    Console.WriteLine(comms);
                                }
                            }

                        });
            //TODO
            //GET ALL SERIES / MOVIES FROM THE EXTERN HARDDRIVE AND PUT THEM IN AN ARRAY!


            if (apiKey == "Your API KEY Here")
            {
                Console.WriteLine("Enter ApiKey At Program.cs:32\nGet Your Api Key Here: http://www.omdbapi.com/apikey.aspx?");
            }

            try
            {
                imdbName = imdbName.Replace(" ", "+");
            }
            catch (System.NullReferenceException)
            {
                return;
            }

            await imdbSite(title, imdbName, apiKey, time, banner);

            string titleApiUrl = baseUrl + "?apikey=" + apiKey + "&" + imdbTitlePrefix + imdbName;
            string idApiUrl = baseUrl + "?apikey=" + apiKey + "&" + imdbIdPrefix + imdbID;


            if (verbose == true)
            {

                if (title == true)
                {
                    Console.WriteLine(titleApiUrl);
                }
                else
                {
                    Console.WriteLine(idApiUrl);
                }
                Console.WriteLine($"ApiKey: {apiKey}");
                Console.WriteLine($"Version:{version}");
            }

        }

        

        private static readonly HttpClient client = new HttpClient();
        private static async Task imdbSite(bool title, string name, string apiKey, int time, string banner)
        {
            Console.WriteLine(banner);

            client.Timeout = TimeSpan.FromSeconds(time);
            if (name.StartsWith("tt"))
            {
                title = false;
            }
            string idTitle = ""; // Creates an empty variable of string type;
            try
            {
                if (title == true)
                {
                    idTitle = await client.GetStringAsync($"http://www.omdbapi.com/?apikey={apiKey}&t={name}"); // Gets
                }                                                                                               //
                else                                                                                            //  
                {                                                                                               // The
                    idTitle = await client.GetStringAsync($"http://www.omdbapi.com/?apikey={apiKey}&i={name}"); // Json
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.Message.StartsWith("Name or service not known"))
                {
                    Console.WriteLine("Request Timed Out!\n");
                    return;
                }
            }

            var error = JsonSerializer.Deserialize<ResError>(idTitle); // Deserialize Json
            if (!string.IsNullOrEmpty(error.Error))
            {
                Console.WriteLine(error.Error);
                Console.WriteLine("hej");//der er en fejl
            }
            else
            {
                var serie = JsonSerializer.Deserialize<Serie>(idTitle); // Deserialize Json

                foreach (PropertyInfo prop in serie.GetType().GetProperties())// Gets Each value from the json returned
                {
                    var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType; // Handles Nullables
                    var value = prop.GetValue(serie, null);
                    var str = String.Format("{0, -11} {1, -10}", prop.Name, value ?? "N/A");
                    Console.WriteLine(str);
                }
                Console.WriteLine("===========================");
                Console.WriteLine($"IMDB Link: https://www.imdb.com/title/{serie.imdbID}/");
            }
        }


    }
}
