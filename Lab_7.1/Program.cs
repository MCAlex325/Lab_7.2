using System;
using System.Collections.Generic;

// Main Application Class
public class TableReservationApp
{
    static void Main(string[] args)
    {
        ReservationManagerClass ManagerClass = new ReservationManagerClass();
        ManagerClass.AddRestaurantMethod("A", 10);
        ManagerClass.AddRestaurantMethod("B", 5);

        Console.WriteLine(ManagerClass.BookTable("A", new DateTime(2023, 12, 25), 3)); // True
        Console.WriteLine(ManagerClass.BookTable("A", new DateTime(2023, 12, 25), 3)); // False
    }
}

// Reservation Manager Class
public class ReservationManagerClass
{
    // res
    public List<RestaurantClass> result;

    public ReservationManagerClass()
    {
        result = new List<RestaurantClass>();
    }

    // Add Restaurant Method
    public void AddRestaurantMethod(string name, int tables)
    {
        try
        {
            RestaurantClass restaurant = new RestaurantClass();
            restaurant.name = name;
            restaurant.tables = new RestaurantTableClass[tables];
            for (int i = 0; i < tables; i++)
            {
                restaurant.tables[i] = new RestaurantTableClass();
            }
            result.Add(restaurant);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    // Load Restaurants From
    // File
    private void LoadRestaurantsFromFileMethod(string fileP)
    {
        try
        {
            string[] lines = File.ReadAllLines(fileP);
            foreach (string line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int tableCount))
                {
                    AddRestaurantMethod(parts[0], tableCount);
                }
                else
                {
                    Console.WriteLine(line);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    //Find All Free Tables
    public List<string> FindAllFreeTables(DateTime datetime)
    {
        try
        {
            List<string> free = new List<string>();
            foreach (var restaurant in result)
            {
                for (int i = 0; i < restaurant.tables.Length; i++)
                {
                    if (!restaurant.tables[i].IsBooked(datetime))
                    {
                        free.Add($"{restaurant.name} - Table {i + 1}");
                    }
                }
            }
            return free;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new List<string>();
        }
    }

    public bool BookTable(string restaurantName, DateTime dateTime, int tableNumber)
    {
        foreach (var restaurant in result)
        {
            if (restaurant.name == restaurantName)
            {
                if (tableNumber < 0 || tableNumber >= restaurant.tables.Length)
                {
                    throw new Exception(null); //Invalid table number
                }

                return restaurant.tables[tableNumber].Book(dateTime);
            }
        }

        throw new Exception(null); //Restaurant not found
    }

    public void SortRestaurantsByAvailabilityForUsersMethod(DateTime dateTime)
    {
        try
        {
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 0; i < result.Count - 1; i++)
                {
                    int availableCurrentTables = CountAvailableTablesForRestaurantClassAndDateTimeMethod(result[i], dateTime);
                    int availableNextTables = CountAvailableTablesForRestaurantClassAndDateTimeMethod(result[i + 1], dateTime);

                    if (availableCurrentTables < availableNextTables)
                    {
                        // Swap restaurants
                        var temp = result[i];
                        result[i] = result[i + 1];
                        result[i + 1] = temp;
                        swapped = true;
                    }
                }
            } while (swapped);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    // count available tables in a restaurant
    public int CountAvailableTablesForRestaurantClassAndDateTimeMethod(RestaurantClass restaurant, DateTime dateTime)
    {
        try
        {
            int count = 0;
            foreach (var tables in restaurant.tables)
            {
                if (!tables.IsBooked(dateTime))
                {
                    count++;
                }
            }
            return count;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return 0;
        }
    }
}

// Restaurant Class
public class RestaurantClass
{
    public string name;
    public RestaurantTableClass[] tables;
}

// Table Class
public class RestaurantTableClass
{
    private List<DateTime> bookedDates;


    public RestaurantTableClass()
    {
        bookedDates = new List<DateTime>();
    }

    // book
    public bool Book(DateTime dateTime)
    {
        try
        {
            if (bookedDates.Contains(dateTime))
            {
                return false;
            }
            //add to bd
            bookedDates.Add(dateTime);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    // is booked
    public bool IsBooked(DateTime dateTime)
    {
        return bookedDates.Contains(dateTime);
    }
}
