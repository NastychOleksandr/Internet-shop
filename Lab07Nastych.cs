using System;
using System.Text;

namespace OnlineStore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            bool running = true;

            while (running)
            {
                Console.WriteLine("=== Інтернет-магазин ===");
                Console.WriteLine("1 — довідка про предметну область");
                Console.WriteLine("2 — табулювання вартості замовлення зі знижкою");
                Console.WriteLine("3 — зведена таблиця продажів за категоріями та днями");
                Console.WriteLine("0 — вихід");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ShowAbout();
                        Pause();
                        break;

                    case "2":
                        ShowDiscountTable();
                        Pause();
                        break;

                    case "3":
                        ShowSalesMatrix();
                        Pause();
                        break;

                    case "0":
                        running = false;
                        Console.WriteLine("Роботу завершено. До побачення!");
                        break;

                    default:
                        Console.WriteLine("Немає такого пункту меню. Спробуйте ще раз.");
                        Pause();
                        break;
                }
            }
        }

        static void Pause()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу для повернення в меню...");
            Console.ReadKey(true);
            Console.Clear();
        }

        static void ShowAbout()
        {
            Console.WriteLine("Тема проєкту: №14. Інтернет-магазин");
            Console.WriteLine("Сутності: Користувач, Товар (головна), Категорія (довідник).");
            Console.WriteLine("\nПоля головної сутності «Товар» (7 полів):");
            Console.WriteLine("  1. артикул             (string) — унікальний код товару");
            Console.WriteLine("  2. назва               (string) — найменування товару");
            Console.WriteLine("  3. код категорії       (int)    — посилання на довідник категорій");
            Console.WriteLine("  4. ціна                (double) — базова ціна за одиницю (грн)");
            Console.WriteLine("  5. знижка              (double) — відсоток знижки (0..100%)");
            Console.WriteLine("  6. кількість на складі (int)    — доступний залишок");
            Console.WriteLine("  7. чи в наявності      (bool)   — статус доступності (так/ні)");
            Console.WriteLine("\nПідсумкова величина: загальна вартість товарів з урахуванням знижок.");
        }

        static void ShowDiscountTable()
        {
            double basePricePerKg = 450.00;
            double fromWeight = 0.5;
            double toWeight = 5.0;
            double step = 0.5;

            Console.WriteLine($"Табулювання вартості замовлення (базова ціна: {basePricePerKg:F2} грн/кг)");
            Console.WriteLine("Знижка зростає на 2% за кожен кілограм ваги.\n");

            Console.WriteLine($"{"Вага (кг)",12} | {"Вартість (грн)",16}");
            Console.WriteLine(new string('-', 32));

            for (double weight = fromWeight; weight <= toWeight + 1e-9; weight += step)
            {
                double discountPercent = weight * 2.0;
                double totalCost = weight * basePricePerKg * (1.0 - discountPercent / 100.0);

                Console.WriteLine($"{weight,12:F2} | {totalCost,16:F2}");
            }
        }

        static void ShowSalesMatrix()
        {
            Console.Write("Введіть кількість категорій товарів (рядків, від 1 до 15): ");
            if (!int.TryParse(Console.ReadLine(), out int rows) || rows <= 0 || rows > 15)
            {
                Console.WriteLine("Помилка: потрібно ввести ціле число від 1 до 15.");
                return;
            }

            Console.Write("Введіть кількість днів тижня (стовпців, від 1 до 7): ");
            if (!int.TryParse(Console.ReadLine(), out int cols) || cols <= 0 || cols > 7)
            {
                Console.WriteLine("Помилка: потрібно ввести ціле число від 1 до 7.");
                return;
            }

            Console.WriteLine("\nКількість проданих товарів за категоріями та днями:\n");

            int[] colTotals = new int[cols];
            int grandTotal = 0;

            Console.Write($"{"Категорія",12}");
            for (int col = 1; col <= cols; col++)
            {
                Console.Write($"{$"День {col}",10}");
            }
            Console.WriteLine($"{"Разом",12}");
            Console.WriteLine(new string('-', 12 + cols * 10 + 12));

            for (int row = 1; row <= rows; row++)
            {
                int rowTotal = 0;
                Console.Write($"{$"Кат. #{row}",12}");

                for (int col = 1; col <= cols; col++)
                {
                    int soldCount = (row * 3 + col * 2) % 15 + 1;

                    rowTotal += soldCount;
                    colTotals[col - 1] += soldCount;

                    Console.Write($"{soldCount,10}");
                }

                grandTotal += rowTotal;
                Console.WriteLine($"{rowTotal,12}");
            }

            Console.WriteLine(new string('-', 12 + cols * 10 + 12));
            Console.Write($"{"Усього",12}");
            for (int col = 0; col < cols; col++)
            {
                Console.Write($"{colTotals[col],10}");
            }
            Console.WriteLine($"{grandTotal,12}");
        }
    }
}