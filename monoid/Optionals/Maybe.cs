public sealed class Optional
{
    public static Maybe<T> Of<T>(T? value)
      => (value is not null) ? OfValue(value) : Empty<T>();

    public static Maybe<T> Of<T>(Func<T?> value)
      => Of(value());

    private static Maybe<T> OfValue<T>(T value)
      => Maybe<T>.Of(value);

    public static Maybe<T> Empty<T>()
      => Maybe<T>.Of();
}

public abstract class Maybe<T>
{
    public static bool operator true(Maybe<T> op) => op.IsPresent;
    public static bool operator false(Maybe<T> op) => !op.IsPresent;

    public static implicit operator Maybe<T>(T? value) => value is null ? new Empty() : new Content(value);
    // public static explicit operator T(Maybe<T> m) => m.Match(a => a, throw new Exception("a"));

    public static Maybe<T> Of(T value)
        => new Content(value);

    public static Maybe<T> Of()
         => new Empty();

    public abstract R Match<R>(Func<T, R> funcOf, Func<R> funcEmpty);

    public bool IsPresent
    {
        get => Match(
        a => true,
        () => false
        );
    }

    public T Get
    {
        get => Match(
        a => a,
        () => throw new EmptyException()
        );
    }

    public T GetOrDefault(T @default)
    => Match(
        a => a,
        () => @default
        );

    private class Content : Maybe<T>
    {
        private T Value;

        public Content(T value)
          => Value = value;

        public override R Match<R>(Func<T, R> funcOf, Func<R> funcEmpty)
          => funcOf(Value);

        public override string ToString()
          => $"{Value}";
    }

    private class Empty : Maybe<T>
    {
        public override R Match<R>(Func<T, R> funcOf, Func<R> funcEmpty)
          => funcEmpty();

        public override string ToString()
          => "<empty>";
    }
}
