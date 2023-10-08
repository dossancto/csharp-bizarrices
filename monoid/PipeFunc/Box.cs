public static class Box
{
    public static BoxContent<P> From<P>(P initialValue)
      => new BoxContent<P>(initialValue);

    public class BoxContent<T>
    {
        private Func<T> theValue;

        internal BoxContent(T value)
          => theValue = () => value;

        private BoxContent(Func<T> input)
          => theValue = input;

        public BoxContent<R> Then<R>(Func<T, R> input)
        {
            var f = () => input(theValue());
            return new BoxContent<R>(f);
        }

        public BoxContent<R> Then<R>(Func<T, Task<R>> input)
        {
            var task = input(theValue());
            return new BoxContent<R>(task.Result);
        }

        public async Task<T> Asyncvalue() => await Task.Run(theValue);

        public T Value() => theValue();
    }
}
