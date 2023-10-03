internal class Program
{
    private static void Main(string[] args)
    {
        Calculator cal = -4;

        Console.WriteLine($"O número é {(int)cal}");

        if (cal.isZero) Console.WriteLine("O número é ZERO meu mano");

        if (cal.isPositive) Console.WriteLine("O número é positivo meu brother");

        if (!cal.isPositive) Console.WriteLine("O número é negativo meu heroi");

        var msg = cal.Match(
           p => "Aqui é ativado se for positivo",
           n => "Aqui é ativado se for negativo",
           () => "Aqui é ativado se for zero"
       );

        Console.WriteLine(msg);

        var factorial = cal.Factorial;
        Console.WriteLine($"Factorial: {factorial}");

        Console.WriteLine($"Pow: {cal.Pow(3)}");

        Console.WriteLine($"Raiz quadrada: {cal.Sqrt}");
    }
}
