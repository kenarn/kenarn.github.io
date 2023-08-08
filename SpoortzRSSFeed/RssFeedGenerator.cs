namespace SpoortzRSSFeed;

using SpoortzRSSFeed.Scrapers;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

public class RssFeedGenerator
{
    public static SyndicationFeed CreateRssFeed(List<NewsItem> newsItems)
    {
        var feed = new SyndicationFeed("Dale IL Nyheter", "Dale IL Spoortz nyheters RSS Feed", new Uri("https://www.dale-il.no"))
        {
            LastUpdatedTime = DateTimeOffset.Now
        };

        var items = new List<SyndicationItem>();
        foreach (var newsItem in newsItems)
        {
            var item = new SyndicationItem(
                title: newsItem.Subject,
                content: newsItem.Description,
                itemAlternateLink: null, // Add the link if available
                id: null, // Add the ID if available
                lastUpdatedTime: DateTimeOffset.Now
            );

            // Add an image to the content if you want
            string contentWithImage = $"<img src='{newsItem.ImageUrl}' /><p>{newsItem.Description}</p>";
            item.Content = SyndicationContent.CreateHtmlContent(contentWithImage);

            items.Add(item);
        }

        feed.Items = items;

        return feed;
    }
    public static string GenerateFeedAsString(SyndicationFeed feed)
    {
        var settings = new XmlWriterSettings
        {
            Indent = true,
            Encoding = Encoding.UTF8
        };

        using var memoryStream = new MemoryStream();
        using var xmlWriter = XmlWriter.Create(memoryStream, settings);

        feed.SaveAsRss20(xmlWriter);
        xmlWriter.Flush();

        memoryStream.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(memoryStream);
        return reader.ReadToEnd();
    }

    public static void SaveFeedToFile(SyndicationFeed feed, string path)
    {
        using var writer = XmlWriter.Create(path, new XmlWriterSettings { Indent = true });
        feed.SaveAsRss20(writer);
    }
}
