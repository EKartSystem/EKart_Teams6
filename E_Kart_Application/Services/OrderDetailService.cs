using AutoMapper;
using E_Kart_Application.DTOs.OrderDetails;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;

namespace E_Kart_Application.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderDetailService(
            IOrderDetailRepository repository,
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IMapper mapper)
        {
            _repository = repository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDetailResponseDto>> GetAllAsync()
        {
            var orderDetails = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<OrderDetailResponseDto>>(orderDetails);
        }

        public async Task<OrderDetailResponseDto?> GetByIdAsync(
            int orderId,
            int productId)
        {
            var orderDetail =
                await _repository.GetByIdAsync(orderId, productId);

            if (orderDetail == null)
            {
                throw new NotFoundException("Order details not found.");
            }

            return _mapper.Map<OrderDetailResponseDto>(orderDetail);
        }

        public async Task<IEnumerable<OrderDetailResponseDto>>
            GetByCustomerIdAsync(string customerId)
        {
            var orderDetails =
                await _repository.GetByCustomerIdAsync(customerId);

            return _mapper.Map<IEnumerable<OrderDetailResponseDto>>(orderDetails);
        }

        public async Task<IEnumerable<OrderDetailResponseDto>>
            GetByOrderIdAsync(int orderId)
        {
            var orderDetails = await _repository.GetByOrderIdAsync(orderId);
            return _mapper.Map<IEnumerable<OrderDetailResponseDto>>(orderDetails);
        }

        public async Task<OrderDetailResponseDto> CreateAsync(
            CreateOrderDetailDto dto)
        {
            if (dto.Quantity <= 0)
            {
                throw new BadRequestException(
                    "Quantity must be greater than zero.");
            }

            var product = await _productRepository.GetProductByIdAsync(dto.ProductId);

            if (product == null)
            {
                throw new NotFoundException("Product not found.");
            }

            var order = await _orderRepository.GetByIdAsync(dto.OrderId);

            if (order == null)
            {
                throw new NotFoundException("Order not found.");
            }

            var existingOrderDetail =
                await _repository.GetByIdAsync(
                    dto.OrderId,
                    dto.ProductId);

            /*
            if (existingOrderDetail != null)
            {
                throw new BadRequestException(
                    "Product already exists in this order.");
            }
            */

            var orderDetail = new OrderDetail
            {
                OrderId = dto.OrderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = product.UnitPrice ?? 0,
                Discount = 0
            };

            var createdOrderDetail =
                await _repository.CreateAsync(orderDetail);

            return _mapper.Map<OrderDetailResponseDto>(
                createdOrderDetail);
        }

        public async Task<OrderDetailResponseDto?> UpdateAsync(
            int orderId,
            int productId,
            UpdateOrderDetailDto dto)
        {
            if (dto.Quantity <= 0)
            {
                throw new BadRequestException(
                    "Quantity must be greater than zero.");
            }

            var existingOrderDetail =
                await _repository.GetByIdAsync(orderId, productId);

            if (existingOrderDetail == null)
            {
                throw new NotFoundException(
                    "Order details not found.");
            }

            existingOrderDetail.UnitPrice = dto.UnitPrice;
            existingOrderDetail.Quantity = dto.Quantity;
            existingOrderDetail.Discount = dto.Discount;

            var updatedOrderDetail =
                await _repository.UpdateAsync(existingOrderDetail);

            return _mapper.Map<OrderDetailResponseDto>(
                updatedOrderDetail);
        }

        public async Task DeleteAsync(
            int orderId,
            int productId)
        {
            var isDeleted =
                await _repository.DeleteAsync(orderId, productId);

            if (!isDeleted)
            {
                throw new NotFoundException(
                    "Order details not found.");
            }
        }
    }
}