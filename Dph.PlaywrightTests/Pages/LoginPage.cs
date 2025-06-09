using Microsoft.Playwright;
using System.Runtime.InteropServices;

namespace Dph.PlaywrightTests;

public class LoginPage : BasePage
{
	// Selectors for the elements on the login page
	private ILocator TxtEmailSelector => Page.Locator("[data-qa='login-email-input']"); //#dph-login-email
	private ILocator TxtPasswordSelector => Page.Locator("[data-qa='login-password-input']"); //#dph-login-pass
	private ILocator BtnLoginSelector => Page.Locator("button[type='submit']"); //#dph-login-submit-btn
	private ILocator BtnBookOrdersSelector => Page.Locator("//ul[1]//li[1]");
	
	private ILocator BtnMyPostsSelector => Page.Locator("//ul[1]//li[2]");
	public LoginPage(IPage page) : base(page)
	{
		// Initialize the page elements here if needed
	}

	// Page Method
	// Navigation to the login page

	public async Task NavigateToLoginPageAsync()
	{
		await NavigateToAsync("https://staging.web.deliveries.ph/login");
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

	public async Task ClickMyPostsButtonAsync()
	{
		await BtnMyPostsSelector.ClickAsync();
	}

	public async Task LoginToWebApplication(string email, string password)
	{
		await EnterEmailAsync(email);
		await EnterPasswordAsync(password);
		await ClickLoginButtonAsync();
		//await ClickMyPostsButtonAsync();
	}

	public async Task ClickMyOrderButtonAsyn()
	{
		await BtnBookOrdersSelector.ClickAsync();
	}

	// Access the locators for testing purposes


}
