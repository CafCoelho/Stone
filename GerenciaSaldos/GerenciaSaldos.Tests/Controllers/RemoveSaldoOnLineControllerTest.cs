using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using GerenciaSaldos.Controllers;
using System.Net;

namespace GerenciaSaldos.Tests.Controllers
{
    [TestClass]
    public class RemoveSaldoOnLineControllerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            RemoveSaldoOnLineController controller = new RemoveSaldoOnLineController();

            // Act
            HttpResponseMessage result = controller.Put(1, 1,"TTT") as HttpResponseMessage;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
