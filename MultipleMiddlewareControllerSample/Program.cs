var pipeline = new List
<
    Func
    <
        Func<RequestContext, Task>,
        Func<RequestContext, Task>
    >
>
{
    ExceptionMiddleware,
    LogMiddleware
};

var routes = new Dictionary<string, Func<RequestContext, Task>?>
{
    { "/", IndexRoute },
    { "/about", AboutRoute }
};

var requestContext = new RequestContext
{
    RequestUrl = "/"
};

var finalFunction = BuildPipeline(pipeline, routes);

await finalFunction(requestContext);

Console.WriteLine($"Response: {requestContext.Response}");

static Func<RequestContext, Task> BuildPipeline
(
    List<Func<Func<RequestContext, Task>, Func<RequestContext, Task>>> middlewareList,
    IReadOnlyDictionary<string, Func<RequestContext, Task>?> routeDict
)
{
    Func<RequestContext, Task> finalFunction = async context =>
    {
        if (context.RequestUrl != null && routeDict.TryGetValue(context.RequestUrl, out var routeFunction))
        {
            await routeFunction?.Invoke(context)!;
        }
        else
        {
            context.Response = "404 Not Found";
        }
    };

    foreach (var middleware in middlewareList)
    {
        finalFunction = middleware(finalFunction);
    }

    return finalFunction;
}

static Func<RequestContext, Task> LogMiddleware(Func<RequestContext, Task> next)
{
    return async context =>
    {
        await next(context);
    };
}

static Func<RequestContext, Task> ExceptionMiddleware(Func<RequestContext, Task> next)
{
    return async context =>
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    };
}

static async Task IndexRoute(RequestContext context)
{
    Console.WriteLine($"IndexRoute: {context.RequestUrl}");
    context.Response = "Index page";
    await Task.CompletedTask;
}

static async Task AboutRoute(RequestContext context)
{
    Console.WriteLine($"AboutRoute: {context.RequestUrl}");
    context.Response = "About page";
    await Task.CompletedTask;
}

class RequestContext
{
    public string? RequestUrl { get; init; }
    public string? Response { get; set; }
}