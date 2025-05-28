using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Text.Json;

public class CustomersControllerTests : IDisposable
{
    private readonly CustomersController _controller;
    private static int _currentTestId = 1000; // Start with a high number to avoid conflicts
    private const string TestDataFile = "customers.json";

    public CustomersControllerTests()
    {
        // Clean up any existing test data
        CleanupTestData();
        _controller = new CustomersController();
    }

    private void CleanupTestData()
    {
        // Create empty customers.json file
        File.WriteAllText(TestDataFile, "[]");
    }

    private int GetUniqueId()
    {
        var id = Interlocked.Increment(ref _currentTestId);
        return id == 0 ? GetUniqueId() : id; // Ensure we never return 0
    }

    [Fact]
    public void Get_ShouldReturnAllCustomers()
    {
        // Act
        var result = _controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var customers = Assert.IsAssignableFrom<List<Customer>>(okResult.Value);
        customers.Should().NotBeNull();
    }

    [Fact]
    public void Post_ShouldRejectInvalidCustomers()
    {
        // Arrange
        var invalidCustomers = new List<Customer>
        {
            new Customer { Id = GetUniqueId(), FirstName = "", LastName = "Doe", Age = 25 },
            new Customer { Id = GetUniqueId(), FirstName = "Jane", LastName = "", Age = 25 },
            new Customer { Id = GetUniqueId(), FirstName = "Jim", LastName = "Beam", Age = 16 },
        };

        // Act
        var result = _controller.Post(invalidCustomers);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<CustomerResponse>(okResult.Value);
        
        Assert.False(response.Success);
        Assert.NotEmpty(response.Errors);
    }

    [Fact]
    public void Post_ShouldAddValidCustomer()
    {
        // Arrange
        var id = GetUniqueId();
        var newCustomer = new List<Customer>
        {
            new Customer 
            { 
                Id = id,
                FirstName = "Unit", 
                LastName = "Test", 
                Age = 30 
            }
        };

        // Act
        var result = _controller.Post(newCustomer);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<CustomerResponse>(okResult.Value);
        
        Assert.True(response.Success, $"Failed to add customer. ID: {id}, Errors: {string.Join(", ", response.Errors)}");
        Assert.Empty(response.Errors);

        // Verify the customer was actually added
        var getResult = _controller.Get();
        var getOkResult = Assert.IsType<OkObjectResult>(getResult);
        var customers = Assert.IsAssignableFrom<List<Customer>>(getOkResult.Value);
        customers.Should().ContainSingle(c => c.Id == id);
    }

    [Fact]
    public void Post_ShouldRejectDuplicateId()
    {
        // Arrange
        var id = GetUniqueId();
        var firstCustomer = new List<Customer>
        {
            new Customer 
            { 
                Id = id,
                FirstName = "First", 
                LastName = "Customer", 
                Age = 30 
            }
        };

        var duplicateCustomer = new List<Customer>
        {
            new Customer 
            { 
                Id = id, // Same ID as first customer
                FirstName = "Second", 
                LastName = "Customer", 
                Age = 30 
            }
        };

        // Act
        var firstResult = _controller.Post(firstCustomer);
        var duplicateResult = _controller.Post(duplicateCustomer);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(duplicateResult);
        var response = Assert.IsType<CustomerResponse>(okResult.Value);
        
        Assert.False(response.Success);
        Assert.Contains(response.Errors, error => error.Contains("already exists"));
    }

    public void Dispose()
    {
        // Clean up test data after each test
        CleanupTestData();
    }
}
