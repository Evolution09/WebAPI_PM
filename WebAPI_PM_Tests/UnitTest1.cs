using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Net.Http;

namespace WebAPI_PM_Tests
{
    [TestClass]
    public class UnitTest1
    {


        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var repo = new TestController<Widget>();
            var controller = new WidgetController(repo);
            var expected = repo.Find(1);

            // Act
            var actual = controller.GetWidget(1) as OkNegotiatedContentResult<Widget>;

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Id, actual.Content.Id);

        }
    }
}
