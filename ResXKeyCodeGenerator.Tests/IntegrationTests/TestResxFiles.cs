using FluentAssertions;
using Xunit;

namespace Ihon.ResXKeyCodeGenerator.Tests.IntegrationTests;

public class TestResxFiles
{
    [Fact]
    public void TestKeyGeneration()
    {
        Test1.CreateDate.Should().Be("CreateDate");
        Test1.CreateDateDescending.Should().Be("CreateDateDescending");

        Test2.CreateDate.Should().Be("CreateDate");
        Test2.CreateDateDescending.Should().Be("CreateDateDescending");

        Test1.Create_Date.Should().Be("Create.Date");
    }
}
