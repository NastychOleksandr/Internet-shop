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
                        Console.WriteLine("Пункт «Табулювання» поки в розробці.");
                        Pause();
                        break;

                    case "3":
                        Console.WriteLine("Пункт «Зведена таблиця» поки в розробці.");
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
    }
}