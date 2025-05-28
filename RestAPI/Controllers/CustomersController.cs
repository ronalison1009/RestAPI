using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private static readonly string DataFile = "customers.json";
    private static readonly List<Customer> Customers;

    static CustomersController()
    {
        if (System.IO.File.Exists(DataFile))
        {
            var json = System.IO.File.ReadAllText(DataFile);
            Customers = JsonSerializer.Deserialize<List<Customer>>(json) ?? new List<Customer>();
        }
        else
        {
            Customers = new List<Customer>();
        }
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(Customers);
    }

    [HttpPost]
    public IActionResult Post([FromBody] List<Customer> newCustomers)
    {
        var errors = new List<string>();

        foreach (var customer in newCustomers)
        {
            if (string.IsNullOrWhiteSpace(customer.FirstName) ||
                string.IsNullOrWhiteSpace(customer.LastName) ||
                customer.Id == 0 || customer.Age == 0)
            {
                errors.Add($"Customer with ID {customer.Id} is missing required fields.");
                continue;
            }

            if (customer.Age <= 18)
            {
                errors.Add($"Customer with ID {customer.Id} must be older than 18.");
                continue;
            }

            if (Customers.Any(c => c.Id == customer.Id))
            {
                errors.Add($"Customer ID {customer.Id} already exists.");
                continue;
            }

            InsertSorted(customer);
        }

        SaveToFile();
        return Ok(new CustomerResponse { Success = errors.Count == 0, Errors = errors });
    }

    private static void InsertSorted(Customer customer)
    {
        int index = 0;
        while (index < Customers.Count)
        {
            var existing = Customers[index];
            if (string.Compare(customer.LastName, existing.LastName) < 0 ||
                (customer.LastName == existing.LastName &&
                 string.Compare(customer.FirstName, existing.FirstName) < 0))
            {
                break;
            }
            index++;
        }
        Customers.Insert(index, customer);
    }

    private static void SaveToFile()
    {
        var json = JsonSerializer.Serialize(Customers, new JsonSerializerOptions { WriteIndented = true });
        System.IO.File.WriteAllText(DataFile, json);
    }
}