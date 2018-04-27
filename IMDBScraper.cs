using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApplication
{
    class IMDBScraper
    {

        string _url;
        public string Title { get; private set; } = "";
        public string IMDBID { get; private set; }
        public string Year { get; private set; } = "";
        public string RunningTime { get; private set;}
        public string IMDBRatings { get; private set;}
        public string ContentRating { get; private set; }
        public string Director { get; private set; } = "";
        public List<string> Writers { get; private set; } 
        public List<string> Stars { get; private set; }
        public List<string> Genres { get; private set; }
        public List<string> Cast { get; private set; }
        public List<string> Countries { get; private set; }
        public Dictionary<string,string> BoxOffice { get; private set; }
        public string Summary { get; private set; } = "";
        public string StoryLine { get; private set; } = "";

        public string jsonResults() => JsonConvert.SerializeObject(this);  

        public IMDBScraper(string url)
        {
            if (url != "")
            {
                Cast = new List<string>();
                Genres = new List<string>();
                Writers = new List<string>();
                Stars = new List<string>();
                Countries = new List<string>();
                BoxOffice = new Dictionary<string, string>();
                _url = url;
                var doc = Gethtml(url);
                if (doc.Text.Contains(@"content=""video.movie"""))
                    ParseHtml(doc);
            }
           
        }

        void ParseHtml(HtmlDocument doc)
        {
            //IMDB Id
            var reg = new Regex("/(tt[0-9]{7})/");
            var match = reg.Match(_url);
            IMDBID = match.Value.Replace("/", "");
            //Title
            Title = WebUtility.HtmlDecode(doc.DocumentNode.SelectNodes("//h1[@itemprop='name']")[0].ChildNodes[0].InnerText);
            Title = Title.TrimEnd();
            //content rating
            var tmpContentRating = doc.DocumentNode.SelectNodes("//meta[@itemprop='contentRating']");
            if (tmpContentRating != null)
                ContentRating = tmpContentRating[0].Attributes[1].Value;
            //Rating
            var tmpRating = doc.DocumentNode.SelectNodes("//span[@itemprop='ratingValue']");
            if (tmpRating != null)
                IMDBRatings = tmpRating[0].InnerText;        
            //Running time
            var tmpRunningTime = doc.DocumentNode.SelectNodes("//time[@itemprop='duration']");
                if(tmpRunningTime != null)
              RunningTime = WebUtility.HtmlDecode(tmpRunningTime[0].ChildNodes[0].InnerText.Trim());       
            //Summary
            var tmpSummary = doc.DocumentNode.SelectNodes("//div[@class='summary_text']");
            if (tmpSummary != null)
            {
                var summaryTrim = WebUtility.HtmlDecode(tmpSummary[0].InnerText.Trim());
                if (!summaryTrim.Contains("Add a Plot"))
                Summary = WebUtility.HtmlDecode(tmpSummary[0].InnerText.Trim());
            }
            //Story Line
            var StoryLines = doc.DocumentNode.SelectNodes("//h2[.='Storyline']")[0].ParentNode;
            if (!(StoryLines.InnerHtml.Contains("Add Full Plot")))
                StoryLine = WebUtility.HtmlDecode(StoryLines.ChildNodes[5].ChildNodes[1].ChildNodes[0].InnerText.Trim());
            //Budget
            var tmpBudget = doc.DocumentNode.SelectNodes("//h4[.='Budget:']");               
            if (tmpBudget != null)
            {
                var Budget = WebUtility.HtmlDecode(tmpBudget[0].ParentNode.ChildNodes[2].InnerText.Trim());
                BoxOffice.Add("Budget", Budget);
            }
            //Opening Weekend
            var tmpOpeningWeekend = doc.DocumentNode.SelectNodes("//h4[.='Opening Weekend USA:']");
            if (tmpOpeningWeekend != null)
            {
                var openingWeekend = WebUtility.HtmlDecode(tmpOpeningWeekend[0].ParentNode.ChildNodes[2].InnerText.Trim());
                BoxOffice.Add("Opening Weekend USA", openingWeekend);
            }
            //Gross USA
            var tmpGross = doc.DocumentNode.SelectNodes("//h4[.='Gross USA:']");
            if (tmpGross != null)
            {
                var grossusa = WebUtility.HtmlDecode(tmpGross[0].ParentNode.ChildNodes[2].InnerText.Trim());
                BoxOffice.Add("Gross USA", grossusa);
            }
            //cumulitive gross
            var tmpcumuGross = doc.DocumentNode.SelectNodes("//h4[.='Cumulative Worldwide Gross:']");
            if (tmpcumuGross != null)
            {
                var cumuGross = WebUtility.HtmlDecode(tmpcumuGross[0].ParentNode.ChildNodes[2].InnerText.Trim());
                BoxOffice.Add("Cumulative Worldwide Gross", cumuGross);
            }
            //Director
            var tmpDirector = doc.DocumentNode.SelectNodes("//div[@class='credit_summary_item']");
            if (tmpDirector != null)
                Director = tmpDirector[0].ChildNodes[3].ChildNodes[1].InnerText;
            //writer
            var tmpWriters = doc.DocumentNode.SelectNodes("//div[@class='credit_summary_item']/span[@itemprop='creator']");
            if (tmpWriters != null)
                foreach (var writer in tmpWriters)
                    Writers.Add(writer.ChildNodes[1].InnerText);
            //Year Release
            var tmpYear = doc.DocumentNode.SelectNodes("//span[@id='titleYear']");
            if (tmpYear != null)
                Year = tmpYear[0].ChildNodes[1].InnerText;
            //Stars
            var tmpStar = doc.DocumentNode.SelectNodes("//div[@class='credit_summary_item']/span[@itemprop='actors']");
            if (tmpStar != null)
                foreach (var star in tmpStar)
                    Stars.Add(star.ChildNodes[1].InnerText);
            //Genre
            var genres = doc.DocumentNode.SelectNodes("//span[@itemprop='genre']");
            if (genres != null)
                foreach (var genre in genres)
                    Genres.Add(genre.InnerText);
            //Actors
            var Actors = doc.DocumentNode.SelectNodes("//td[@itemprop='actor']");
            if (Actors != null)
                foreach (var actor in Actors)
                    Cast.Add(actor.ChildNodes[1].ChildNodes[1].InnerText);
            //Country
            var tmpCountry = doc.DocumentNode.SelectNodes("//h4[.='Country:']");
            if (tmpCountry != null)
                foreach (var c in tmpCountry[0].ParentNode.ChildNodes)
                    if (c.Name == "a")
                    {
                        var Country = WebUtility.HtmlDecode(c.ChildNodes[0].InnerText.Trim());
                        Countries.Add(Country);
                    }
        }      
        HtmlDocument Gethtml(string url)
        {
            var doc = new HtmlDocument();
            using (WebClient c = new WebClient())
                doc.LoadHtml(c.DownloadString(url));
            return doc;
        }
    }
}
