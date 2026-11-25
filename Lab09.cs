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