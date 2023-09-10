public abstract class Calculator
{
    public static Calculator New(int number)
    {
        if (number == 0)
            return new Zero();

        return number > 0 ? new Positive(number) : new Negative(number);
    }

    public static bool operator true(Calculator c) => !(c.isZero);
    public static bool operator false(Calculator c) => c.isZero;
    public static bool operator !(Calculator c) => c ? false : true;
    public static explicit operator int(Calculator c) => c.Number;

    public double Factorial
    {
        get
        {
            int number = Number;

            if (!this) return 0;
            if (number.Abs() == 1) return number;

            var result = Fac(number);

            return isNegative ? -result : result;
        }
    }

    private double Fac(int n)
    {
        n = n.Abs();

        if (n == 1) return n;

        return n * (Fac(n - 1));
    }

    public double Pow(int n) =>
      Math.Pow(Number, n);

    public string Sqrt
    {
        get => MatchSqrt(
            pos => Math.Sqrt(pos).ToString(),
            neg => $"{Math.Sqrt(neg.Abs())}i"
            );
    }


    public abstract R Match<R>(Func<int, R> positiveFunc, Func<int, R> negativeFunc, Func<R> zeroFunc);
    public abstract R MatchSqrt<R>(Func<int, R> positiveFunc, Func<int, R> negativeFunc);

    public int AbsoluteNumber
    {
        get => Match(
            p => p,
            n => -n,
            () => 0
          );
    }

    public int Number
    {
        get => Match(
            p => p,
            n => n,
            () => 0
          );
    }

    public bool isZero
    {
        get => Match(
          _ => false,
          _ => false,
          () => true
        );
    }

    public bool isPositive
    {
        get => Match(
          positive => true,
          negative => false,
          () => false
        );
    }

    public bool isNegative
    {
        get => !isZero && !isPositive;
    }

    public sealed class Zero : Calculator
    {
        public override R Match<R>(Func<int, R> positiveFunc, Func<int, R> negativeFunc, Func<R> zeroFunc) =>
             zeroFunc();

        public override R MatchSqrt<R>(Func<int, R> positiveFunc, Func<int, R> negativeFunc) => positiveFunc(0);
    }
    public sealed class Positive : Calculator
    {
        private readonly int Value;

        public Positive(int value) =>
            Value = value;

        public override R Match<R>(Func<int, R> positiveFunc, Func<int, R> negativeFunc, Func<R> zeroFunc) =>
          positiveFunc(Value);

        public override R MatchSqrt<R>(Func<int, R> positiveFunc, Func<int, R> negativeFunc) =>
          positiveFunc(Value);
    }

    public class Negative : Calculator
    {
        private readonly int Value;

        public Negative(int value) =>
            Value = value;


        public override R Match<R>(Func<int, R> positiveFunc, Func<int, R> negativeFunc, Func<R> zeroFunc) =>
          negativeFunc(Value);

        public override R MatchSqrt<R>(Func<int, R> positiveFunc, Func<int, R> negativeFunc) =>
          negativeFunc(Value);
    }
}
