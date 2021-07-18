using Notebook.SharedKernel.Extensions;
using Xunit;

namespace Notebook.UnitTests.SharedKernel.Extensions
{
    public class ExtensionsTests
    {
        [Fact]
        public void Select_BySelector_Selects()
        {
            string test = "Test";

            bool actual = test.Select(from => from == test);

            Assert.True(actual);
        }
    }
}
