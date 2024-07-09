﻿using Cart.Products;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace Cart;

internal class Program
{
    internal static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        // Задание 2. Считывание товаров из файла.
        //Store.GenerateProducts();
        Store.ReadProductsFromFile();

        // Задание 4. Генератор тестовых заказов.
        //OrdersGenerator.GenerateRandomOrders();
        OrdersGenerator.ReadOrdersFromFile();

        // Задание 1. Ввод заказа из консоли с учётом пожеланий пользователя.
        //Store.PrintProductsTypesInfo();
        Order orderFromConsole = new Order();
        //orderFromConsole.ReadOrderFromConsole();
        //orderFromConsole.WriteOrderToFile();

        //Задание 7. Ввод заказа из файла.
        Order orderFromFile= new();
        orderFromFile = orderFromFile.ReadOrderFromFile();
        Dictionary<Product, uint> orderFromFileDictionary = orderFromFile.Products.ToDictionary();
        //orderFromFile.PrintOrderInfo();
        foreach(KeyValuePair<Product, uint> orderItem in orderFromFileDictionary)
        {
            Console.WriteLine("Состав заказа\n");
            PropertyInfo[] propertyInfo = orderItem.Key.GetType().GetProperties();
            foreach (PropertyInfo property in propertyInfo)
            {
                Console.WriteLine($"{property.Name}, {property.GetValue(orderItem.Key)?.ToString()}");
            }
            Console.WriteLine();
        }

        // Задание 3. Калькулятор заказов.
        CartCalculator cartCalculator = new(new Calculator.Logger());
        Order orderA = new();
        orderA = cartCalculator.Add(Store.Products[0], Store.Products[1]);
        orderA = cartCalculator.Add(Store.Products[0], Store.Products[0]);
        orderA = cartCalculator.Add(orderA, Store.Products[1]);
        orderA = cartCalculator.Add(orderA, Store.Products[1]);
        Order orderB = new();
        orderB = cartCalculator.Add(orderB, Store.Products[1]);
        Order orderC = cartCalculator.Add(orderA, orderB);
        orderC = cartCalculator.Multiply(orderC, 2);

        // Задание 5. Работа с LINQ.
        List<Order> validOrders = new();
        // Заказы, дешевле заданной суммы (15000.00).
        validOrders = OrdersGenerator.Orders.Where(order => order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) < 15000.00M).ToList();
        // Заказы, дороже заданной суммы (15000.00).
        validOrders = OrdersGenerator.Orders.Where(order => order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) > 15000.00M).ToList();
        // Заказы, имеющие в составе товары определённого типа (корвалол).
        validOrders = OrdersGenerator.Orders.Where(order => order.Products.Any(orderItem => orderItem.Key.GetType() == typeof(Corvalol))).ToList();
        // Заказы, отсортированные по весу в порядке возрастания.
        validOrders = OrdersGenerator.Orders.OrderBy(order => order.Products.Sum(orderItem => orderItem.Key.Weight)).ToList();
        // Заказы, с уникальными названиями (заказы, в которых количество каждого товара не превышает единицы).
        //validOrders = orders.Select(order => order.DistinctBy(orderItem => orderItem.Key.Name).ToDictionary()).ToList();
        validOrders = OrdersGenerator.Orders.Where(order => order.Products.All(orderItem => orderItem.Value == 1)).ToList();
        // Заказы, отправленные до указанной даты.
        DateTime maxDepartureDateTime = DateTime.Now.AddDays(1);
        validOrders = OrdersGenerator.Orders.Where(order => order.TimeOfDeparture <= maxDepartureDateTime).ToList();
    }
}