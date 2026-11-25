using System;
using System.Globalization;
using System.Text;

namespace OnlineStore
{
    internal class Program
    {
        const int Capacity = 100;

        static int[] ids = new int[Capacity];
        static string[] articles = new string[Capacity];
        static string[] names = new string[Capacity];
        static int[] categoryIds = new int[Capacity];
        static double[] prices = new double[Capacity];
        static double[] discounts = new double[Capacity];
        static int[] stockQuantities = new int[Capacity];
        static bool[] inStockFlags = new bool[Capacity];

        static int count = 0;
        static int nextId = 1;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            double[] priceSeries = new double[Capacity];
            int seriesCount = 0;

            bool running = true;

            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": ShowAllProducts(); Pause(); break;
                    case "2": AddProduct(); Pause(); break;
                    case "3": SearchByNameSubstring(); Pause(); break;
                    case "4": SearchByPriceRange(); Pause(); break;
                    case "5": SeedSampleProducts(); Pause(); break;
                    case "6": ShowAbout(); Pause(); break;
                    case "7": ShowDiscountTable(); Pause(); break;
                    case "8": ShowSalesMatrix(); Pause(); break;
                    case "9":
                        double val = ReadDouble("Введіть значення ціни (0..1000000): ", 0, 1000000);
                        AddSeriesValue(priceSeries, ref seriesCount, val, Capacity);
                        Console.WriteLine($"Лічильник у Main: {seriesCount}");
                        Pause();
                        break;
                    case "10": PrintSeriesTable(priceSeries, seriesCount); Pause(); break;
                    case "11":
                        if (seriesCount <= 0)
                        {
                            Console.WriteLine("Масив порожній, видаляти нічого.");
                        }
                        else
                        {
                            PrintSeriesTable(priceSeries, seriesCount);
                            int idx = ReadInt($"Введіть номер для видалення (1..{seriesCount}): ", 1, seriesCount);
                            RemoveSeriesAt(priceSeries, ref seriesCount, idx);
                        }
                        Pause();
                        break;
                    case "12": PrintSeriesStatistics(priceSeries, seriesCount); Pause(); break;
                    case "13": RunTypeExperiment(priceSeries, seriesCount); Pause(); break;
                    case "0": running = false; Console.WriteLine("Роботу завершено. До побачення!"); break;
                    default: Console.WriteLine("Немає такого пункту меню. Спробуйте ще раз."); Pause(); break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("=== Інтернет-магазин ===");
            Console.WriteLine("1 — показати всі записи");
            Console.WriteLine("2 — додати запис");
            Console.WriteLine("3 — знайти за частиною назви");
            Console.WriteLine("4 — знайти за діапазоном ціни");
            Console.WriteLine("5 — заповнити тестовими записами");
            Console.WriteLine("6 — довідка про предметну область");
            Console.WriteLine("7 — табулювання вартості замовлення зі знижкою");
            Console.WriteLine("8 — зведена таблиця продажів за категоріями та днями");
            Console.WriteLine("9 — [ЛР9] додати значення у масив");
            Console.WriteLine("10 — [ЛР9] показати масив значень");
            Console.WriteLine("11 — [ЛР9] видалити елемент за номером");
            Console.WriteLine("12 — [ЛР9] показати статистику масиву");
            Console.WriteLine("13 — [ЛР9] дослід: значимий та посилальний типи");
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

        static string ReadNonEmptyText(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }
                Console.WriteLine("Помилка: поле не може бути порожнім.");
            }
        }

        static bool ReadYesNo(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    string normalized = input.Trim().ToLower();
                    if (normalized == "так" || normalized == "т" || normalized == "yes" || normalized == "y")
                    {
                        return true;
                    }
                    if (normalized == "ні" || normalized == "н" || normalized == "no" || normalized == "n")
                    {
                        return false;
                    }
                }
                Console.WriteLine("Помилка: введіть «так» або «ні».");
            }
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

        static int FindIndexById(int id)
        {
            for (int i = 0; i < count; i++)
            {
                if (ids[i] == id)
                {
                    return i;
                }
            }
            return -1;
        }

        static void PrintProductHeader()
        {
            Console.WriteLine(
                $"{"ID",4} {"Артикул",-9} {"Назва",-23} {"Кат.",4} {"Ціна",10} {"Зниж.",7} {"Склад",6} {"Наявн.",7}"
            );
            Console.WriteLine(new string('-', 76));
        }

        static void PrintProductRow(int i)
        {
            string availability = inStockFlags[i] ? "так" : "ні";
            Console.WriteLine(
                $"{ids[i],4} {articles[i],-9} {names[i],-23} {categoryIds[i],4} {prices[i],10:F2} {discounts[i],6:F1}% {stockQuantities[i],6} {availability,7}"
            );
        }

        static void SearchByNameSubstring()
        {
            if (count == 0)
            {
                Console.WriteLine("Сховище порожнє, шукати нічого.");
                return;
            }

            string query = ReadNonEmptyText("Введіть частину назви товару: ");
            int found = 0;
            bool headerPrinted = false;

            for (int i = 0; i < count; i++)
            {
                if (names[i].Contains(query))
                {
                    if (!headerPrinted)
                    {
                        Console.WriteLine();
                        PrintProductHeader();
                        headerPrinted = true;
                    }
                    PrintProductRow(i);
                    found++;
                }
            }

            if (found == 0)
            {
                Console.WriteLine($"За запитом «{query}» нічого не знайдено.");
                return;
            }

            Console.WriteLine(new string('-', 76));
            Console.WriteLine($"Знайдено записів: {found}.");
        }

        static void SearchByPriceRange()
        {
            if (count == 0)
            {
                Console.WriteLine("Сховище порожнє, шукати нічого.");
                return;
            }

            double minPrice = ReadDouble("Введіть нижню межу ціни (0..1000000): ", 0.0, 1000000.0);
            double maxPrice = ReadDouble("Введіть верхню межу ціни (0..1000000): ", 0.0, 1000000.0);

            if (minPrice > maxPrice)
            {
                Console.WriteLine("Помилка: нижня межа ціни не може бути більшою за верхню.");
                return;
            }

            int found = 0;
            bool headerPrinted = false;

            for (int i = 0; i < count; i++)
            {
                if (prices[i] >= minPrice && prices[i] <= maxPrice)
                {
                    if (!headerPrinted)
                    {
                        Console.WriteLine();
                        PrintProductHeader();
                        headerPrinted = true;
                    }
                    PrintProductRow(i);
                    found++;
                }
            }

            if (found == 0)
            {
                Console.WriteLine($"Товарів із ціною в діапазоні [{minPrice:F2}..{maxPrice:F2}] не знайдено.");
                return;
            }

            Console.WriteLine(new string('-', 76));
            Console.WriteLine($"Знайдено записів у діапазоні: {found}.");
        }

        static void AddProduct()
        {
            if (count == Capacity)
            {
                Console.WriteLine("Сховище заповнене, видаліть щось зайве.");
                return;
            }

            string article = ReadNonEmptyText("Артикул (string): ");
            string name = ReadNonEmptyText("Назва (string): ");
            int categoryId = ReadInt("Код категорії (int, 1..20): ", 1, 20);
            double price = ReadDouble("Ціна за одиницю (double, 0.01..1000000): ", 0.01, 1000000.0);
            double discount = ReadDouble("Знижка (double, 0..100%): ", 0.0, 100.0);
            int stock = ReadInt("Кількість на складі (int, 0..100000): ", 0, 100000);
            bool inStock = ReadYesNo("Чи в наявності (так/ні): ");

            ids[count] = nextId;
            articles[count] = article;
            names[count] = name;
            categoryIds[count] = categoryId;
            prices[count] = price;
            discounts[count] = discount;
            stockQuantities[count] = stock;
            inStockFlags[count] = inStock;

            count++;
            nextId++;

            Console.WriteLine($"Додано товар №{ids[count - 1]}.");
        }

        static void SeedSampleProducts()
        {
            string[] sampleArticles = { "ART-001", "ART-002", "ART-003", "ART-004", "ART-005", "ART-006" };
            string[] sampleNames = { "Механічна клавіатура", "Ігрова миша", "Монітор 27\"", "USB-хаб 4-портовий", "Вебкамера FullHD", "Килимок XL" };
            int[] sampleCategories = { 1, 1, 2, 3, 2, 3 };
            double[] samplePrices = { 2600.00, 1350.00, 9400.00, 780.00, 1650.00, 520.00 };
            double[] sampleDiscounts = { 10.0, 5.0, 15.0, 0.0, 12.0, 20.0 };
            int[] sampleStocks = { 8, 15, 3, 20, 6, 14 };
            bool[] sampleInStock = { true, true, true, true, true, false };

            int added = 0;

            for (int i = 0; i < sampleArticles.Length; i++)
            {
                if (count == Capacity)
                {
                    Console.WriteLine("Сховище заповнене, решту тестових записів не додано.");
                    break;
                }

                ids[count] = nextId;
                articles[count] = sampleArticles[i];
                names[count] = sampleNames[i];
                categoryIds[count] = sampleCategories[i];
                prices[count] = samplePrices[i];
                discounts[count] = sampleDiscounts[i];
                stockQuantities[count] = sampleStocks[i];
                inStockFlags[count] = sampleInStock[i];

                count++;
                nextId++;
                added++;
            }

            Console.WriteLine($"Додано тестових записів: {added}. Усього записів у сховищі: {count}.");
        }

        static double CalculateTotalStockValue(double[] itemPrices, double[] itemDiscounts, int[] itemStocks, int itemsCount)
        {
            if (itemsCount <= 0)
            {
                return 0.0;
            }

            double total = 0.0;

            for (int i = 0; i < itemsCount; i++)
            {
                double unitPriceWithDiscount = itemPrices[i] * (1.0 - itemDiscounts[i] / 100.0);
                total += unitPriceWithDiscount * itemStocks[i];
            }

            return total;
        }

        static void ShowAllProducts()
        {
            if (count == 0)
            {
                Console.WriteLine("Записів поки немає.");
                return;
            }

            PrintProductHeader();

            for (int i = 0; i < count; i++)
            {
                PrintProductRow(i);
            }

            double totalStockValue = CalculateTotalStockValue(prices, discounts, stockQuantities, count);

            Console.WriteLine(new string('-', 76));
            Console.WriteLine($"Записів: {count}.");
            Console.WriteLine($"Загальна вартість товарів на складі зі знижкою: {totalStockValue:F2} грн.");
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

        static void AddSeriesValue(double[] values, ref int itemsCount, double newValue, int capacity)
        {
            if (itemsCount >= capacity)
            {
                Console.WriteLine("Місця в масиві немає.");
                return;
            }

            values[itemsCount] = newValue;
            itemsCount++;
        }

        static void PrintSeriesTable(double[] values, int itemsCount)
        {
            if (itemsCount <= 0)
            {
                Console.WriteLine("Масив порожній.");
                return;
            }

            Console.WriteLine($"{"№",5} | {"Значення",14}");
            Console.WriteLine(new string('-', 24));

            for (int i = 0; i < itemsCount; i++)
            {
                Console.WriteLine($"{i + 1,5} | {values[i],14:F2}");
            }
        }

        static void RemoveSeriesAt(double[] values, ref int itemsCount, int index)
        {
            if (index < 1 || index > itemsCount)
            {
                Console.WriteLine("Помилка: неправильний номер елемента. Змін немає.");
                return;
            }

            int pos = index - 1;
            for (int i = pos; i < itemsCount - 1; i++)
            {
                values[i] = values[i + 1];
            }

            values[itemsCount - 1] = 0;
            itemsCount--;
            Console.WriteLine($"Елемент №{index} видалено. Поточний лічильник: {itemsCount}");
        }

        static bool FindSeriesMax(double[] values, int itemsCount, out double maxValue, out int maxNumber)
        {
            if (itemsCount <= 0)
            {
                maxValue = 0;
                maxNumber = 0;
                return false;
            }

            maxValue = values[0];
            maxNumber = 1;

            for (int i = 1; i < itemsCount; i++)
            {
                if (values[i] > maxValue)
                {
                    maxValue = values[i];
                    maxNumber = i + 1;
                }
            }

            return true;
        }

        static void CalculateSeriesStats(double[] values, int itemsCount, out double sum, out double average)
        {
            sum = 0;
            for (int i = 0; i < itemsCount; i++)
            {
                sum += values[i];
            }

            average = itemsCount > 0 ? sum / itemsCount : 0;
        }

        static void PrintSeriesStatistics(double[] values, int itemsCount)
        {
            if (!FindSeriesMax(values, itemsCount, out double maxValue, out int maxNumber))
            {
                Console.WriteLine("Набір порожній: статистики немає.");
                return;
            }

            CalculateSeriesStats(values, itemsCount, out double sum, out double average);

            Console.WriteLine($"Кількість елементів: {itemsCount}");
            Console.WriteLine($"Найбільше значення: {maxValue:F2} (номер: {maxNumber})");
            Console.WriteLine($"Сума: {sum:F2}");
            Console.WriteLine($"Середнє: {average:F2}");
        }

        static void ChangeIntWithoutRef(int number)
        {
            number = 999;
        }

        static void ChangeArrayElement(double[] values, int itemsCount)
        {
            if (itemsCount > 0)
            {
                values[0] = values[0] + 50.0;
            }
        }

        static void RunTypeExperiment(double[] values, int itemsCount)
        {
            int sampleInt = 15;
            Console.WriteLine($"Значущий тип int до виклику: {sampleInt}");
            ChangeIntWithoutRef(sampleInt);
            Console.WriteLine($"Значущий тип int після виклику без ref: {sampleInt}");

            if (itemsCount > 0)
            {
                Console.WriteLine($"\nПерший елемент масиву до виклику: {values[0]:F2}");
                ChangeArrayElement(values, itemsCount);
                Console.WriteLine($"Перший елемент масиву після виклику: {values[0]:F2}");
            }
            else
            {
                double[] demo = { 100.0, 200.0 };
                Console.WriteLine($"\nТестовий масив до виклику: {demo[0]:F2}");
                ChangeArrayElement(demo, demo.Length);
                Console.WriteLine($"Тестовий масив після виклику: {demo[0]:F2}");
            }
        }
    }
}