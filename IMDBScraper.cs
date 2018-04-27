using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApplication
{
    class IMDBScraper
    {
        public string Title { get; private set; } = "";
        public string Year { get; private set; } = "";

        public string Director { get; private set; } = "";
        public List<string> Writers { get; private set; } 
        public List<string> Stars { get; private set; }
        public List<string> Genres { get; private set; }
        public List<string> Cast { get; private set; }
        public string Summary { get; private set; } = "";
        public string StoryLine { get; private set; } = "";

        public string jsonResults() { return JsonConvert.SerializeObject(this);  }

        public IMDBScraper(string url)
        {
            if (url != "")
            {
                Cast = new List<string>();
                Genres = new List<string>();
                Writers = new List<string>();
                Stars = new List<string>();
                var doc = Gethtml(url);
                if (doc.Text.Contains(@"content=""video.movie"""))
                    ParseHtml(doc);
            }
           
        }

        void ParseHtml(HtmlDocument doc)
        {
            //Title
            Title = WebUtility.HtmlDecode(doc.DocumentNode.SelectNodes("//h1[@itemprop='name']")[0].ChildNodes[0].InnerText);
            Title = Title.TrimEnd();
            //summary and story line
            var tmpSummary = doc.DocumentNode.SelectNodes("//div[@itemprop='description']");
            if (tmpSummary != null)
            {
                //Summary
                if (tmpSummary[1] != null)
                    Summary = WebUtility.HtmlDecode(tmpSummary[1].ChildNodes[1].ChildNodes[0].InnerText.Trim());

                //story line
                if (tmpSummary[0] != null)
                    StoryLine = WebUtility.HtmlDecode(tmpSummary[0].ChildNodes[0].InnerText.Trim());
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
            var genres  = doc.DocumentNode.SelectNodes("//span[@itemprop='genre']");
            if(genres != null)
            foreach (var genre in genres)
                Genres.Add(genre.InnerText);
            //Actors
            var Actors = doc.DocumentNode.SelectNodes("//td[@itemprop='actor']");
            if(Actors != null)
            foreach (var actor in Actors)
                    Cast.Add(actor.ChildNodes[1].ChildNodes[1].InnerText);


            
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
