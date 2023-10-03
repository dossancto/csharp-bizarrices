public static class Box
{
    public static BoxContent<P> From<P>(P initialValue)
      => new BoxContent<P>(initialValue);

    public static BoxContent<P> FromFunc<P>(Func<P> func)
      => new BoxContent<P>(func());

    public class BoxContent<T>
    {
        public static bool operator true(BoxContent<T> box) => box.IsSuccess;
        public static bool operator false(BoxContent<T> box) => !box.IsSuccess;
        public static bool operator !(BoxContent<T> box) => box ? false : true;

        public T Value { get; private set; } = default!;
        public Exception? Error { get; private set; }

        public BoxContent(T value)
          => Value = value;

        public BoxContent(T value, Exception? ex)
        {
            Value = value;
            Error = ex;
        }

        public BoxContent(Exception e)
          => Error = e;

        public bool IsSuccess
        {
            get => Error is null;
        }

        public (T Value, Exception? Error) Unwrap()
          => (Value, Error);

        public T UnwrapResult()
        {
            if (!IsSuccess) throw Error!;

            return Value;
        }

        public BoxContent<R> Then<R>(Func<T, R> process)
        {
            try
            {
                if (!IsSuccess)
                    return new BoxContent<R>(Error!);

                var result = process(Value);

                return new BoxContent<R>(result, Error);
            }
            catch (Exception e)
            {
                return new BoxContent<R>(e);
            }
        }

        public BoxContent<R> Then<R>(Func<T, Task<R>> process)
        {
            try
            {
                if (!IsSuccess)
                    return new BoxContent<R>(Error!);

                var task = process(Value);

                var result = task.Result;

                return new BoxContent<R>(result, Error);
            }
            catch (AggregateException e)
            {
                return new BoxContent<R>(e.GetBaseException());
            }
            catch (Exception e)
            {
                return new BoxContent<R>(e);
            }
        }

        public override string ToString()
          => IsSuccess ? $"{Value}" : Error!.Message;

    }
}

