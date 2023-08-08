using SpoortzRSSFeed;
using SpoortzRSSFeed.Scrapers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.MapGet("/siste.rss", async () =>
{
	try
	{
        var scraper = new NewsScraper();
        var newsItems = await scraper.GetTopNewsItems(10);
        var feed = RssFeedGenerator.CreateRssFeed(newsItems);
        return Results.Text(RssFeedGenerator.GenerateFeedAsString(feed), "application/xml");
    }
	catch (Exception)
	{
        return Results.BadRequest<string>("Something went wrong, and I dont want to share with you what that was.");
	}
    
});

app.Run();


