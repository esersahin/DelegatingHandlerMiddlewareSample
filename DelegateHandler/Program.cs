// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

MyDelegateHandler handler = new MyDelegateHandler();

MiddlewareDelegate middlewarePipeline = MiddlewareHandler.InitialMiddleware;
middlewarePipeline += MiddlewareHandler.OtherMiddleware;
middlewarePipeline += (message, next) => handler.PrintMessage("the end..");

// Invoke the middleware pipeline
middlewarePipeline("Hello, world!", (message) => { Console.WriteLine("Chaining..");});

class MyDelegateHandler
{
    public void PrintMessage
    (
        string message
    )
    {
        Console.WriteLine
        (
            $"Final action: {message}"
        );
    }
}

// Create the middleware pipeline

delegate void MiddlewareDelegate(string message, Action<string> next);

class MiddlewareHandler
{
    public static void InitialMiddleware
    (
        string message,
        Action<string> next
    )
    {
        Console.WriteLine
        (
            $"Initial Middleware: {message}"
        );

        next(message);
    }

    public static void OtherMiddleware
    (
        string message,
        Action<string> next
    )
    {
        Console.WriteLine
        (
            $"Other Middleware: {message}"
        );

        next(message);
    }
}