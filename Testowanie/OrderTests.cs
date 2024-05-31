using Moq;
using NUnit.Framework;
using OnlineStore;
using System;

namespace OnlineStoreTests
{
    [TestFixture]
    public class OrderTests
    {
        private Mock<ILogger> _mockLogger;
        private Mock<IPaymentService> _mockPaymentService;
        private Mock<INotificationService> _mockNotificationService;
        private Mock<ICouponService> _mockCouponService;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger>();
            _mockPaymentService = new Mock<IPaymentService>();
            _mockNotificationService = new Mock<INotificationService>();
            _mockCouponService = new Mock<ICouponService>();
        }

        [Test]
        public void AddProduct_ValidProduct_LogsMessage()
        {
            var order = new Order(_mockLogger.Object);
            var product = new Product("Test Product", 10);
            order.AddProduct(product);
            _mockLogger.Verify(logger => logger.Log("Product added: Test Product"), Times.Once);
        }

        [Test]
        public void ApplyDiscount_ValidDiscount_LogsMessage()
        {
            var order = new Order(_mockLogger.Object);
            order.AddProduct(new Product("Product 1", 100));
            order.ApplyDiscount(10);
            _mockLogger.Verify(logger => logger.Log("Discount applied: 10%"), Times.Once);
        }

        [Test]
        public void ProcessPayment_ValidAmount_CallsProcessPayment()
        {
            var order = new Order(_mockLogger.Object);
            order.AddProduct(new Product("Product 1", 50));
            _mockPaymentService.Setup(service => service.ProcessPayment(order, 50)).Returns(true);

            var result = order.ProcessPayment(_mockPaymentService.Object, 50);

            Assert.IsTrue(result);
            _mockPaymentService.Verify(service => service.ProcessPayment(order, 50), Times.Once);
        }

        [Test]
        public void NotifyCustomer_ValidOrder_CallsSendOrderConfirmation()
        {
            var order = new Order(_mockLogger.Object);
            order.AddProduct(new Product("Product 1", 50));

            order.NotifyCustomer(_mockNotificationService.Object);

            _mockNotificationService.Verify(service => service.SendOrderConfirmation(order), Times.Once);
            _mockLogger.Verify(logger => logger.Log("Order confirmation sent."), Times.Once);
        }

        [Test]
        public void ApplyCoupon_ValidCoupon_ReturnsTrue()
        {
            var order = new Order(_mockLogger.Object);
            order.AddProduct(new Product("Product 1", 50));
            _mockCouponService.Setup(service => service.ValidateCoupon("DISCOUNT10")).Returns(true);

            var result = order.ApplyCoupon(_mockCouponService.Object, "DISCOUNT10");

            Assert.IsTrue(result);
            _mockLogger.Verify(logger => logger.Log("Coupon validation result for DISCOUNT10: True"), Times.Once);
        }

        [Test]
        public void ApplyCoupon_InvalidCoupon_ReturnsFalse()
        {
            var order = new Order(_mockLogger.Object);
            order.AddProduct(new Product("Product 1", 50));
            _mockCouponService.Setup(service => service.ValidateCoupon("INVALID")).Returns(false);

            var result = order.ApplyCoupon(_mockCouponService.Object, "INVALID");

            Assert.IsFalse(result);
            _mockLogger.Verify(logger => logger.Log("Coupon validation result for INVALID: False"), Times.Once);
        }

        [Test]
        public void CalculateTotal_SingleProduct_ReturnsCorrectTotal()
        {
            var order = new Order(_mockLogger.Object);
            var product = new Product("Product 1", 100);
            order.AddProduct(product);

            var total = order.CalculateTotal();

            Assert.AreEqual(100, total);
        }

        [Test]
        public void CalculateTotal_MultipleProducts_ReturnsCorrectTotal()
        {
            var order = new Order(_mockLogger.Object);
            var product1 = new Product("Product 1", 50);
            var product2 = new Product("Product 2", 150);
            order.AddProduct(product1);
            order.AddProduct(product2);

            var total = order.CalculateTotal();

            Assert.AreEqual(200, total);
        }

        [Test]
        public void ApplyDiscount_ZeroPercent_ReturnsOriginalTotal()
        {
            var order = new Order(_mockLogger.Object);
            order.AddProduct(new Product("Product 1", 100));

            var discountedTotal = order.ApplyDiscount(0);

            Assert.AreEqual(100, discountedTotal);
        }

        [Test]
        public void ApplyDiscount_HundredPercent_ReturnsZero()
        {
            var order = new Order(_mockLogger.Object);
            order.AddProduct(new Product("Product 1", 100));

            var discountedTotal = order.ApplyDiscount(100);

            Assert.AreEqual(0, discountedTotal);
        }
    }
}
