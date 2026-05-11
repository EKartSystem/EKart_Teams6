using AutoMapper;
using E_Kart_Application.DBContext;
using E_Kart_Application.DTOs.OrderDetails;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace E_Kart_Application.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _repository;
        private readonly IMapper _mapper;
        private readonly EkartContext _context;

        public OrderDetailService(IOrderDetailRepository repository, IMapper mapper, EkartContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<OrderDetailResponseDto>> GetAllAsync()
        {
            var orderDetails = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDetailResponseDto>>(orderDetails);
        }

        public async Task<OrderDetailResponseDto?> GetByIdAsync(int orderId, int productId)
        {
            var orderDetail = await _repository.GetByIdAsync(orderId, productId);
            if (orderDetail == null)
            {
                throw new NotFoundException("Order details not found.");
            }
            return _mapper.Map<OrderDetailResponseDto>(orderDetail);
        }

        public async Task<OrderDetailResponseDto> CreateAsync(CreateOrderDetailDto dto)
        {
            if (dto.Quantity <= 0)
            {
                throw new BadRequestException("Quantity must be greater than zero.");
            }
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == dto.ProductId);
            if (product == null)
            {
                throw new NotFoundException("Product not found.");
            }
            var orderExists = await _context.Orders.AnyAsync(o => o.OrderId == dto.OrderId);
            if (!orderExists)
            {
                throw new NotFoundException("Order not found.");
            }
            var existingOrderDetail = await _repository.GetByIdAsync(dto.OrderId, dto.ProductId);
            //if (existingOrderDetail != null)
            //{
            //    throw new BadRequestException("Product already exists in this order.");
            //}
            var orderDetail = new OrderDetail
            {
                OrderId = dto.OrderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = product.UnitPrice ?? 0,
                Discount = 0
            };
            var createdOrderDetail = await _repository.CreateAsync(orderDetail);
            return _mapper.Map<OrderDetailResponseDto>(createdOrderDetail);
        }

        public async Task<OrderDetailResponseDto?> UpdateAsync(int orderId, int productId, UpdateOrderDetailDto dto)
        {
            if (dto.Quantity <= 0)
            {
                throw new BadRequestException("Quantity must be greater than zero.");
            }
            var existingOrderDetail = await _repository.GetByIdAsync(orderId, productId);
            if (existingOrderDetail == null)
            {
                throw new NotFoundException("Order details not found.");
            }
            var orderDetail = _mapper.Map<OrderDetail>(dto);
            orderDetail.OrderId = orderId;
            orderDetail.ProductId = productId;
            var updatedOrderDetail = await _repository.UpdateAsync(orderDetail);
            return _mapper.Map<OrderDetailResponseDto>(updatedOrderDetail);
        }

        public async Task DeleteAsync(int orderId, int productId)
        {
            var isDeleted = await _repository.DeleteAsync(orderId, productId);
            if (!isDeleted)
            {
                throw new NotFoundException("Order details not found.");
            }
        }
    }
}