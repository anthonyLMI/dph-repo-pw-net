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
	//protected PageTest PageTest { get; private set; } = null!;
	public LoginPage LoginPage { get; private set; } = null!;
	public OrderPage OrderPage { get; private set; } = null!; // Initialize OrderPage to null
	public LoginAssertions LoginAssertions { get; private set; } = null!;
	protected IBrowser Browser { get; private set; } = null!;
	protected IBrowserContext Context { get; private set; } = null!;
	protected static IPlaywright Playwright { get; private set; } = null!;

	[TestInitialize]
	public async Task TestInitialize()
	{
		// Initialize Playwright
		Playwright = await Microsoft.Playwright.Playwright.CreateAsync(); // Create Playwright instance

		// Launch the browser with configured options
		Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
		{
			Headless = false, // Set to false if you want to see the browser UI during tests
			SlowMo = 50, // Optional: slows down operations by 50ms for better debugging
			Args = ["--start-maximized"]
		});

		// Create a new browser context without viewport size
		Context = await Browser.NewContextAsync(new BrowserNewContextOptions
		{
			//ViewportSize = new ViewportSize { Width = 1280, Height = 720 }, // Set a default viewport size
			RecordVideoDir = "videos/", // Optional: record videos of the tests
			AcceptDownloads = true, // Enable downloads in the context

		});

		// Configure timeouts
		Context.SetDefaultTimeout(30000); // 30 seconds

		// Enable request/response logging (optional)
		await Context.RouteAsync("**/*", async route =>
		{
			// Log URL (optional)
			Console.WriteLine($"Request: {route.Request.Method} {route.Request.Url}");
			await route.ContinueAsync();
		});

		// Create a new page in the context
		Page = await Context.NewPageAsync();

		// Page Object Setup
		// Initialize Page with the current page
		LoginPage = new(Page);
		OrderPage = new(Page); 
		LoginAssertions = new(Page);

		// Login to app
		await LoginPage.NavigateToLoginPageAsync(); // Navigate to the login page

		// Setup tracing for debugging failed tests
		await Context.Tracing.StartAsync(new TracingStartOptions
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
			if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
			{
				var screenshotPath = $"screenshot/test-failure--{TestContext.TestName}--{DateTime.Now:yyyyMMddHHmmss}.png";
				await Page.ScreenshotAsync(new PageScreenshotOptions
				{
					Path = screenshotPath, // Save the screenshot to a file
					FullPage = true // Capture the full page screenshot
				});

				// Store tracing data
				await Context.Tracing.StopAsync(new TracingStopOptions
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
			if(Context != null) await Context.CloseAsync();
			if(Browser != null) await Browser.DisposeAsync();
		}

	}

	[ClassCleanup]
	public static void ClassCleanup()
	{
		// Dispose of Playwright if needed
		Playwright?.Dispose();
	}

	// TestContext for test information
	public TestContext TestContext { get; set; }
}
