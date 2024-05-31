namespace OnlineStore
{
    public interface IPaymentService
    {
        bool ProcessPayment(Order order, double amount);
    }
}
