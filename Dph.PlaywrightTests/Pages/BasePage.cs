using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Microsoft.Playwright;

namespace Dph.PlaywrightTests;

public class BasePage
{
    // Protected properties
    protected IPage Page;
    public PageTest PageTest;
    protected readonly string BaseUrl;
	protected readonly int DefaultTimeout = 30000; // 30 seconds
	protected static ExtentSparkReporter Reports { get; private set; } = null!;
	protected static ExtentReports Extent { get; private set; } = null!;

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
		string filePath = $"screenshots/{fileName}--{DateTime.Now:yyyyMMddHHmmss} - screenshot.png"; //{Guid.NewGuid().ToString()[..3]} <-- for random unique ids
		await Page.ScreenshotAsync(new PageScreenshotOptions
		{
			Path = filePath,
			FullPage = true
		});
		return filePath;
	}

	// Take html report
	public static ExtentReports GetInstance()
	{
		if (Extent == null)
		{
			Reports = new ExtentSparkReporter("Reports/index.html");
			Extent = new ExtentReports();
			Extent.AttachReporter(Reports);
		}
		return Extent;
	}
}
