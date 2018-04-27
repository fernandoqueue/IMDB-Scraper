# IMDB-Scraper
IMDB Scraper class scrapes information from the site and creates a json string.

How to use:

Install nuget packages

Newtonsoft json

HtmlAgilityPack

information being scraped
```
        Title
        IMDB ID
        Year
        RunningTime
        IMDBRatings
        ContentRating
        Director
        [Writers]
        [Stars] 
        [Genres] 
        [Cast]
        [Countries]
        [BoxOffice]
             -Budget
             -Opening Weekend
             -Gross USA
             -Cumulative Worldwide Gross
        
        Summary 
        StoryLine 
```
1) pass URL into the constructor and call jsonResults method to recieve JsonString
```
namespace ConsoleApplication
{
    class Program    
    {
        static void Main(string[] args)        
        {    
                var jsonResults = new IMDBScraper("https://www.imdb.com/title/tt0175142").jsonResults();                
                Console.WriteLine(jsonResults);                
                Console.WriteLine();                           
        }        
    }    
}
```
Output:
```
{"Title":"The Departed","Year":"2006","RunningTime":"2h 31min","IMDBRatings":"8.5","ContentRating":"R","Director":"Martin Scorsese","Writers":["William Monahan","Alan Mak"],"Stars":["Leonardo DiCaprio","Matt Damon","Jack Nicholson"],"Genres":["Crime","Drama","Thriller"],"Cast":["Leonardo DiCaprio","Matt Damon","Jack Nicholson","Mark Wahlberg","Martin Sheen","Ray Winstone","Vera Farmiga","Anthony Anderson","Alec Baldwin","Kevin Corrigan","James Badge Dale","David O'Hara","Mark Rolston","Robert Wahlberg","Kristen Dalton"],"Countries":["USA","Hong Kong"],"BoxOffice":{"Budget":"$90,000,000","Opening Weekend USA":"$26,887,467,","Gross USA":"$132,384,315,","Cumulative Worldwide Gross":"$289,847,354"},"Summary":"In South Boston, the state police force is waging war on Irish-American organized crime. Young undercover cop Billy Costigan is assigned to infiltrate the mob syndicate run by gangland chief Frank Costello. While Billy quickly gains Costello's confidence, Colin Sullivan, a hardened young criminal who has infiltrated the state police as an informer for the syndicate is rising to a position of power in the Special Investigation Unit. Each man becomes deeply consumed by their double lives, gathering information about the plans and counter-plans of the operations they have penetrated. But when it becomes clear to both the mob and the police that there is a mole in their midst, Billy and Colin are suddenly in danger of being caught and exposed to the enemy - and each must race to uncover the identity of the other man in time to save themselves. But is either willing to turn on their friends and comrades they've made during their long stints undercover?","StoryLine":"An undercover cop and a mole in the police attempt to identify each other while infiltrating an Irish gang in South Boston."}
```
NOTE:

This scraper only supports movie information. TV shows have a different layout so I will need to implement tv show support for the future.

TODO: 

1)Add the rest of the movie information(ratings,credits,synopsis, etc..)

2)Add TV show support.
