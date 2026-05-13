using System.Security.Cryptography;
using System.Text;
using E_Kart_Application.DBContext;
using E_Kart_Application.DTOs.Customersdto;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Models;
using Microsoft.EntityFrameworkCore;
using EKARTContext = E_Kart_Application.DBContext.EKARTContext;


namespace E_Kart_Application.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly EKARTContext _context;
        public CustomerRepository(EKARTContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await _context.Customers.Include(x => x.Orders).ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(string id)
        {
            return await _context.Customers.Include(x => x.Orders).FirstOrDefaultAsync(x => x.CustomerId == id);
        }

        public async Task<IEnumerable<Order>> GetCustomerOrdersAsync(string id)
        {
            return await _context.Orders.Where(x => x.CustomerId == id).ToListAsync();
        }

        public async Task<IEnumerable<Customer>> SearchCustomersAsync(string name)
        {
            return await _context.Customers.Include(x => x.Orders).Where(x => x.ContactName!.Contains(name)).ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetCustomersByCountryAsync(string country)
        {
            return await _context.Customers.Include(x => x.Orders).Where(x => x.Country == country).ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetTopCustomersAsync()
        {
            return await _context.Customers.Include(x => x.Orders).OrderByDescending(x => x.Orders.Count).Take(5).ToListAsync();
        }

        public async Task<Customer> RegisterCustomerAsync(Customer customer)
        {
            var exists = await _context.Customers.AnyAsync(x => x.ContactName == customer.ContactName);
            if (exists)
            {
                throw new BadRequestException("Customer already exists");
            }
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> LoginAsync(string contactName, string password, string role)
        {
            string hashedPassword;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                hashedPassword = BitConverter.ToString(bytes).Replace("-", "");
            }
            return await _context.Customers.FirstOrDefaultAsync(x => x.ContactName == contactName && x.PasswordHash == hashedPassword && x.Role.ToLower() == role.ToLower());
        }
        
        

        public async Task<bool> UpdateCustomerAsync(string id, Customer customer)
        {
            var data = await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == id);
            if (data == null)
            {
                return false;
            }
            data.ContactName = customer.ContactName;
            data.Phone = customer.Phone;
            data.Address = customer.Address;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAddressAsync(string id, string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return false;
            var data = await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == id);

            if (data == null)
            {
                return false;
            }

            data.Address = address;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateContactAsync(string id, string contactName)
        {
            if (string.IsNullOrWhiteSpace(contactName))
                return false;
            var data = await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == id);
            if (data == null)
            {
                return false;
            }
            data.ContactName = contactName;
            await _context.SaveChangesAsync();
            return true;
        }
    }



}