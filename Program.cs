using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    class GasStation
    {
        public string Name { get; set; }
        public FuelPrice NowCost { get; set; }
    }

    public class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Сколько типов топлива вы хотите ввести?");

            int count = Convert.ToInt32(Console.ReadLine());

            FuelPrice[] fuels = new FuelPrice[count];


            while (count > 0)
            {
                count--;

                Console.WriteLine("Введите свойства топлива в формате: вид топлива, гггг.мм.дд, цена");

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

                Console.WriteLine("Результат:");
                string result = price.GetInfo();
                Console.WriteLine(result);

                
                fuels.Append(price);

            }

        }
    }
}
