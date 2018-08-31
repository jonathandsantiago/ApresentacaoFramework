using Framework.Helper.Extension;
using System;
using Xunit;

namespace Framework.Helper.Tests.Extension
{
    public class StringExtensionTests
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("Nome", "NomeId".RemoveFromEnd("Id"));
            Assert.Equal("Nome", "IdNome".RemoveFromEnd("Id"));
        }
    }
}
