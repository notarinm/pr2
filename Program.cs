using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace pis1
{
    class FuelPrice
    {
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public double Cost { get; set; }

        public string GetInfo()
        {
            return $"Топливо: {Type}; Дата: {Date:yyyy.MM.dd}; Цена: {Cost} руб.";
        }
    }

    class FuelPriceWithSupplier
    {
        public FuelPrice BasePrice { get; set; }
        public string SupplierCompany {  get; set; }
        public string ContactNomber { get; set; }

        public string GetSupplierInfo()
        {
            return $"Поставщик: {SupplierCompany}\n" + $"Договор: {ContactNomber}\n" + $"Информация о топливе: {BasePrice.GetInfo()}";
        }
    }

    class FuelSalesStats
    {
        public FuelPrice FuelType { get; set; }
        public double TotalLitersSold { get; set; }
        public double TotalRevenue {  get; set; }

        public string GetStatsInfo()
        {
            return $"Статистика продаж: {FuelType.Type}\n" + $"Продано: {TotalLitersSold} л\n" + $"Выручка: {TotalRevenue} руб.\n" + $"Текущая цена: {FuelType.Cost} руб./л";
        }
    }

    class GasStation
    {
        public string Name { get; set; }
        public FuelPrice NowCost { get; set; }
    }

    public class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("1 - Ручной ввод\n2 - Загрузка из файла");
            int choice = Convert.ToInt32(Console.ReadLine());

            FuelPrice[] fuels = Array.Empty<FuelPrice>();

            if (choice == 1)
            {

                Console.WriteLine("Сколько типов топлива вы хотите ввести?");
                int count = Convert.ToInt32(Console.ReadLine());

                fuels = new FuelPrice[count];
                int currentIndex = 0;


                while (count > 0)
                {
                    count--;

                    Console.WriteLine("\nВведите свойства топлива в формате: вид топлива, гггг.мм.дд, цена");
                    string input = Console.ReadLine();
                    string[] parts = input.Split(',');

                    for (int i = 0; i < parts.Length; i++)
                    {
                        parts[i] = parts[i].Trim();
                    }

                    FuelPrice price = new FuelPrice();

                    price.Type = parts[0];


                    string[] dateParts = parts[1].Split('.');
                    price.Date = new DateTime(
                        int.Parse(dateParts[0]),
                        int.Parse(dateParts[1]),
                        int.Parse(dateParts[2])
                    );

                    price.Cost = double.Parse(parts[2]);

                    Console.WriteLine("\nИНФОРМАЦИЯ О ТОПЛИВЕ");
                    string result = price.GetInfo();
                    Console.WriteLine(result);


                    fuels[currentIndex] = price;
                    currentIndex++;

                }
            }

            else if (choice == 2)
            {
                Console.WriteLine("Введите путь к файлу");
                string fileName = Console.ReadLine();

                if (File.Exists(fileName))
                {
                    string[] lines = File.ReadAllLines(fileName);
                    fuels = new FuelPrice[lines.Length];

                    for (int i = 0;i < lines.Length;i++)
                    {
                        string[] parts = lines[i].Split(',');
                        for (int j = 0; j < parts.Length; j++)
                        {
                            parts[j] = parts[j].Trim();
                        }

                        string[] dateParts = parts[1].Split('.');
                        fuels[i] = new FuelPrice
                        {
                            Type = parts[0],
                            Date = new DateTime(
                                int.Parse(dateParts[0]),
                                int.Parse(dateParts[1]),
                                int.Parse(dateParts[2])
                            ),
                            Cost = double.Parse(parts[2])
                        };
                        Console.WriteLine(fuels[i].GetInfo());
                    }

                }
                else
                {
                    Console.WriteLine("Файл не найден");
                    return;
                }
            }

            if (fuels.Length > 0)
            {

                Console.WriteLine("\nВВОД ДАННЫХ О ПОСТАВЩИКЕ");
                Console.WriteLine("Введите номер типа топлива для поставщика (от 1 до {0}):", fuels.Length);
                int fuelIndex1 = Convert.ToInt32(Console.ReadLine()) - 1;

                Console.WriteLine("Введите название компании-поставщика:");
                string supplier = Console.ReadLine();

                Console.WriteLine("Введите номер договора:");
                string contract = Console.ReadLine();

                FuelPriceWithSupplier supplierInfo = new FuelPriceWithSupplier()
                {
                    BasePrice = fuels[fuelIndex1],
                    SupplierCompany = supplier,
                    ContactNomber = contract,
                };

                Console.WriteLine("\nИНФОРМАЦИЯ О ПОСТАВЩИКЕ");
                Console.WriteLine(supplierInfo.GetSupplierInfo());

                Console.WriteLine("\nВВОД СТАТИСТИКИ ПРОДАЖ");
                Console.WriteLine("Введите номер типа топлива для статистики (от 1 до {0}):", fuels.Length);
                int fuelIndex2 = Convert.ToInt32(Console.ReadLine()) - 1;

                Console.WriteLine("Введите количество проданных литров:");
                double liters = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Введите общую выручку:");
                double revenue = Convert.ToDouble(Console.ReadLine());

                FuelSalesStats salesStats = new FuelSalesStats
                {
                    FuelType = fuels[fuelIndex2],
                    TotalLitersSold = liters,
                    TotalRevenue = revenue
                };

                Console.WriteLine("\nСТАТИСТИКА ПРОДАЖ");
                Console.WriteLine(salesStats.GetStatsInfo());

            }

        }
    }
}
