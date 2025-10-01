namespace blazorEx.Messages;

public class ClickMessage
{
    public (int x, int y) Position { get; set; }

    public ClickMessage((int, int) position)
    {
        Position = position;
    }
    
}
