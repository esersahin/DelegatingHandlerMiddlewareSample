using System;
using System.Threading.Tasks;

public class Middleware1
{
    private readonly Func<Task> _next;

    public Middleware1(Func<Task> next)
    {
        _next = next;
    }

    public async Task InvokeAsync()
    {
        // Do something before calling the next middleware in the pipeline
        Console.WriteLine("Middleware 1: Before");

        await _next();

        // Do something after calling the next middleware in the pipeline
        Console.WriteLine("Middleware 1: After");
    }
}

public class Middleware2
{
    private readonly Func<Task> _next;

    public Middleware2(Func<Task> next)
    {
        _next = next;
    }

    public async Task InvokeAsync()
    {
        // Do something before calling the next middleware in the pipeline
        Console.WriteLine("Middleware 2: Before");

        await _next();

        // Do something after calling the next middleware in the pipeline
        Console.WriteLine("Middleware 2: After");
    }
}

public class Middleware3
{
    private readonly Func<Task> _next;

    public Middleware3(Func<Task> next)
    {
        _next = next;
    }

    public async Task InvokeAsync()
    {
        // Do something before calling the next middleware in the pipeline
        Console.WriteLine("Middleware 3: Before");

        await _next();

        // Do something after calling the next middleware in the pipeline
        Console.WriteLine("Middleware 3: After");
    }
}

public class Program
{
    public static async Task Main(string[] args)
    {
        var middleware1 = new Middleware1(() => Task.CompletedTask);
        var middleware2 = new Middleware2(() => Task.CompletedTask);
        var middleware3 = new Middleware3(() => Task.CompletedTask);

        middleware2 = new Middleware2(middleware3.InvokeAsync);
        middleware1 = new Middleware1(middleware2.InvokeAsync);

        var pipeline = middleware1.InvokeAsync;

        await pipeline();
    }
}