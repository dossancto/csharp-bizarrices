internal class Program
{
    private async static Task Main(string[] args)
    {
        // FirstBox();
        // SeccoundBox();
        // ThirdBox();
        // FourthBox();
        // BoxFive();
        // BoxFive1();
        // BoxFive2();
        // BoxFive3();
        // BoxSix();
        await Box7();
        await Box8();
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
        var r = Box.From(10 * 5)
          .Then(num => num * num)
          .Then(num => num - 5);

        // Console.WriteLine(r.Value);
    }

    private static void ThirdBox()
    {
        var username = Box.From(FetchUser())
          .Then(user => user.Username + " dos Santos Silva");
        // .Pipe(name => name.ToUpper()).Value;

        var anoNascenca = Box.From(FetchUser())
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
    // private static void BoxFive()
    // {
    //     // Like golang
    //     var (val, err) = Box.From("523aa")
    //       .Then(str => int.Parse(str))
    //       .Then(num => num % 360).Unwrap();

    //     if (err != null)
    //         Console.WriteLine(err);
    //     else
    //         Console.WriteLine(val);
    // }

    // Basic error Handleing.
    // private static void BoxFive1()
    // {
    //     var res = Box.From("Box Five 1")
    //       .Then(str => int.Parse(str))
    //       .Then(num => num % 360);

    //     if (res) // same as res.IsSuccesS
    //     {
    //         Console.WriteLine(res.Value);
    //     }
    //     else
    //     {
    //         Console.WriteLine("Something went wrong => " + res.Error!.GetType());
    //         Console.WriteLine(res.Error!.Message);
    //     }

    // }

    // Handle erros
    // private static void BoxFive2()
    // {
    //     var res = Box.From("523")
    //       .Then(str => int.Parse(str), "Convert string to int")
    //       .Then(num => num % 360);

    //     try
    //     {
    //         var angle = res.UnwrapResult();

    //         Console.WriteLine("================");
    //         Console.WriteLine("Run logs\n");
    //         foreach (var log in res.Logs)
    //         {
    //             Console.WriteLine(log);
    //         }
    //         Console.WriteLine("================\n");

    //         Console.WriteLine(angle);
    //     }
    //     catch (FormatException ex) { Console.WriteLine(ex.Message); }
    //     catch (Exception) { Console.WriteLine("Generic Exception"); }
    // }

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

        try
        {
            var angle = res.Value();
            Console.WriteLine(angle);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

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

    private static string ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new Exception("Place write a password");
        {
        }

        if (password.Length < 8)
        {
            throw new Exception("Password too short");
        }

        return password;
    }

    private static (string Password, string Salt) HashPassword(string password)
      => (password, Guid.NewGuid().ToString());

    private static User RegisterUserWithPassword(User user)
    {
        // Pefrorms some IO operation, as save in database;

        return user;
    }

    private static User MountUser(string name, (string password, string salt) hashedPassword)
      => new User(name, ((byte)hashedPassword.password.Length));

    private async static Task Box7()
    {
        var userName = "lu-css";
        var password = "something valid";

        var task = Box.From(password)
          .Then(ValidatePassword)
          .Then(HashPassword)
          .Then(p => MountUser(userName, p))
          .Then(RegisterUserWithPassword);

        try
        {
            var res = await task.Asyncvalue();

            Console.WriteLine(res);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private async static Task Box8()
    {
        var task = Box.From("4234")
          .Then(str => int.Parse(str))
          .Then(n => n + 5);

        try
        {
            Console.WriteLine(await task.Asyncvalue());
        }
        catch (Exception e)
        {
            // Console.WriteLine(e);
            Console.WriteLine(e);
        }
    }
}

