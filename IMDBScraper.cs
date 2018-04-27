using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApplication7
{
    class IMDBScraper
    {
        string Title { get;  set; }
        string Year_Release { get;  set; } = "";
        List<string> Genre_List { get; set; }
        List<string> Cast_List { get; set; }

        string _jsonResults;

        public string jsonResults{get { return _jsonResults; } }

        public IMDBScraper(string url)
        {
            if (url != "")
            {
                Cast_List = new List<string>();
                Genre_List = new List<string>();
                var doc = Gethtml(url);
                if (doc.Text.Contains(@"content=""video.movie"""))
                    ParseHtml(doc);
            }
            _jsonResults = JsonConvert.SerializeObject(this);
        }

        void ParseHtml(HtmlDocument doc)
        {
            //Title
            Title = WebUtility.HtmlDecode(doc.DocumentNode.SelectNodes("//h1[@itemprop='name']")[0].ChildNodes[0].InnerText);
            Title = Title.TrimEnd();
          
            //Year Release
            var tmpyr = doc.DocumentNode.SelectNodes("//span[@id='titleYear']");
            if (tmpyr != null)
                Year_Release = tmpyr[0].ChildNodes[1].InnerText;

            //Genre
            var genres  = doc.DocumentNode.SelectNodes("//span[@itemprop='genre']");
            if(genres != null)
            foreach (var genre in genres)
                Genre_List.Add(genre.InnerText);
            //Actors
            var Actors = doc.DocumentNode.SelectNodes("//td[@itemprop='actor']");
            if(Actors != null)
            foreach (var actor in Actors)
                    Cast_List.Add(actor.ChildNodes[1].ChildNodes[1].InnerText);
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
