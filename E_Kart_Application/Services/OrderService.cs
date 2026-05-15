using AutoMapper;
using E_Kart_Application.DBContext;
using E_Kart_Application.DTOs.Orders;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace E_Kart_Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly EKARTContext _context;

        public OrderService(IOrderRepository repository, IMapper mapper, EKARTContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null)
            {
                throw new NotFoundException($"Order with id {id} not found");
            }
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerAsync(string customerId)
        {
            var orders = await _repository.GetByCustomerAsync(customerId);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<IEnumerable<OrderDto>> GetRecentOrdersAsync(int count)
        {
            if (count <= 0)
            {
                throw new BadRequestException("Count must be greater than zero");
            }
            var orders = await _repository.GetRecentAsync(count);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByShipperAsync(int shipperId)
        {
            var orders = await _repository.GetByShipperAsync(shipperId);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)
        {
            if (dto.Products == null || !dto.Products.Any())
            {
                throw new BadRequestException("Order must contain at least one product.");
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            var productIds = dto.Products.Select(p => p.ProductId).ToList();
            var products = await _context.Products.Where(p => productIds.Contains(p.ProductId)).ToListAsync();

            foreach (var item in dto.Products)
            {
                if (item.Quantity <= 0)
                {
                    throw new BadRequestException($"Quantity must be greater than zero");
                }
                var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product == null)
                {
                    throw new NotFoundException($"Product with ID {item.ProductId} not found.");
                }
                if ((product.UnitsInStock ?? 0) < item.Quantity)
                {
                    throw new BadRequestException($"Insufficient stock for Product ID {item.ProductId}");
                }
            }
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == dto.CustomerId);
            if (!customerExists)
            {
                throw new NotFoundException("Customer not found.");
            }
            var order = _mapper.Map<Order>(dto);
            order.OrderDate = DateTime.Now;
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var item in dto.Products)
            {
                var product = products.First(p => p.ProductId == item.ProductId);
                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.UnitPrice ?? 0,
                    Discount = 0
                };
                await _context.OrderDetails.AddAsync(orderDetail);
                product.UnitsInStock -= item.Quantity;
            }
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> UpdateOrderAsync(int id, UpdateOrderDto dto)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null)
            {
                throw new NotFoundException($"Order with id {id} not found");
            }
            _mapper.Map(dto, order);
            await _repository.UpdateAsync(order);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> UpdateOrderStatusAsync(int id, DateTime? shippedDate)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null)
            {
                throw new NotFoundException($"Order with id {id} not found");
            }
            order.ShippedDate = shippedDate;
            await _repository.UpdateAsync(order);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> UpdateOrderAddressAsync(int id, UpdateOrderAddressDto dto)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null)
            {
                throw new NotFoundException($"Order with id {id} not found");
            }
            _mapper.Map(dto, order);
            await _repository.UpdateAsync(order);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> UpdateOrderShipperAsync(int id, int shipVia)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null)
            {
                throw new NotFoundException($"Order with id {id} not found");
            }
            order.ShipVia = shipVia;
            await _repository.UpdateAsync(order);
            return _mapper.Map<OrderDto>(order);
        }
    }
}