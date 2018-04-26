using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using System.IO;

namespace ConsoleApplication7
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("Enter movie title: ");
                var urlToPass = SearchEngines.UseGoogle(Console.ReadLine());
                Console.WriteLine("Choose which movie to parse");
                int userInput = Int32.Parse(Console.ReadLine());
                string url = urlToPass[userInput];   
                //send URL to the constructor and get the results calling the jsonResults property    
                var jsonResults = new IMBDScraper(url).jsonResults;
                Console.WriteLine(jsonResults);
                Console.WriteLine();
            } while (true);
        }
    }
}
