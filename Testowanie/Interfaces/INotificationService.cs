namespace OnlineStore
{
    public interface INotificationService
    {
        void SendOrderConfirmation(Order order);
    }
}
