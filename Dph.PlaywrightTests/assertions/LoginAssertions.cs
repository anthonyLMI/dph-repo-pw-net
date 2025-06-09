using AventStack.ExtentReports;
using Microsoft.Playwright;

namespace Dph.PlaywrightTests;

[TestClass]
public class LoginAssertions : BasePage
{
	public LoginAssertions(IPage page) : base(page)
	{
		LoginPage = new(Page);
		OrderPage = new(Page);
		PageTest = new PageTest();
	}
	/// <summary>
	ExtentTest Test = null!;
	Exception ex = null!;
	private readonly PageTest PageTest;

	public LoginPage LoginPage { get; private set; }
	public OrderPage OrderPage { get; private set; }

	public async Task ShoulBeDisplayPage()
	{
		//Create a test instance in ExtentReports
		ExtentReports Extent = GetInstance();
		Test = Extent.CreateTest("Should display Book Orders page");

		try
		{
			await PageTest.Expect(OrderPage.GetTitleOrderPageSelector()).ToHaveTextAsync("asd");

			// Log a pass status in the report
			Test.Pass($"<b>PASSED.</b> { OrderPage.GetTitleOrderPageSelector().InnerTextAsync().GetAwaiter().GetResult()}");
		}
		catch (Exception e)
		{
			// Log a fail status with the exception message
			Test.Fail("<b>FAILED.</b> The expected result did not match.");
			ex = e;
			//StepsToReplicateForReporting(reportDescription);
			await TakeScreenshotAsync($"Test - FAILED");
		}
		Extent.Flush();
	}
}
