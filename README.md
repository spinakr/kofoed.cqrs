# kofoed.cqrs

Simple  CQRS library showcasing how CQRS and event sourcing can be set up. 


##Setup in dotnet core application


### Command/query handler using mediator
Command and query handlers are implemented using a simple mediator pattern. All commands and query implementing `IQueryHandler` or `ICommandHandler` will be invoked if the `AddHandlers` extension has been invoked on the service provider.

```
//in ConfigureServices in Startup.cs 
services.AddHandlers(typeof(Assembly.GetExecutingAssembly()).Assembly);
services.AddSingleton<IMessaging, Messaging>();

//example command and handler
public class CreateAccountCommand : ICommand
{
    public Guid CustomerId { get; set; }
    public string Name { get; set; }
}

public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand>
{
    public Result Handle(CreateAccountCommand cmd)
    {
        var newAccount = Account.CreateNew(cmd.Name);
        return Result.Complete(newAccount.Id);
    }
}

//in controller 
public AccountsController(IMessaging messaging)
{
    _messaging = messaging;

}

[HttpPost("{customerId}/accounts")]
public IActionResult CreateAccount(Guid customerId, [FromBody]dynamic req)
{
    var result = _messaging.Dispatch(new CreateAccountCommand(customerId, (string)req.name));

    return Ok(((Result<Guid>)result).Value);
}
```


### Register and use event store

services.AddHandlers(typeof(AccountsOverviewQueryHandler).Assembly);
services.AddSingleton<IEventStore, EventStore>();
services.AddSingleton<IAppendOnlyStore>(new FileAppendOnlyStore("bankaccounts"));
services.AddSingleton<IProjectionStore<Guid, AccountsOverview>, FileProjectionStore<Guid, AccountsOverview>>();