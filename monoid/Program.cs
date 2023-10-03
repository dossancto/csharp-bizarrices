internal class Program
{
    private static void Main(string[] args)
    {
        // FirstBox();
        // SeccoundBox();
        // ThirdBox();
        // FourthBox();
        // BoxFive();
        // BoxFive1();
        // BoxFive2();
        BoxFive3();
        // BoxSix();
    }

    private static void FirstBox()
    {
        var res = Box.From(5)
          .Then(a => a + 5)
          .Then(n => n * 7)
          .Then(n => n.ToString())
          .Then(n =>
          {
              Console.WriteLine("Só passando aqui pra dar um salve para meu amigão junior");
              return n;
          });

        // Console.WriteLine(res.Value);
    }

    private static void SeccoundBox()
    {
        var r = Box.FromFunc(() => 10 + 5)
          .Then(num => num * num)
          .Then(num => num - 5);

        // Console.WriteLine(r.Value);
    }

    private static void ThirdBox()
    {
        var username = Box.FromFunc(FetchUser)
          .Then(user => user.Username + " dos Santos Silva");
        // .Pipe(name => name.ToUpper()).Value;

        var anoNascenca = Box.FromFunc(FetchUser)
          .Then(user => user.Age);
        // .Pipe(age => 2023 - age).Value;

        Console.WriteLine($"{username} nasceu em {anoNascenca}");
        // Output =>
        // LUCAS DOS SANTOS SILVA nasceu em 2005
    }

    private static User FetchUser() => new User("Lucas", 18);
    private static string SendEmailCode(string email) => "Code sended: {code}";
    record User(string Username, byte Age) { }

    private static void FourthBox()
    {
        var users = new List<User>() {
            new User("lucas", 18),
            new User("Débora", 19),
            new User("Alguem", 18)
        };

        var groupedNUms = users.GroupBy(n => n.Age);

        foreach (var numGroup in groupedNUms)
        {
            Console.WriteLine(numGroup.First().Age + " anos");
            foreach (var num in numGroup)
            {
                Console.WriteLine(" - " + num.Username);
            }
        }
    }

    // Unwrap result
    private static void BoxFive()
    {
        // Like golang
        var (val, err) = Box.From("523aa")
          .Then(str => int.Parse(str))
          .Then(num => num % 360).Unwrap();

        if (err != null)
            Console.WriteLine(err);
        else
            Console.WriteLine(val);
    }

    // Basic error Handleing.
    private static void BoxFive1()
    {
        var res = Box.From("Box Five 1")
          .Then(str => int.Parse(str))
          .Then(num => num % 360);

        if (res) // same as res.IsSuccesS
        {
            Console.WriteLine(res.Value);
        }
        else
        {
            Console.WriteLine("Something went wrong => " + res.Error!.GetType());
            Console.WriteLine(res.Error!.Message);
        }

    }

    // Handle erros
    private static void BoxFive2()
    {
        var res = Box.From("523aa")
          .Then(str => int.Parse(str))
          .Then(num => num % 360);

        try
        {
            var angle = res.UnwrapResult();
            Console.WriteLine(angle);
        }
        catch (FormatException ex) { Console.WriteLine(ex.Message); }
        catch (Exception) { Console.WriteLine("Generic Exception"); }
    }

    // Async calls
    private static void BoxFive3()
    {
        var res = Box.From("523a")
          .Then(async str =>
          {
              await Task.Delay(1000);
              return int.Parse(str);
          })
          .Then(async num =>
          {
              Console.WriteLine("Other");
              await Task.Delay(1000);

              return num;
          })
          .Then(num => num % 360);

        if (!res)
        {
            var ex = res.Error!;
            Console.WriteLine(ex);
            return;
        }

        var angle = res.Value;
        Console.WriteLine(angle);
    }

    private static void BoxSix()
    {
        var op = Options();

        if (op) // same as op.IsPresent
            Console.WriteLine("Valor está presente");

        Console.WriteLine(op.GetOrDefault("Tem nada aq"));

        var emptyOp = Optional.Empty<string>();
        Console.WriteLine(emptyOp.GetOrDefault("Tem nada aq"));

        var username = "lucas";

        // If function returns NULL, "Empty" is used.
        var funcOp = Optional.Of(FetchUser(username));

        Console.WriteLine(funcOp ? funcOp.Get : $"User with this username '{username}' not found");
        Console.WriteLine(Optional.Of(FetchUser("lucas1")));
        // Output =>
        //
        // Valor está presente
        // salve
        // Tem nada aq
        // User { Username = Lucas, Age = 18 }
        // <empty>
    }

    static List<User> dbUsers = new List<User>() {
      new User("Lucas", 18),
      new User("lu-css", 19)
    };

    private static User? FetchUser(string username)
      => dbUsers.Find(user => user.Username.ToLower() == username.ToLower());

    private static Maybe<string> Options()
    {
        // return Optional.Empty<string>();
        return Optional.Of("salve");
    }
}

