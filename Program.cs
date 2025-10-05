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
        public string ContactNumber { get; set; }

        public string GetSupplierInfo()
        {
            return $"Поставщик: {SupplierCompany}\n" + $"Договор: {ContactNumber}\n" + $"Информация о топливе: {BasePrice.GetInfo()}";
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
                fuels = ProcessManualInput();
            }
            else if (choice == 2)
            {
                fuels = ProcessFileInput();
            }

            if (fuels.Length > 0)
            {
                ProcessSupplierData(fuels);
                ProcessSalesStatistics(fuels);
            }

        }


        private static FuelPrice[] ProcessManualInput()
        {
            Console.WriteLine("Сколько типов топлива вы хотите ввести?");
            int count = Convert.ToInt32(Console.ReadLine());

            FuelPrice[] fuels = new FuelPrice[count];
            int currentIndex = 0;


            while (count > 0)
            {
                count--;

                Console.WriteLine("\nВведите свойства топлива в формате: вид топлива, гггг.мм.дд, цена");
                string input = Console.ReadLine();
                
                FuelPrice price = ParseFuelFromString(input);

                Console.WriteLine("\nИНФОРМАЦИЯ О ТОПЛИВЕ");
                string result = price.GetInfo();

                fuels[currentIndex] = price;
                currentIndex++;
            }
            return fuels;
        }

        private static FuelPrice ParseFuelFromString(string input)
        {
            string[] parts = SplitAndTrimInput(input, ',');

            string[] dateParts = SplitAndTrimInput(parts[1], '.');

            return new FuelPrice
            {
                Type = parts[0],
                Date = new DateTime(
                    int.Parse(dateParts[0]),
                    int.Parse(dateParts[1]),
                    int.Parse(dateParts[2])
                ),
                Cost = double.Parse(parts[2])
            };
        }

        private static string[] SplitAndTrimInput(string input, char separator)
        {
            string[] parts = input.Split(separator);
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = parts[i].Trim();
            }
            return parts;
        }

        private static FuelPrice[] ProcessFileInput()
        {
            Console.WriteLine("Введите путь к файлу");
            string fileName = Console.ReadLine();

            if (!File.Exists(fileName))
            {
                Console.WriteLine("Файл не найден");
                return Array.Empty<FuelPrice>();
            }

            string[] lines = File.ReadAllLines(fileName);
            FuelPrice[] fuels = new FuelPrice[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                fuels[i] = ParseFuelFromString(lines[i]);
                Console.WriteLine(fuels[i].GetInfo());
            }

            return fuels;
        }

        private static void ProcessSupplierData(FuelPrice[] fuels)
        {
            Console.WriteLine("\nВВОД ДАННЫХ О ПОСТАВЩИКЕ");
            int fuelIndex = GetFuelIndexFromUser(fuels, "поставщика");

            Console.WriteLine("Введите название компании-поставщика:");
            string supplier = Console.ReadLine();

            Console.WriteLine("Введите номер договора:");
            string contract = Console.ReadLine();

            FuelPriceWithSupplier supplierInfo = new FuelPriceWithSupplier()
            {
                BasePrice = fuels[fuelIndex],
                SupplierCompany = supplier,
                ContactNumber = contract,
            };

            Console.WriteLine("\nИНФОРМАЦИЯ О ПОСТАВЩИКЕ");
            Console.WriteLine(supplierInfo.GetSupplierInfo());
        }

        private static void ProcessSalesStatistics(FuelPrice[] fuels)
        {
            Console.WriteLine("\nВВОД СТАТИСТИКИ ПРОДАЖ");
            int fuelIndex = GetFuelIndexFromUser(fuels, "статистики");

            Console.WriteLine("Введите количество проданных литров:");
            double liters = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Введите общую выручку:");
            double revenue = Convert.ToDouble(Console.ReadLine());

            FuelSalesStats salesStats = new FuelSalesStats
            {
                FuelType = fuels[fuelIndex],
                TotalLitersSold = liters,
                TotalRevenue = revenue
            };

            Console.WriteLine("\nСТАТИСТИКА ПРОДАЖ");
            Console.WriteLine(salesStats.GetStatsInfo());
        }

        private static int GetFuelIndexFromUser(FuelPrice[] fuels, string purpose)
        {
            Console.WriteLine("Введите номер типа топлива для {0} (от 1 до {1}):", purpose, fuels.Length);
            return Convert.ToInt32(Console.ReadLine()) - 1;
        }
    }
}
