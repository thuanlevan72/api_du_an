using FOLYFOOD.Dto;
using FOLYFOOD.Dto.oderDto;
using FOLYFOOD.Dto.oderDto.orderDetailDto;
using FOLYFOOD.Entitys;
using FOLYFOOD.Hellers;
using FOLYFOOD.Hellers.Mail;
using FOLYFOOD.IService.IOrder;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace FOLYFOOD.Services.order
{
    public class OrderServicer : OrderInterface
    {
        public readonly Context DBContext;

        public OrderServicer()
        {
            DBContext = new Context();
        }
        public async Task<RetunObject<Order>> CreateNewOrder(OrderRequest order)
        {

            try
            {
                if (string.IsNullOrEmpty(order.FullName) || order.Phone == null || order.OrderStatusId == null || order.PaymentId == null || order.orderDetailDtos == null || string.IsNullOrEmpty(order.Address) || string.IsNullOrEmpty(order.Email) || string.IsNullOrEmpty(order.Phone))
                {
                    throw new Exception("dữ liệu truyền lên không đầy đủ");
                }
                foreach (var item in order.orderDetailDtos)
                {
                    if (DBContext.Products.SingleOrDefault(x => x.ProductId == item.ProductId) == null)
                    {
                        throw new Exception("sản phẩm gửi lên không tồn tại");
                    }
                }
                if (DBContext.PaymentOrders.SingleOrDefault(x => x.PaymentId == order.PaymentId) == null)
                {
                    throw new Exception("phương thức thanh toán không tồn tại");
                }
                if (DBContext.OrderStatuses.SingleOrDefault(x => x.OrderStatusId == order.OrderStatusId) == null)
                {
                    throw new Exception("trạng thái hóa đơn không tồn tại");
                }
            }
            catch (Exception ex)
            {
                return new RetunObject<Order>()
                {
                    data = null,
                    mess = ex.Message,
                    statusCode = 401
                };
            }

            var orderCreate = new Order()
            {
                CodeOrder = Generate.GenerateOrderCode(),
                OrderStatusId = order.OrderStatusId,
                PaymentOrderPaymentId = 1,
                Address = order.Address,
                Email = order.Email,
                Phone = order.Phone,
                FullName = order.FullName,
                actualPrice = order.actualPrice,
                UserId = order.UserId,
                originalPrice = order.originalPrice,
                
            };
            await DBContext.Orders.AddAsync(orderCreate);
            await DBContext.SaveChangesAsync();
            List<OrderDetail> listOrder = new List<OrderDetail>();
            foreach(OrderDetailRequest detail in order.orderDetailDtos)
            {
                var item = new OrderDetail()
                {
                    OrderId = orderCreate.OrderId,
                    Price = detail.Price,
                    Quantity = detail.Quantity,
                    ProductId = detail.ProductId
                };
                listOrder.Add(item);
            }
            await DBContext.OrderDetails.AddRangeAsync(listOrder);
            await DBContext.SaveChangesAsync();


            return new RetunObject<Order>()
            {
                data = orderCreate,
                mess = "đã tạo hóa đơn thành công",
                statusCode = 201
            };
        }

        public async Task<IQueryable<Order>> GetAllOrders()
        {
           return DBContext.Orders.Include(x => x.OrderStatus).Include(x => x.OrderDetails).Include(x => x.PaymentOrder).Include(x => x.User).Include(x => x.OrderDetails).AsQueryable();
        }

        public async Task<RetunObject<Order>> getOrderForCodeOrder(string code)
        {
            var dataOne = await DBContext.Orders.Include(x=>x.OrderStatus).Include(x=>x.PaymentOrder).Include(x => x.OrderDetails).ThenInclude(x=>x.Product).FirstOrDefaultAsync(x => x.CodeOrder == code);
            if (dataOne == null)
            {
                return new RetunObject<Order>()
                {
                    data = null,
                    mess = "thông tin hóa đơn không tồn tại",
                    statusCode = 400
                };
            }
            SendMail.send(dataOne.Email, OrderEmailTemplate.GenerateOrderEmail(dataOne), "test mail");
            return new RetunObject<Order>()
            {
                data = dataOne,
                mess = "đã lấy được hóa đơn thành công",
                statusCode = 201
            }; ;
        }


        public async Task<RetunObject<Order>> updateStatusOrder(int orderId, int statusId)
        {
            var dataOne = DBContext.Orders.SingleOrDefault(x => x.OrderId == orderId);
            try
            {
                if(dataOne == null)
                {
                    throw new Exception("đơn hàng không tồn tại");
                }
                if(DBContext.OrderStatuses.SingleOrDefault(x=>x.OrderStatusId == statusId) == null)
                {
                    throw new Exception("trạng thái chưa được định nghĩa");
                }
            }
            catch (Exception ex)
            {
                return new RetunObject<Order>()
                {
                    data = null,
                    mess = ex.Message,
                    statusCode = 401
                };
            }
            dataOne.OrderStatusId = statusId;
            DBContext.Orders.Update(dataOne);
            DBContext.SaveChanges();
            return new RetunObject<Order>()
            {
                data = dataOne,
                mess = "trạng thái đã được thay đổi",
                statusCode = 201
            };
        }

        public async Task<IQueryable<Order>> GetOrderForEmail(string code)
        {
            return DBContext.Orders.Where(x=>x.Email == code).Include(x => x.OrderStatus).Include(x => x.PaymentOrder).Include(x => x.OrderDetails).ThenInclude(x=>x.Product).AsQueryable();
        }

        public async Task<RetunObject<Order>> getDetail(int id)
        {
            var dataOne = await DBContext.Orders.Include(x => x.OrderStatus).Include(x => x.PaymentOrder).Include(x => x.OrderDetails).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.OrderId == id);
            if(dataOne == null)
            {
                return new RetunObject<Order>()
                {
                    data = null,
                    mess = "thông tin hóa đơn không tồn tại",
                    statusCode = 400
                };
            }
            return new RetunObject<Order>()
            {
                data = dataOne,
                mess = "đã lấy được hóa đơn thành công",
                statusCode = 200
            }; 
        }
    }
}
