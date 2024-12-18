using System.Xml;
using System.ServiceModel.Syndication;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var svtRssUrl = "https://www.svt.se/rss.xml";
app.MapPost("/news", async context =>
{
    context.Request.Body.Dispose(); // i can't be fucked updating pladder to use http get instead of post, so here's some filth instead.
    using var rssReader = XmlReader.Create(svtRssUrl);
    var feed = SyndicationFeed.Load(rssReader);
    var items = feed.Items.ToArray();
    var randomIndex = new Random().Next(items.Length);
    var randomItem = items[randomIndex];
    await context.Response.WriteAsync(randomItem?.Summary?.Text ?? "Eror :(");
});

var flashbackRssUrl = "https://www.flashback.org/external.php?type=rss2&forumids=165,68,181,26,21,338,150,226,337,34,77,147,508,535";
app.MapPost("/flashback", async context =>
{
    context.Request.Body.Dispose();
    using var rssReader = XmlReader.Create(flashbackRssUrl);
    var feed = SyndicationFeed.Load(rssReader);
    var items = feed.Items.ToArray();
    var randomIndex = new Random().Next(items.Length);
    var randomItem = items[randomIndex];
    await context.Response.WriteAsync(randomItem?.Title?.Text ?? "Eror :(");
});

app.Run();
