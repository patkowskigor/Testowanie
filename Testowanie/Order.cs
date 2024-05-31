using System;
using System.Collections.Generic;

namespace OnlineStore
{
    public class Order
    {
        private List<Product> _products;
        private readonly ILogger _logger;
        public IReadOnlyList<Product> Products => _products.AsReadOnly();

        public Order(ILogger logger)
        {
            _products = new List<Product>();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void AddProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            _products.Add(product);
            _logger.Log($"Product added: {product.Name}");
        }

        public double CalculateTotal()
        {
            double total = 0;
            foreach (var product in _products)
            {
                total += product.Price;
            }
            return total;
        }

        public double ApplyDiscount(double discountPercentage)
        {
            if (discountPercentage < 0 || discountPercentage > 100)
            {
                throw new ArgumentException("Discount percentage must be between 0 and 100.");
            }

            double total = CalculateTotal();
            double discountedTotal = total - (total * (discountPercentage / 100));
            _logger.Log($"Discount applied: {discountPercentage}%");
            return discountedTotal;
        }

        public bool ProcessPayment(IPaymentService paymentService, double amount)
        {
            return paymentService.ProcessPayment(this, amount);
        }

        public void NotifyCustomer(INotificationService notificationService)
        {
            notificationService.SendOrderConfirmation(this);
            _logger.Log("Order confirmation sent.");
        }

        public bool ApplyCoupon(ICouponService couponService, string couponCode)
        {
            var isValid = couponService.ValidateCoupon(couponCode);
            _logger.Log($"Coupon validation result for {couponCode}: {isValid}");
            return isValid;
        }
    }
}
