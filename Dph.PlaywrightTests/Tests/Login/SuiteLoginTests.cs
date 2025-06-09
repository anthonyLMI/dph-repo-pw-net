using Dph.PlaywrightTests.Tests;

namespace Dph.PlaywrightTests;

[TestClass]
[TestCategory("LOGIN SUITES")]
public class SuiteLoginTests : BaseTest
{
    [TestMethod]
    [Description("First Test of Login")]
    [TestCategory("Login")]
	public async Task VerifyInputValidCredentials()
    {
        await LoginPage.LoginToWebApplication("rm@qa.fleet.ph", "LMI@2020");
        await LoginAssertions.ShoulBeDisplayPage();
	}
}
