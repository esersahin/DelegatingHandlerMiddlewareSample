public class Middleware
{
    private readonly Func<Task> _next;

    public Middleware(Func<Task> next)
    {
        _next = next;
    }

    public async Task InvokeAsync()
    {
        // Do something before calling the next middleware in the pipeline
        Console.WriteLine("Middleware: Before");

        await _next();

        // Do something after calling the next middleware in the pipeline
        Console.WriteLine("Middleware: After");
    }
}

public class Program
{
    public static async Task Main(string[] args)
    {
        var middleware = new Middleware(() =>
        {
            Console.WriteLine("...");
            return Task.CompletedTask;
        });

        // This line demonstrates how to chain multiple middleware components together
        var pipeline = middleware.InvokeAsync;

        await pipeline();
    }
}