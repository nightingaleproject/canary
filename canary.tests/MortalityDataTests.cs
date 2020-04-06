using Xunit;
using canary.Models;

namespace canary.tests
{
    public class MortalityDataTests
    {
        [Fact]
        public void StateNameToStateCode_Test()
        {
            MortalityData mortalityData = MortalityData.Instance;
            Assert.Equal("FL", mortalityData.StateNameToStateCode("Florida"));
            Assert.Equal("FL", mortalityData.StateNameToStateCode("FLOrida"));
            Assert.Equal("FL", mortalityData.StateNameToStateCode("  FLOrida "));
        }
    }
}
