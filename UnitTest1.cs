using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests;



[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
    private IBrowser? Browser { get; set; }
    private IPage _page;
    private IBrowserContext context;
    [Test]
    public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
    {
       await Page.GotoAsync("https://playwright.dev");
		
		var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        //Browser
        Browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });

        _page = await Browser.NewPageAsync();

        await _page.Context.Tracing.StartAsync(new TracingStartOptions
        {
            Title = "zare43", //Note this is for MSTest only. 
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });

       
        _page.GotoAsync("http://www.eaapp.somee.com");
        Thread.Sleep(5000);

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));

        // create a locator
        var getStarted = Page.GetByRole(AriaRole.Link, new() { Name = "Get started" });

        // Expect an attribute "to be strictly equal" to the value.
        await Expect(getStarted).ToHaveAttributeAsync("href", "/docs/intro");

        // Click the get started link.
        await getStarted.ClickAsync();

        // Expects the URL to contain intro.
        await Expect(Page).ToHaveURLAsync(new Regex(".*intro"));


        if (Browser != null)
        {
            await _page.Context.Tracing.StopAsync(new TracingStopOptions
            {
                Path = "videos2.zip" //+ context.ToString()
            });

            await Browser.CloseAsync();
        }
    }
}