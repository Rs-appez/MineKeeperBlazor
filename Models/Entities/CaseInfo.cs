namespace blazorEx.Models.Entities;

public class CaseInfo
{
    public (int x, int y) Position { get; set; }
    public bool IsOpen { get; set; }
    public bool HasMine { get; set; }
    public bool IsDefused { get; set; }
    public bool IsFlagged { get; set; }
    public int AdjacentMines { get; set; }

    public CaseInfo((int, int) position)
    {
        Position = position;
        IsOpen = false;
        IsDefused = false;
        HasMine = false;
        AdjacentMines = 0;
    }
}
