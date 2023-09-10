internal class Program
{
    private static void Main(string[] args)
    {
        var cal = Calculator.New(3);

        if (cal.isZero) Console.WriteLine("O número é ZERO meu mano");

        if (cal.isPositive) Console.WriteLine("O número é positivo meu brother");

        if (!cal.isPositive) Console.WriteLine("O número é negativo meu heroi");

        Console.WriteLine($"Factorial: {cal.Factorial()}");
    }
}
