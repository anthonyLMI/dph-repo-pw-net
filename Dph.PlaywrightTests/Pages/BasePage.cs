using Microsoft.Playwright;

namespace Dph.PlaywrightTests;

public class BasePage
{
    // Protected properties
    protected IPage Page;
    //protected PageTest PageTest;
    protected readonly string BaseUrl;
	protected readonly int DefaultTimeout = 30000; // 30 seconds
    
    public BasePage(IPage page)
    {
        Page = page;
		//PageTest = pageTest;
	}

    public virtual async Task NavigateToAsync(string url ="")
    {
        // This method can be overridden in derived classes for additional initialization
        await Page.GotoAsync($"{BaseUrl}{url}", new PageGotoOptions { Timeout = DefaultTimeout });
	}

	// Take a screenshot
	public async Task<string> TakeScreenshotAsync(string fileName)
	{
		string filePath = $"screenshots/{fileName}";
		await Page.ScreenshotAsync(new PageScreenshotOptions
		{
			Path = filePath,
			FullPage = true
		});
		return filePath;
	}
}
