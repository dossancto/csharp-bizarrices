static class NumberExtention
{
    public static int Abs(this int number) => number < 0 ? -number : number;
    public static bool Zero(this int number) => number == 0;
}
