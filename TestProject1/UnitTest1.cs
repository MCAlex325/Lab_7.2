using System;
using Xunit;

namespace Lab7.Tests
{
    public class RestaurantTests
    {
        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenNameIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => new Restaurant("", 5));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenTableCountIsZeroOrNegative()
        {
            Assert.Throws<ArgumentException>(() => new Restaurant("Test", 0));
            Assert.Throws<ArgumentException>(() => new Restaurant("Test", -1));
        }

        [Fact]
        public void Constructor_ShouldCreateRestaurant_WhenValidInput()
        {
            var restaurant = new Restaurant("Test", 5);
            Assert.Equal("Test", restaurant.Name);
            Assert.Equal(5, restaurant.Tables.Count);
        }

        [Fact]
        public void CountAvailableTables_ShouldReturnCorrectCount()
        {
            var restaurant = new Restaurant("Test", 5);
            var date = new DateTime(2023, 12, 25);

            Assert.Equal(5, restaurant.CountAvailableTables(date));

            restaurant.BookTable(0, date);
            Assert.Equal(4, restaurant.CountAvailableTables(date));
        }

        [Fact]
        public void BookTable_ShouldReturnTrue_WhenTableIsAvailable()
        {
            var restaurant = new Restaurant("Test", 5);
            var date = new DateTime(2023, 12, 25);

            var result = restaurant.BookTable(0, date);
            Assert.True(result);
        }

        [Fact]
        public void BookTable_ShouldReturnFalse_WhenTableIsAlreadyBooked()
        {
            var restaurant = new Restaurant("Test", 5);
            var date = new DateTime(2023, 12, 25);

            restaurant.BookTable(0, date);
            var result = restaurant.BookTable(0, date);
            Assert.False(result);
        }

        [Fact]
        public void BookTable_ShouldThrowArgumentOutOfRangeException_WhenTableNumberIsInvalid()
        {
            var restaurant = new Restaurant("Test", 5);
            var date = new DateTime(2023, 12, 25);

            Assert.Throws<ArgumentOutOfRangeException>(() => restaurant.BookTable(-1, date));
            Assert.Throws<ArgumentOutOfRangeException>(() => restaurant.BookTable(5, date));
        }
        [Fact]
        public void IsBooked_ShouldReturnFalse_WhenTableIsNotBooked()
        {
            var table = new RestaurantTable();
            var date = new DateTime(2023, 12, 25);

            var isBooked = table.IsBooked(date);

            Assert.False(isBooked);
        }
    }
}
