using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;


namespace WebScraperWebApplication.web
{
    public  class Scraper
    {
        public static IEnumerable<Headline> GetHeadlines()
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.UserAgent] = "Lakewood..";
                string html = client.DownloadString($"http://www.thelakewoodscoop.com/");
                return ParseHtml(html);
            }
        }
        public static IEnumerable<Headline> ParseHtml(string html)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(html);
            IEnumerable<IElement> elements = document.QuerySelectorAll("div.post");
            List<Headline> headlines = new List<Headline>();
            foreach(var div in elements)
            {
                var headline = new Headline();
                headline.Title = div.QuerySelector("h2 a").TextContent;
                headline.Link = div.QuerySelector("h2 a").GetAttribute("href");
                headline.ImageUrl = div.QuerySelector("p img").GetAttribute("src");
                headline.Date = div.QuerySelector("div.postmetadata-top small").TextContent;

                headlines.Add(headline);
            }
            return headlines;
        }
        
    }
}