using NUnit.Framework;

namespace IntegrationTests;

using static IntegrationTests.Testing;

public class TestBase
{
    [SetUp]
    public async Task TestSetUp()
    {
        await ResetState();
    }
}