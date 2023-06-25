using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FOLYFOOD.Dto;
using FOLYFOOD.Dto.oderDto;
using FOLYFOOD.Dto.oderDto.orderDetailDto;
using FOLYFOOD.Entitys;
using FOLYFOOD.Hellers;
using FOLYFOOD.Hellers.Mail;
using FOLYFOOD.Hellers.validate;
using FOLYFOOD.IService.IOrder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System.Data;

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
                noteOrder = !string.IsNullOrEmpty(order.noteOrder) ? order.noteOrder : "",
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
            await this.getOrderForCodeOrder(orderCreate.CodeOrder);

            return new RetunObject<Order>()
            {
                data = orderCreate,
                mess = "đã tạo hóa đơn thành công",
                statusCode = 201
            };
        }

        public async Task<IQueryable<Order>> GetAllOrders()
        {
            var DataOrder = DBContext.Orders.Include(x => x.OrderDetails).ThenInclude(x => x.Product);
            var res = DataOrder.Include(x => x.OrderDetails).Include(x => x.OrderStatus).AsNoTracking().AsQueryable();
            return res.OrderByDescending(x=>x.CreatedAt);
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
            if(ValidateValue.IsValidEmail(dataOne.Email)) {
                SendMail.send(dataOne.Email, OrderEmailTemplate.GenerateOrderEmail(dataOne,"thông báo đơn"), "foly food");
            }
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
            if (ValidateValue.IsValidEmail(dataOne.Email))
            {
                SendMail.send(dataOne.Email, OrderEmailTemplate.GenerateOrderEmail(dataOne, "Cập nhật trạng thái đơn"), "foly food");
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
        public async Task<IQueryable<Order>> orderIsCanceled()
        {
            var DataOrder = DBContext.Orders.Where(x=>x.OrderStatusId == 7).Include(x => x.OrderDetails).ThenInclude(x => x.Product);
            var res = DataOrder.Include(x => x.OrderDetails).Include(x => x.OrderStatus).AsNoTracking().AsQueryable();
            return res.OrderByDescending(x => x.CreatedAt);
        }
        public async Task<IQueryable<Order>> getOrdersAreBeingDelivered()
        {
            var DataOrder = DBContext.Orders.Where(x => x.OrderStatusId == 9).Include(x => x.OrderDetails).ThenInclude(x => x.Product);
            var res = DataOrder.Include(x => x.OrderDetails).Include(x => x.OrderStatus).AsNoTracking().AsQueryable();
            return res.OrderByDescending(x => x.CreatedAt);
        }
        public async Task<IQueryable<Order>> getWaitingOrder()
        {
            var DataOrder = DBContext.Orders.Where(x => x.OrderStatusId == 4).Include(x => x.OrderDetails).ThenInclude(x => x.Product);
            var res = DataOrder.Include(x => x.OrderDetails).Include(x => x.OrderStatus).AsNoTracking().AsQueryable();
            return res.OrderByDescending(x => x.CreatedAt);
        }
        public async Task<IQueryable<Order>> getOrderComplete()
        {
            var DataOrder = DBContext.Orders.Where(x => x.OrderStatusId == 5).Include(x => x.OrderDetails).ThenInclude(x => x.Product);
            var res = DataOrder.Include(x => x.OrderDetails).Include(x => x.OrderStatus).AsNoTracking().AsQueryable();
            return res.OrderByDescending(x => x.CreatedAt);
        }
        public async Task<IQueryable<Order>> GetOrderForEmail(string email,string accountId,string role)
        {
            if(role != "admin")
            {
                var user  = DBContext.Accounts.SingleOrDefault(x=>x.User.Email == email);
                if(int.Parse(accountId) != user.AccountId)
                {
                    return null;
                }
            }
            return DBContext.Orders.Where(x=>x.Email == email).Include(x => x.OrderStatus).Include(x => x.PaymentOrder).Include(x => x.OrderDetails).ThenInclude(x=>x.Product).OrderByDescending(x=>x.CreatedAt).AsNoTracking().AsQueryable();
        }

        public async Task<RetunObject<Order>> cancelOrder(string code, string accountId, string role) {
            var order = DBContext.Orders.Include(x => x.User).SingleOrDefault(x => x.CodeOrder == code);
            try
            {
                if(order == null)
                {
                    throw new Exception("đơn hàng không tồn tại");
                }
                if (role != "admin")
                {
                    var user = order.User;
                    if (int.Parse(accountId) != user.AccountId)
                    {
                        throw new Exception("bạn không quyền hủy đơn hàng của người khác");
                    }
                }
                DateTime currentDate = DateTime.Today;
                DateTime availableDate = order.CreatedAt;

                TimeSpan difference = currentDate - availableDate;
                int daysDifference = difference.Days;
                if (daysDifference > 2)
                {
                    throw new Exception("đơn hàng của quý khách đã quá hạn để hủy");
                }

                if(order.OrderStatusId != 4)
                {
                    throw new Exception("đơn hàng đã đang quá trình giao không được hủy");
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
            order.User = null;
            order.OrderStatusId = 7;
            DBContext.Orders.Update(order);
            DBContext.SaveChanges();
            return new RetunObject<Order>()
            {
                data = order,
                mess = "đơn hàng đã bị hủy",
                statusCode = 201
            };
        }

        public async Task<IQueryable<OrderDetail>> getDetail(int id)
        {
            var dataOne = DBContext.OrderDetails.Where(x=>x.OrderId == id).Include(x=>x.Product).AsQueryable();
            if(dataOne == null)
            {
                return null;
            }
           return dataOne;
        }
    }
}
