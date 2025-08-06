namespace Hoshi.Repositories.OrderVisitService
{
    public interface IOrderVisitService
    {
        Task sendNoificationforclient(int orderId , string message);
        Task sendNoificationforclient2(int VisitId, string message);
    }
}
