using System.Security.Cryptography;
using System.Web;
using FluentAssertions;
using Microsoft.Playwright;
using TryTest.Pages;

namespace TryTest;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test1()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            }
        );
        var page = await browser.NewPageAsync();
        await page.GotoAsync("http://www.eaapp.somee.com");
        await page.ClickAsync("text=Login");
        await page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = "tryty.jpg"
        });
        await page.FillAsync("#UserName", value: "admin");
        await page.FillAsync("#Password", value: "password");
        await page.ClickAsync("text=Log in");
        var isExist = await page.Locator("text='Employee Details'").IsVisibleAsync();
        Assert.IsTrue(isExist);
    }
    
    [Test]
    public async Task TestWithPOM()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            }
        );
        var page = await browser.NewPageAsync();
        await page.GotoAsync("http://www.eaapp.somee.com");
        LoginPageUpgraded loginPage = new LoginPageUpgraded (page);
        await loginPage.ClickLogin();
        await loginPage.FillLogin("admin", "password");
        //loginPage.IsEmployeeDetailsExists();
        var isExist = await loginPage.IsEmployeeDetailsExists();
        Assert.IsTrue(isExist);
    }
    
    [Test]
    public async Task TestNetwork()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            }
        );
        var page = await browser.NewPageAsync();
        await page.GotoAsync("http://www.eaapp.somee.com");
        LoginPageUpgraded loginPage = new LoginPageUpgraded (page);
        await loginPage.ClickLogin();
        await loginPage.FillLogin("admin", "password");
        
        var waitForRequest = page.WaitForRequestAsync("**/Employee"); 
        var waitForResponse = page.WaitForResponseAsync("**/Employee"); 
        await loginPage.ClickEmployeeList();
        var getRequest = await waitForRequest;
        var getResponse = await waitForResponse;
        
        var waitForNewResponse = page.RunAndWaitForResponseAsync(async() =>
        {
            await loginPage.ClickEmployeeList();
        }, x=>x.Url.Contains("Employee")&&x.Status == 200); 


        //loginPage.IsEmployeeDetailsExists();
        var isExist = await loginPage.IsEmployeeDetailsExists();
        Assert.IsTrue(isExist);
    }

    [Test]
    public async Task Flipkart()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        var page = await browser.NewPageAsync();
        await page.GotoAsync("http://www.flipkart.com/", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle
        });
        //await page.Locator("text=x").ClickAsync();
        await page.Locator("span", new PageLocatorOptions
        {
            HasTextString = "Login"
        }).ClickAsync();
        var request = await page.RunAndWaitForRequestAsync(async () =>
        {
            await page.Locator("text=x").ClickAsync();
        }, x => x.Method == "GET");
        var returnData = HttpUtility.UrlDecode(request.Url);
        Console.WriteLine(returnData);
        returnData.Should().Contain("flix");
    }

    [Test]
    public async Task FlipkartNetworkInterception()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        // page.Request += (_, request) => Console.WriteLine(request.Method + "----" + request.Url);
        // page.Response += (_, response) => Console.WriteLine(response.Status + "----" + response.Url);

        await page.RouteAsync("**/*", async route =>
        {
            if (route.Request.ResourceType == "image")
                await route.AbortAsync();
            else
            {
                await route.ContinueAsync();
            }
        });
        await page.GotoAsync("http://www.flipkart.com/", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle
        });
        
    }
}