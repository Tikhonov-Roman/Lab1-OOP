namespace Lab1.Input;

public static class UserInput
{
    public static string ReadId(string request, Func<string, bool> validationFunc)
    {
        while (true)
        {
            Console.Write(request);
            string? id = Console.ReadLine()?.Trim();
            if (id is not null && validationFunc(id)) return id;
        }
    }

    public static string ReadNotBlankLine(string request)
    {
        while (true)
        {
            Console.Write(request);
            var value = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }

            Console.WriteLine("Значение не может быть пустым.");
        }
    }

    public static int ReadPositiveInt(string request)
    {
        while (true)
        {
            Console.Write(request);
            if (int.TryParse(Console.ReadLine()?.Trim(), out var value) && value > 0)
            {
                return value;
            }

            Console.WriteLine("Число дожно быть положительным.");
        }
    }
}