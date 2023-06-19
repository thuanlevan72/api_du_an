using FOLYFOOD.Dto;
using FOLYFOOD.Dto.oderDto;
using FOLYFOOD.Entitys;

namespace FOLYFOOD.IService.IOrder
{
    public interface OrderInterface
    {
        public Task<RetunObject<Order>> CreateNewOrder(OrderRequest order);
        public Task<RetunObject<Order>> updateStatusOrder(int orderId,int statusId);
        public Task<IQueryable<Order>> GetAllOrders();
        public Task<RetunObject<Order>> getOrderForCodeOrder(string code);
        public Task<IQueryable<Order>> GetOrderForEmail(string code);
        public Task<RetunObject<Order>> getDetail(int id);
    }
}
