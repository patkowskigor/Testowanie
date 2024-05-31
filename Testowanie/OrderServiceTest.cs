using Moq;
using NUnit.Framework;
using OnlineStore;
using System;

namespace OnlineStoreTests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private Mock<ILogger> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger>();
        }

        [Test]
        public void CreateOrderWithProduct_ValidProductId_ReturnsOrder()
        {
            var product = new Product("Test Product", 10);
            _mockProductRepository.Setup(repo => repo.GetProductById(It.IsAny<int>())).Returns(product);

            var orderService = new OrderService(_mockProductRepository.Object, _mockLogger.Object);
            var order = orderService.CreateOrderWithProduct(1);

            Assert.AreEqual(1, order.Products.Count);
            Assert.AreEqual("Test Product", order.Products[0].Name);
        }

        [Test]
        public void CreateOrderWithProduct_InvalidProductId_ThrowsArgumentException()
        {
            _mockProductRepository.Setup(repo => repo.GetProductById(It.IsAny<int>())).Returns((Product)null);

            var orderService = new OrderService(_mockProductRepository.Object, _mockLogger.Object);

            Assert.Throws<ArgumentException>(() => orderService.CreateOrderWithProduct(1));
        }
    }
}
