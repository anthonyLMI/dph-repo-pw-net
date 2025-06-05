using Microsoft.Playwright;

namespace Dph.PlaywrightTests;

public class LoginPage : BasePage
{
	// Selectors for the elements on the login page
	private ILocator TxtEmailSelector => Page.Locator("#dph-login-email");
	private ILocator TxtPasswordSelector => Page.Locator("#dph-login-pass");
	private ILocator BtnLoginSelector => Page.Locator("#dph-login-submit-btn");
	public LoginPage(IPage page, PageTest pageTest) : base(page, pageTest)
	{
		// Initialize the page elements here if needed
	}

	// Page Method
	// Navigation to the login page

	public async Task NavigateToLoginPageAsync()
	{
		await NavigateToAsync("https://staging.deliveries.ph/");
	}

	// Action Methods
	public async Task EnterEmailAsync(string email)
	{
		await TxtEmailSelector.FillAsync(email);
	}

	public async Task EnterPasswordAsync(string password)
	{
		await TxtPasswordSelector.FillAsync(password);
	}

	public async Task ClickLoginButtonAsync()
	{
		await BtnLoginSelector.ClickAsync();
	}

	public async Task LoginToWebApplication(string email, string password)
	{
		await EnterEmailAsync(email);
		await EnterPasswordAsync(password);
		await ClickLoginButtonAsync();
	}
}
