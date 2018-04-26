using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication7
{
    //TODO: Rewrite search engine so it return 1 filtered node not containing illegal url. find common node and use regex to filter results;
    class SearchEngines
    {
        public static Dictionary<int,string> UseGoogle(string searchterm)
        {
            var dic = new Dictionary<int, string>();
            int x = 0, xx = 1;
            var doc = GetDocument(searchterm);
            var title = doc.DocumentNode.SelectNodes("//h3[@class='r']").Where(c => !(c.InnerText.Contains("Images for site")) && !(c.InnerText.Contains("News for")) && !(c.InnerText.Contains("Trailer")) && !(c.InnerText.Contains("Episodes")));
            var url = doc.DocumentNode.SelectNodes("//div[@class='s']");
            foreach (var tite in title.Where(c => !(c.InnerText.Contains("Images for site:www.imdb.com"))))
            {
                var reg = new Regex(@"https://www.imdb.com/title/tt\d{7}/");
                var match = reg.Match(url[x].ChildNodes[0].ChildNodes[0].InnerText);
                dic.Add(xx,match.Value);
                Console.WriteLine(WebUtility.HtmlDecode(xx + ") " + tite.InnerText));
                x++;
                xx++;
            }
            return dic;
        }

        static HtmlDocument GetDocument(string searchterm)
        {
            string html;
            HtmlDocument doc = new HtmlDocument();
            using (WebClient c = new WebClient())
            {
                html = c.DownloadString($"https://www.google.com/search?q=site:www.imdb.com+{searchterm}");
            }
            doc.LoadHtml(html);
            return doc;
        }
    }
    
}
