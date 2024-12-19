using System;
using System.Collections.Generic;
using Xunit;
using Lab7;

public class RestaurantTests
{
    [Fact]
    public void Constructor_ValidInput_CreatesRestaurant()
    {
        var restaurant = new Restaurant("Test Restaurant", 5);

        Assert.Equal("Test Restaurant", restaurant.Name);
        Assert.Equal(5, restaurant.Tables.Count);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_InvalidName_ThrowsException(string name)
    {
        Assert.Throws<ArgumentException>(() => new Restaurant(name, 5));
    }

    [Fact]
    public void Constructor_InvalidTableCount_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new Restaurant("Test", 0));
    }

    [Fact]
    public void BookTable_ValidInput_ReturnsTrue()
    {
        var restaurant = new Restaurant("Test", 5);
        var result = restaurant.BookTable(2, DateTime.Now);

        Assert.True(result);
    }

    [Fact]
    public void BookTable_InvalidTableNumber_ThrowsException()
    {
        var restaurant = new Restaurant("Test", 5);

        Assert.Throws<ArgumentOutOfRangeException>(() => restaurant.BookTable(-1, DateTime.Now));
        Assert.Throws<ArgumentOutOfRangeException>(() => restaurant.BookTable(10, DateTime.Now));
    }
}

public class RestaurantTableTests
{
    [Fact]
    public void Book_AvailableDate_ReturnsTrue()
    {
        var table = new RestaurantTable();
        var date = DateTime.Now;

        Assert.True(table.Book(date));
    }

    [Fact]
    public void Book_AlreadyBookedDate_ReturnsFalse()
    {
        var table = new RestaurantTable();
        var date = DateTime.Now;
        table.Book(date);

        Assert.False(table.Book(date));
    }

    [Fact]
    public void IsBooked_BookedDate_ReturnsTrue()
    {
        var table = new RestaurantTable();
        var date = DateTime.Now;
        table.Book(date);

        Assert.True(table.IsBooked(date));
    }

    [Fact]
    public void IsBooked_UnbookedDate_ReturnsFalse()
    {
        var table = new RestaurantTable();

        Assert.False(table.IsBooked(DateTime.Now));
    }
}

public class ReservationManagerTests
{
    [Fact]
    public void AddRestaurant_ValidInput_AddsRestaurant()
    {
        var manager = new ReservationManager();
        manager.AddRestaurant("Test Restaurant", 5);

        Assert.Single(manager.FindAllFreeTables(DateTime.Now));
    }

    [Fact]
    public void AddRestaurant_DuplicateName_ThrowsException()
    {
        var manager = new ReservationManager();
        manager.AddRestaurant("Test Restaurant", 5);

        Assert.Throws<InvalidOperationException>(() => manager.AddRestaurant("Test Restaurant", 5));
    }

    [Fact]
    public void BookTable_ValidInput_ReturnsTrue()
    {
        var manager = new ReservationManager();
        manager.AddRestaurant("Test Restaurant", 5);

        Assert.True(manager.BookTable("Test Restaurant", 2, DateTime.Now));
    }

    [Fact]
    public void BookTable_InvalidRestaurant_ThrowsException()
    {
        var manager = new ReservationManager();

        Assert.Throws<KeyNotFoundException>(() => manager.BookTable("Invalid Restaurant", 2, DateTime.Now));
    }

    [Fact]
    public void FindAllFreeTables_ReturnsCorrectFreeTables()
    {
        var manager = new ReservationManager();
        manager.AddRestaurant("Test Restaurant", 3);
        manager.BookTable("Test Restaurant", 1, DateTime.Now);

        var freeTables = manager.FindAllFreeTables(DateTime.Now);

        Assert.Equal(2, freeTables.Count);
    }
}
