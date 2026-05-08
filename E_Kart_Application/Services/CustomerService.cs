using AutoMapper;
using E_Kart_Application.DTOs;
using E_Kart_Application.DTOs.Customersdto;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;

namespace E_Kart_Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        private readonly IMapper _mapper;

        public CustomerService(
            ICustomerRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerDto>> GetCustomersAsync()
        {
            var data = await _repository.GetCustomersAsync();
            return _mapper.Map<IEnumerable<CustomerDto>>(data);
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(string id)
        {
            var data = await _repository.GetCustomerByIdAsync(id);
            return _mapper.Map<CustomerDto>(data);
        }

        public async Task<IEnumerable<OrderDto>> GetCustomerOrdersAsync(string id)
        {
            var data = await _repository.GetCustomerOrdersAsync(id);
            return _mapper.Map<IEnumerable<OrderDto>>(data);
        }

        public async Task<IEnumerable<CustomerDto>> SearchCustomersAsync(string name)
        {
            var data = await _repository.SearchCustomersAsync(name);
            return _mapper.Map<IEnumerable<CustomerDto>>(data);
        }

        public async Task<IEnumerable<CustomerDto>> GetCustomersByCountryAsync(string country)
        {
            var data = await _repository.GetCustomersByCountryAsync(country);
            return _mapper.Map<IEnumerable<CustomerDto>>(data);
        }

        public async Task<IEnumerable<CustomerDto>> GetTopCustomersAsync()
        {
            var data = await _repository.GetTopCustomersAsync();
            return _mapper.Map<IEnumerable<CustomerDto>>(data);
        }

        public async Task<CustomerDto> RegisterCustomerAsync(RegisterCustomerDto dto)
        {
            var customer = _mapper.Map<Customer>(dto);

            customer.CustomerId = GenerateCustomerId(dto.CompanyName);

            customer.PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(dto.Password);

            customer.Role = "Customer";

            var data = await _repository.RegisterCustomerAsync(customer);

            return _mapper.Map<CustomerDto>(data);
        }

        public async Task<CustomerDto?> LoginAsync(CustomerLogin dto)
        {
            var data = await _repository.LoginAsync(dto.ContactName, dto.Password, dto.Role);
            return _mapper.Map<CustomerDto>(data);
        }

        public async Task<bool> UpdateCustomerAsync(string id, UpdateCustomerDto dto)
        {
            var customer = new Customer
            {
                ContactName = dto.ContactName,
                Phone = dto.Phone,
                Address = dto.Address
            };
            return await _repository.UpdateCustomerAsync(id, customer);
        }

        public async Task<bool> UpdateAddressAsync(string id, UpdateAddressDto dto)
        {
            return await _repository.UpdateAddressAsync(id, dto);
        }

        public async Task<bool> UpdateContactAsync(string id, UpdateContactDto dto)
        {
            return await _repository.UpdateContactAsync(id, dto);
        }
        public string GenerateCustomerId(string companyName)
        {
            var lettersOnly = new string(companyName
                .Where(char.IsLetter)
                .ToArray());

            return lettersOnly
                .ToUpper()
                .PadRight(5, 'X')
                .Substring(0, 5);
        }
    }
}