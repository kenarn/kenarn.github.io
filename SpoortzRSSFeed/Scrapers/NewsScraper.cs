using Microsoft.Playwright;

namespace SpoortzRSSFeed.Scrapers;
public class NewsScraper
{
    public async Task<List<NewsItem>> GetTopNewsItems(int count)
    {
        var newsItems = new List<NewsItem>();

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        var page = await browser.NewPageAsync();

        await page.GotoAsync("https://dale-il.no/portal/arego/club/947/news");
        await page.WaitForTimeoutAsync(3000);

        var newsLinks = await page.EvaluateAsync<string[]>("Array.from(document.querySelectorAll('app-news-block a')).map(a => a.href)");

        for (int i = 0; i < Math.Min(count, newsLinks.Length); i++)
        {
            await page.GotoAsync(newsLinks[i]);
            await page.WaitForSelectorAsync(".description-text");

            string imageUrl = await page.EvaluateAsync<string>("document.querySelector('.item').style.backgroundImage.slice(4, -1).replace(/['\"]/g, '')");
            string subject = await page.EvaluateAsync<string>("document.querySelector('.description-text').innerText");
            string description = await page.EvaluateAsync<string>("document.querySelector('p:not([class])').innerText");

            newsItems.Add(new NewsItem
            {
                ImageUrl = imageUrl,
                Subject = subject,
                Description = description
            });
        }

        return newsItems;
    }
}
