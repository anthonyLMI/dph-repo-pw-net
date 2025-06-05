using Microsoft.Playwright;

namespace Dph.PlaywrightTests;

public class BasePage
{
    // Protected properties
    protected IPage Page { get; private set; } = null!;
    protected PageTest PageTest { get; private set; } = null!;
    protected readonly string BaseUrl;
	protected readonly int DefaultTimeout = 30000; // 30 seconds
    
    public BasePage(IPage page, PageTest pageTest)
    {
        Page = page ?? throw new ArgumentNullException(nameof(page));
		PageTest = pageTest ?? throw new ArgumentNullException(nameof(pageTest));
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
