using System;
using System.Globalization;
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
                ShowMenu();
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": ShowAbout(); Pause(); break;
                    case "2": ShowDiscountTable(); Pause(); break;
                    case "3": ShowSalesMatrix(); Pause(); break;
                    case "0": running = false; Console.WriteLine("Роботу завершено. До побачення!"); break;
                    default: Console.WriteLine("Немає такого пункту меню. Спробуйте ще раз."); Pause(); break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("=== Інтернет-магазин ===");
            Console.WriteLine("1 — довідка про предметну область");
            Console.WriteLine("2 — табулювання вартості замовлення зі знижкою");
            Console.WriteLine("3 — зведена таблиця продажів за категоріями та днями");
            Console.WriteLine("0 — вихід");
            Console.Write("Ваш вибір: ");
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

        static double ReadDouble(string prompt, double min, double max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    string normalized = input.Trim().Replace(',', '.');
                    if (double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out double value)
                        && value >= min && value <= max)
                    {
                        return value;
                    }
                }
                Console.WriteLine($"Помилка: введіть число в межах від {min} до {max}.");
            }
        }

        static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (int.TryParse(input, out int value) && value >= min && value <= max)
                {
                    return value;
                }
                Console.WriteLine($"Помилка: введіть ціле число в межах від {min} до {max}.");
            }
        }

        static double CalculateDiscountedCost(double weight, double basePricePerKg)
        {
            double discountPercent = weight * 2.0;
            return weight * basePricePerKg * (1.0 - discountPercent / 100.0);
        }

        static void PrintDiscountTable(double startWeight, double endWeight, double step, double basePricePerKg)
        {
            Console.WriteLine($"\nТабулювання вартості замовлення (базова ціна: {basePricePerKg:F2} грн/кг)");
            Console.WriteLine("Знижка зростає на 2% за кожен кілограм ваги.\n");

            Console.WriteLine($"{"Вага (кг)",12} | {"Вартість (грн)",16}");
            Console.WriteLine(new string('-', 32));

            for (double weight = startWeight; weight <= endWeight + 1e-9; weight += step)
            {
                double totalCost = CalculateDiscountedCost(weight, basePricePerKg);
                Console.WriteLine($"{weight,12:F2} | {totalCost,16:F2}");
            }
        }

        static void ShowDiscountTable()
        {
            double basePricePerKg = ReadDouble("Введіть базову ціну за 1 кг (грн, 1-100000): ", 1.0, 100000.0);
            double fromWeight = ReadDouble("Введіть початкову вагу (кг, 0.1-50): ", 0.1, 50.0);
            double toWeight = ReadDouble("Введіть кінцеву вагу (кг, від початкової до 50): ", fromWeight, 50.0);
            double step = ReadDouble("Введіть крок ваги (кг, 0.1-10): ", 0.1, 10.0);

            PrintDiscountTable(fromWeight, toWeight, step, basePricePerKg);
        }

        static void PrintSalesHeader(int cols)
        {
            Console.Write($"{"Категорія",12}");
            for (int col = 1; col <= cols; col++)
            {
                Console.Write($"{$"День {col}",10}");
            }
            Console.WriteLine($"{"Разом",12}");
            Console.WriteLine(new string('-', 12 + cols * 10 + 12));
        }

        static int PrintSalesRow(int row, int cols, int[] colTotals)
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

            Console.WriteLine($"{rowTotal,12}");
            return rowTotal;
        }

        static void PrintSalesSummary(int[] colTotals, int grandTotal)
        {
            Console.WriteLine(new string('-', 12 + colTotals.Length * 10 + 12));
            Console.Write($"{"Усього",12}");
            for (int col = 0; col < colTotals.Length; col++)
            {
                Console.Write($"{colTotals[col],10}");
            }
            Console.WriteLine($"{grandTotal,12}");
        }

        static void ShowSalesMatrix()
        {
            int rows = ReadInt("Введіть кількість категорій товарів (рядків, від 1 до 15): ", 1, 15);
            int cols = ReadInt("Введіть кількість днів тижня (стовпців, від 1 до 7): ", 1, 7);

            Console.WriteLine("\nКількість проданих товарів за категоріями та днями:\n");

            int[] colTotals = new int[cols];
            int grandTotal = 0;

            PrintSalesHeader(cols);

            for (int row = 1; row <= rows; row++)
            {
                grandTotal += PrintSalesRow(row, cols, colTotals);
            }

            PrintSalesSummary(colTotals, grandTotal);
        }
    }
}

static void AddPrice(double[] prices, ref int count, double newPrice, int capacity)
{
    if (count >= capacity)
    {
        Console.WriteLine("Сховище цін заповнене.");
        return;
    }

    prices[count] = newPrice;
    count++;
}

static void PrintPricesTable(double[] prices, int count)
{
    if (count <= 0)
    {
        Console.WriteLine("Масив цін порожній.");
        return;
    }

    Console.WriteLine($"{"№",5} | {"Ціна (грн)",14}");
    Console.WriteLine(new string('-', 24));

    for (int i = 0; i < count; i++)
    {
        Console.WriteLine($"{i + 1,5} | {prices[i],14:F2}");
    }
}

static void TryChangeDiscount(int discount)
{
    discount = 50;
}

static void ApplyFirstPriceMarkup(double[] prices, int count)
{
    if (count > 0)
    {
        prices[0] = prices[0] + 100.0;
    }
}

static void RunTypeExperiment(double[] prices, int count)
{
    int discount = 10;
    Console.WriteLine($"До виклику TryChangeDiscount: discount = {discount}%");
    TryChangeDiscount(discount);
    Console.WriteLine($"Після виклику TryChangeDiscount (int без ref): discount = {discount}%");

    if (count > 0)
    {
        Console.WriteLine($"\nДо виклику ApplyFirstPriceMarkup: prices[0] = {prices[0]:F2}");
        ApplyFirstPriceMarkup(prices, count);
        Console.WriteLine($"Після виклику ApplyFirstPriceMarkup (масив): prices[0] = {prices[0]:F2}");
    }
    else
    {
        double[] demo = { 250.0, 400.0 };
        Console.WriteLine($"\nТестовий масив до виклику: demo[0] = {demo[0]:F2}");
        ApplyFirstPriceMarkup(demo, demo.Length);
        Console.WriteLine($"Тестовий масив після виклику: demo[0] = {demo[0]:F2}");
    }
}

static bool FindMaxPrice(double[] prices, int count, out double maxPrice, out int maxNumber)
{
    if (count <= 0)
    {
        maxPrice = 0;
        maxNumber = 0;
        return false;
    }

    maxPrice = prices[0];
    maxNumber = 1;

    for (int i = 1; i < count; i++)
    {
        if (prices[i] > maxPrice)
        {
            maxPrice = prices[i];
            maxNumber = i + 1;
        }
    }

    return true;
}

static void CalculateStatistics(double[] prices, int count, out double sum, out double average)
{
    sum = 0;
    for (int i = 0; i < count; i++)
    {
        sum += prices[i];
    }

    average = count > 0 ? sum / count : 0;
}

static void PrintStatistics(double[] prices, int count)
{
    if (!FindMaxPrice(prices, count, out double maxPrice, out int maxNumber))
    {
        Console.WriteLine("Набір порожній: статистики немає.");
        return;
    }

    CalculateStatistics(prices, count, out double sum, out double average);

    Console.WriteLine($"Кількість значень: {count}");
    Console.WriteLine($"Найбільше значення: {maxPrice:F2} (номер: {maxNumber})");
    Console.WriteLine($"Сума: {sum:F2}");
    Console.WriteLine($"Середнє: {average:F2}");
}
static void RemoveAt(double[] values, ref int count, int index)
{
    if (index < 1 || index > count)
    {
        Console.WriteLine("Помилка: неправильний номер елемента. Змін немає.");
        return;
    }

    int pos = index - 1;
    for (int i = pos; i < count - 1; i++)
    {
        values[i] = values[i + 1];
    }

    values[count - 1] = 0;
    count--;
    Console.WriteLine($"Елемент №{index} видалено. Поточний count: {count}");
}