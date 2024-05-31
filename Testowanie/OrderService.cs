namespace OnlineStore
{
    public class OrderService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger _logger;

        public OrderService(IProductRepository productRepository, ILogger logger)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Order CreateOrderWithProduct(int productId)
        {
            var product = _productRepository.GetProductById(productId);
            if (product == null)
            {
                throw new ArgumentException("Invalid product ID.");
            }

            var order = new Order(_logger);
            order.AddProduct(product);
            return order;
        }
    }
}
