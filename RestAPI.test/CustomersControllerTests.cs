using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class CustomersControllerTests
{
    [Fact]
    public void Get_ShouldReturnAllCustomers()
    {
        var controller = new CustomersController();

        var result = controller.Get();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var customers = Assert.IsAssignableFrom<List<Customer>>(okResult.Value);
        customers.Should().NotBeNull();
    }

    [Fact]
    public void Post_ShouldRejectInvalidCustomers()
    {
        var controller = new CustomersController();
        var invalidCustomers = new List<Customer>
        {
            new Customer { Id = 1, FirstName = "", LastName = "Doe", Age = 25 },
            new Customer { Id = 2, FirstName = "Jane", LastName = "", Age = 25 },
            new Customer { Id = 3, FirstName = "Jim", LastName = "Beam", Age = 16 },
        };

        var result = controller.Post(invalidCustomers);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value as dynamic;

        Assert.False(response?.Success);
        Assert.NotEmpty(response?.Errors);
    }

    [Fact]
    public void Post_ShouldAddValidCustomer()
    {
        var controller = new CustomersController();
        var newCustomer = new List<Customer>
        {
            new Customer { Id = 126, FirstName = "Unit", LastName = "Test", Age = 30 }
        };

        var result = controller.Post(newCustomer);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value as dynamic;

        Assert.True(response?.Success);
        Assert.Empty(response?.Errors);
    }
}
