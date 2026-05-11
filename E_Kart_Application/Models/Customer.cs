using System;
using System.Collections.Generic;

namespace E_Kart_Application.Models;

public partial class Customer
{
    public string CustomerId { get; set; } = null!;

    public string CompanyName { get; set; } = null!;

    public string? ContactName { get; set; }

    public string? ContactTitle { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Region { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public string? Phone { get; set; }

    public string? Fax { get; set; }

    public string? PasswordHash { get; set; }

<<<<<<< HEAD
<<<<<<< HEAD
    public string Role { get; set; } = null!;
=======
    public string Role { get; set; } = "Customer";
>>>>>>> origin/feature/orders
=======
    public string Role { get; set; } = null!;
>>>>>>> origin/feature/employee-categories-module

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<CustomerDemographic> CustomerTypes { get; set; } = new List<CustomerDemographic>();
}
