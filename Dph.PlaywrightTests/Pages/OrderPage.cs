using Microsoft.Playwright;

namespace Dph.PlaywrightTests;

[TestClass]
public class OrderPage : BasePage
{
	// Selectors for the elements on the order page
	private ILocator TitleOrderPageSelector => Page.Locator("//h1[normalize-space()='Book Orders']");

	public OrderPage(IPage page) : base(page)
	{

	}

	// Action Methods

	// Access the locators for testing purposes
	public ILocator GetTitleOrderPageSelector() { return TitleOrderPageSelector; }
	

}
