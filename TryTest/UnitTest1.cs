// using Microsoft.Playwright;
//
// namespace TryTest;
//
// public class Tests
// {
//     [SetUp]
//     public void Setup()
//     {
//     }
//
//     [Test]
//     public async Task Test1()
//     {
//         using var playwright = await Playwright.CreateAsync();
//         await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
//             {
//                 Headless = false
//             }
//         );
//         var page = await browser.NewPageAsync();
//         await page.GotoAsync("http://www.eaapp.somee.com");
//         await page.ClickAsync("text=Login");
//         await page.ScreenshotAsync(new PageScreenshotOptions
//         {
//             Path = "tryty.jpg"
//         });
//         await page.FillAsync("#UserName", value: "admin");
//         await page.FillAsync("#Password", value: "password");
//         await page.ClickAsync("text=Log in");
//         var isExist = await page.Locator("text='Employee Details'").IsVisibleAsync();
//         Assert.IsTrue(isExist);
//     }
// }