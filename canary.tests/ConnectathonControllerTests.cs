using Xunit;
using canary.Controllers;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using VRDR;

namespace canary.tests
{
    public class ConnectathonControllerTests
    {
        [Fact]
        public void Index_ReturnsDeathRecordArray()
        {
            // Arrange
            var controller = new ConnectathonController();

            // Act
            var result = controller.Index();

            // Assert
            var deathRecordArray = Assert.IsType<DeathRecord[]>(result);
            Assert.Equal(3, result.Length);
        }
    }
}
