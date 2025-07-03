using _305.Tests.Integration.Base.Factory;
using NUnit.Framework;
using System.Net;

namespace _305.Tests.Integration.Health;

[TestFixture]
public class HealthCheckTests
{
    private CustomWebApplicationFactory _factory = null!;

    [SetUp]
    public void SetUp()
    {
        _factory = new CustomWebApplicationFactory();
    }

    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
    }

    [Test]
    public async Task Health_Endpoint_Should_Return_OK()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/health");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
