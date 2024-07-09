﻿using Cart.Products;
using Newtonsoft.Json;
using System.Reflection;

namespace Cart;

/// <summary>
/// Класс магазина. Может генерировать продукты или считывать список продуктов из файла. Хранит список продуктов. 
/// </summary>
public static class Store
{
    /// <summary>
    /// Список продуктов в магазине.
    /// </summary>
    public static List<Product> Products = new();

    /// <summary>
    /// Настройка сеариализации.
    /// </summary>
    private static JsonSerializerSettings jsonSerializerSettings = new()
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.Auto,
        NullValueHandling = NullValueHandling.Include
    };

    /// <summary>
    /// Типы товаров магазина.
    /// </summary>
    public static List<Type> ProductsTypes = new();

    /// <summary>
    /// Сгенерировать продукты, доступные в магазине.
    /// </summary>
    public static void GenerateProducts()
    {
        uint productId = 0;
        Random random = new();

        for (int i = 0; i < 5; i++)
        {
            Corvalol corvarol = new(
                id: productId += 1,
                name: "Корвалол-" + productId.ToString(),
                weight: GetWeight(productId),
                price: GetPrice(productId)
                );
            WashingMachine washingMachine = new(
                id: productId += 1,
                name: "Стиральная машина-" + productId.ToString(),
                weight: GetWeight(productId),
                price: GetPrice(productId),
                isDryerIncluded: random.Next(0, 2) == 0 ? false : true
                );
            Chips chips = new(
                id: productId += 1,
                name: "Чипсы-" + productId.ToString(),
                weight: GetWeight(productId),
                price: GetPrice(productId)
                );

            Products.Add(corvarol);
            Products.Add(washingMachine);
            Products.Add(chips);
        }

        while(true)
        { 
            Console.WriteLine("Сохранить список в файл? Введите y или n.");
            switch(Console.ReadLine())
            {
                case "y":
                    string jsonString = JsonConvert.SerializeObject(Products, jsonSerializerSettings);
                    string fileName = "Products.json";
                    string filePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                    File.AppendAllText(filePath + Path.DirectorySeparatorChar + fileName, jsonString);
                    break;
                case "n":
                    break;
                default:
                    Console.WriteLine("Такой команды нет.");
                    continue;
            }

            Console.WriteLine("Вывести список товаров на экран? Введите y или n.");
            switch (Console.ReadLine())
            {
                case "y":
                    PrintProductsInfo();
                    break;
                case "n":
                    break;
                default:
                    Console.WriteLine("Такой команды нет.");
                    continue;
            }
            break;
        }
    }

    /// <summary>
    /// Считать из файла список товаров магазина.
    /// </summary>
    public static void ReadProductsFromFile()
    {
        string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        string fileNameProducts = "Products.json";
        string jsonProductsList = File.ReadAllText(projectPath + Path.DirectorySeparatorChar + fileNameProducts);
        Products = JsonConvert.DeserializeObject<List<Product>>(jsonProductsList, jsonSerializerSettings);
    }

    private static double GetWeight(uint ProductId)
    {
        Random random = new();
        return Double.Round(random.NextDouble() * (30 - 15) + 15, 2);
    }

    private static decimal GetPrice(uint ProductId)
    {
        Random random = new();
        
        return Decimal.Round((decimal)(random.NextDouble() * (1000 - 500) + 500), 2);
    }

    /// <summary>
    /// Вывод товаров магазина.
    /// </summary>
    public static void PrintProductsInfo()
    {
        foreach (Product product in Products)
        {
            PropertyInfo[] propertyInfo = product.GetType().GetProperties();
            foreach (PropertyInfo property in propertyInfo)
            {
                Console.WriteLine($"{property.Name}, {property.GetValue(product)?.ToString()}");
            }
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Вывод типов товаров магазина.
    /// </summary>
    public static void PrintProductsTypesInfo()
    {
        Console.WriteLine("Типы товаров в магазине.");
        ProductsTypes = typeof(Product).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Product))).ToList();
        uint index = 0;
        foreach (Type productType in ProductsTypes)
        {
            Console.WriteLine($"{index += 1}) {productType}");
        }
    }
}