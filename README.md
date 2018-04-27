# IMDB-Scraper
IMDB Scraper class scrapes information from the site and creates a json string.

How to use:

Install nuget packages

Newtonsoft json

HtmlAgilityPack

1) pass URL into the constructor and call jsonResults propertie to recieve JsonString
```
namespace ConsoleApplication
{
    class Program    
    {
        static void Main(string[] args)        
        {    
                var jsonResults = new IMDBScraper("https://www.imdb.com/title/tt0175142").jsonResults;                
                Console.WriteLine(jsonResults);                
                Console.WriteLine();                           
        }        
    }    
}
```
NOTE:

This scraper only supports movie information. TV shows have a different layout so I will need to implement tv show support for the future.

TODO: 

1)Add the rest of the movie information(ratins,credits,synopsis, etc..)

2)Add TV show support.
