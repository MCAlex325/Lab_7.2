using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab7
{
    public interface IRestaurant
    {
        string Name { get; }
        List<ITable> Tables { get; }
        int CountAvailableTables(DateTime dateTime);
        bool BookTable(int tableNumber, DateTime dateTime);
    }

    public interface ITable
    {
        bool IsBooked(DateTime dateTime);
        bool Book(DateTime dateTime);
    }

    public class Restaurant : IRestaurant
    {
        public string Name { get; private set; }
        public List<ITable> Tables { get; private set; }

        public Restaurant(string name, int tableCount)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Restaurant name cannot be empty.");

            if (tableCount <= 0)
                throw new ArgumentException("Table count must be greater than 0.");

            Name = name;
            Tables = Enumerable.Range(0, tableCount).Select(_ => (ITable)new RestaurantTable()).ToList();
        }

        public int CountAvailableTables(DateTime dateTime)
        {
            return Tables.Count(t => !t.IsBooked(dateTime));
        }

        public bool BookTable(int tableNumber, DateTime dateTime)
        {
            if (tableNumber < 0 || tableNumber >= Tables.Count)
                throw new ArgumentOutOfRangeException(nameof(tableNumber), "Invalid table number.");

            return Tables[tableNumber].Book(dateTime);
        }
    }

    public class RestaurantTable : ITable
    {
        private readonly HashSet<DateTime> _bookedDates;

        public RestaurantTable()
        {
            _bookedDates = new HashSet<DateTime>();
        }

        public bool IsBooked(DateTime dateTime)
        {
            return _bookedDates.Contains(dateTime);
        }

        public bool Book(DateTime dateTime)
        {
            if (IsBooked(dateTime)) return false;

            _bookedDates.Add(dateTime);
            return true;
        }
    }

    public class ReservationManager
    {
        private readonly List<IRestaurant> _restaurants;

        public ReservationManager()
        {
            _restaurants = new List<IRestaurant>();
        }

        public void AddRestaurant(string name, int tables)
        {
            if (_restaurants.Any(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A restaurant with the name '{name}' already exists.");
            }

            _restaurants.Add(new Restaurant(name, tables));
        }

        public bool BookTable(string restaurantName, int tableNumber, DateTime dateTime)
        {
            var restaurant = _restaurants.FirstOrDefault(r => r.Name.Equals(restaurantName, StringComparison.OrdinalIgnoreCase));

            if (restaurant == null)
                throw new KeyNotFoundException($"Restaurant '{restaurantName}' not found.");

            return restaurant.BookTable(tableNumber, dateTime);
        }

        public List<string> FindAllFreeTables(DateTime dateTime)
        {
            return _restaurants.SelectMany(r => r.Tables
                .Select((t, index) => new { r.Name, TableIndex = index, IsFree = !t.IsBooked(dateTime) })
                .Where(t => t.IsFree)
                .Select(t => $"{t.Name} - Table {t.TableIndex + 1}"))
                .ToList();
        }

        public void SortRestaurantsByAvailability(DateTime dateTime)
        {
            _restaurants.Sort((r1, r2) => r2.CountAvailableTables(dateTime).CompareTo(r1.CountAvailableTables(dateTime)));
        }

        public void LoadRestaurantsFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"The file '{filePath}' does not exist.");

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int tableCount))
                {
                    AddRestaurant(parts[0], tableCount);
                }
                else
                {
                    Console.WriteLine($"Invalid line format: {line}");
                }
            }
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var manager = new ReservationManager();
            manager.AddRestaurant("Mak", 10);
            manager.AddRestaurant("Kfc", 5);

            Console.WriteLine(manager.BookTable("Mak", 9, new DateTime(2023, 12, 25)));

        }
    }
}
