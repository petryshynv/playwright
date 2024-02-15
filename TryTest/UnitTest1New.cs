using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace TryTest;

public class NUnitPlayWright: PageTest
{
    [SetUp]
    public async Task Setup()
    {
        await Page.GotoAsync("http://www.eaapp.somee.com");
    }

    [Test]
    public async Task Test2()
    {
        
        await Page.ClickAsync("text=Login");
        await Page.FillAsync("#UserName", value: "admin");
        await Page.FillAsync("#Password", value: "password");
        await Page.ClickAsync("text=Log in");
        await Expect(Page.Locator("text='Employee Details'")).ToBeVisibleAsync();
    }
}