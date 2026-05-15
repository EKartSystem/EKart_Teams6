using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using E_Kart_Application.DTOs;
using E_Kart_Application.DTOs.Customersdto;
using E_Kart_Application.DTOs.Orders;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;

namespace E_Kart_Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerDto>> GetCustomersAsync()
        {
            var data = await _repository.GetCustomersAsync();
            if (data == null)
            {
                throw new NotFoundException("No customers found.");
            }
            return _mapper.Map<IEnumerable<CustomerDto>>(data);
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(string id)
        {
            var data = await _repository.GetCustomerByIdAsync(id);
            if (data == null)
            {
                throw new NotFoundException($"Customer with id {id} not found.");
            }
            return _mapper.Map<CustomerDto>(data);
        }

        public async Task<IEnumerable<OrderDto>> GetCustomerOrdersAsync(string id)
        {

            var data = await _repository.GetCustomerOrdersAsync(id);
            if (data == null)
            {
                throw new NotFoundException($"Customer with id {id} not found.");
            }
            return _mapper.Map<IEnumerable<OrderDto>>(data);
        }

        public async Task<IEnumerable<CustomerDto>> SearchCustomersAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new BadRequestException("Name cannot be empty.");
            }
            var data = await _repository.SearchCustomersAsync(name);
            if (data == null)
            {
                throw new NotFoundException($"No customers found with name containing '{name}'.");
            }
            return _mapper.Map<IEnumerable<CustomerDto>>(data);
        }

        public async Task<IEnumerable<CustomerDto>> GetCustomersByCountryAsync(string country)
        {
            var data = await _repository.GetCustomersByCountryAsync(country);
            if (data == null)
            {
                throw new NotFoundException($"No customers found in country '{country}'.");
            }
            return _mapper.Map<IEnumerable<CustomerDto>>(data);
        }

        public async Task<IEnumerable<CustomerDto>> GetTopCustomersAsync()
        {
            var data = await _repository.GetTopCustomersAsync();
            if (data == null)
            {
                throw new NotFoundException("No customers found.");
            }
            return _mapper.Map<IEnumerable<CustomerDto>>(data);
        }

        public async Task<CustomerDto> RegisterCustomerAsync(RegisterCustomerDto dto)
        {
            var customer = _mapper.Map<Customer>(dto);
            customer.CustomerId = GenerateCustomerId(dto.CompanyName);
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

            customer.PasswordHash = BitConverter.ToString(bytes).Replace("-", "");
            customer.Role = "Customer";


            var data = await _repository.RegisterCustomerAsync(customer);
            return _mapper.Map<CustomerDto>(data);
        }
        public async Task<CustomerDto?> LoginAsync(CustomerLogin dto)
        {
            var role = dto.Role ?? "Customer";

            var data = await _repository.LoginAsync(dto.ContactName, dto.Password, role);
            if (data == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }
            return _mapper.Map<CustomerDto>(data);
        }

        public async Task<bool> UpdateCustomerAsync(string id, UpdateCustomerDto dto)
        {
            var data = await _repository.GetCustomerByIdAsync(id);
            if (data == null)
            {
                throw new NotFoundException($"No Customer found with id {id}");

            }
            _mapper.Map(dto, data);
            return await _repository.UpdateCustomerAsync(id, data);

        }

        public async Task<bool> UpdateAddressAsync(string id, string address)
        {
            var data = await _repository.GetCustomerByIdAsync(id);
            if (data == null)
            {
                throw new NotFoundException($"Customer with id {id} not found.");
            }
            return await _repository.UpdateAddressAsync(id, address);

        }

        public async Task<bool> UpdateContactAsync(string id, string contactName)
        {
            var data = await _repository.GetCustomerByIdAsync(id);
            if (data == null)
            {
                throw new NotFoundException($"Customer with id {id} not found.");
            }
            return await _repository.UpdateContactAsync(id, contactName);
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