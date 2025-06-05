using Microsoft.Playwright;

namespace Dph.PlaywrightTests.Tests;

/// <summary>
/// Base test class that all test classes will inherit from
/// Contains common setup and teardown methods
/// </summary>
public class BaseTest
{
	// Playwright Objects
	protected IPage Page { get; private set; } = null!;
	protected IBrowser Browser { get; private set; } = null!;
	protected IBrowserContext BrowserContext { get; private set; } = null!;
	public TestContext TestContext { get; private set; } = null!;
	//protected IPlaywright Playwright { get; private set; } = null!;

	[TestInitialize]
	public async Task TestInitialize()
	{
		// Initialize Playwright
		var playwright = await Playwright.CreateAsync();

		// Launch the browser with configured options
		Browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
		{
			Headless = true, // Set to false if you want to see the browser UI during tests
			SlowMo = 50, // Optional: slows down operations by 50ms for better debugging
			Args = ["--start-maximized"]
		});

		BrowserContext = await Browser.NewContextAsync(new BrowserNewContextOptions
		{
			ViewportSize = new ViewportSize { Width = 1280, Height = 720 }, // Set a default viewport size
			RecordVideoDir = "videos/", // Optional: record videos of the tests
			AcceptDownloads = true // Enable downloads in the context

		});

		// Configure timeouts
		BrowserContext.SetDefaultTimeout(30000); // 30 seconds

		// Enable request/response logging (optional)
		await BrowserContext.RouteAsync("**/*", async route =>
		{
			// Log URL (optional)
			Console.WriteLine($"Request: {route.Request.Method} {route.Request.Url}");
			await route.ContinueAsync();
		});

		// Create a new page in the context
		Page = await BrowserContext.NewPageAsync();

		// Page Object Setup

		// Login to app

		// Setup tracing for debugging failed tests
		await BrowserContext.Tracing.StartAsync(new TracingStartOptions
		{
			Screenshots = true, // Capture screenshots on failure
			Snapshots = true, // Capture DOM snapshots on failure
			Title = "Test Trace" // Optional: title for the trace file
		});

	}

	[TestCleanup]
	public async Task TestCleanup()
	{
		try
		{
			// Take screenshot on test failure
			if (TestContext.CurrentTestOutcome != UnitTestOutcome.Failed)
			{
				var screenshotPath = $"screenshot/test-failure--{TestContext.TestName}--{DateTime.Now:yyyyMMddHHmmss}.png";
				await Page.ScreenshotAsync(new PageScreenshotOptions
				{
					Path = screenshotPath, // Save the screenshot to a file
					FullPage = true // Capture the full page screenshot
				});

				// Store tracing data
				await BrowserContext.Tracing.StopAsync(new TracingStopOptions
				{
					Path = $"traces/trace--{TestContext.TestName}.zip" // Save the trace file
				});

				Console.WriteLine($"Screenshot saved to: {screenshotPath}");
				TestContext.AddResultFile(screenshotPath); // Add screenshot to test results
			}
		}
		catch (Exception ex)
		{

			Console.WriteLine($"Error during cleanup: {ex.Message}");
		}
		finally
		{
			// Disp[ose of Playwright objects
			if(BrowserContext != null) await BrowserContext.CloseAsync();
			if(Browser != null) await Browser.DisposeAsync();
		}

		//[ClassCleanup]
		//public static void ClassCleanup()
		//{
		//	// Dispose of Playwright if needed
		//	IPlaywright playwright;
		//	playwright.Dispose();
		//	//Playwright?.Dispose();
		//}
	}
}
