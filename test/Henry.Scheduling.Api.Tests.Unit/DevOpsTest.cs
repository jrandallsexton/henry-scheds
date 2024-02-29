using FluentAssertions;
using Xunit;

namespace Henry.Scheduling.Api.Tests.Unit
{
    public class DevOpsTest
    {
        [Fact]
        public void TrueShouldBeTrue()
        {
            true.Should().BeTrue();
        }
    }
}
