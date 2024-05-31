namespace OnlineStore
{
    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }

        public Product(string name, double price)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name cannot be empty.");
            }
            if (price <= 0)
            {
                throw new ArgumentException("Product price must be positive.");
            }
            Name = name;
            Price = price;
        }
    }
}
