namespace blazorEx.Tools;
public class Mediator<TMessage>
{
    
    public static Mediator<TMessage> Instance { get; } = new Mediator<TMessage>();

    private Mediator() { }

    private Action<TMessage>? _actions;

    public void Subscribe(Action<TMessage> action) => _actions += action;
    public void Unsubscribe(Action<TMessage> action) => _actions -= action;
    public void Notify(TMessage message) => _actions?.Invoke(message);
}
