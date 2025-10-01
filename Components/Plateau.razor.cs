using blazorEx.Messages;
using blazorEx.Models.Entities;
using blazorEx.Tools;

namespace blazorEx.Components;
public partial class Plateau : IDisposable
{
    private int Rows { get; set; } = 10;
    private int NumberOfMines { get; set; } = 10;
    private int ClearedCells { get; set; } = 0;
    private bool GameOver { get; set; } = false;
    private bool GameWin { get; set; } = false;

    private Dictionary<(int x, int y), CaseInfo> Cases { get; set; } = new();
    private List<(int x, int y)> Mines = new();

    protected override void OnInitialized()
    {
        Mediator<ClickMessage>.Instance.Subscribe(OnClick);
        InitGame();
    }
    private void InitGame()
    {
        GameOver = false;
        GameWin = false;
        ClearedCells = 0;
        Mines.Clear();

        GenerateCells();
    }

    private void GenerateCells()
    {
        Cases.Clear();
        var rand = new Random();
        for (int x = 0; x < Rows; x++)
        {
            for (int y = 0; y < Rows; y++)
            {
                Cases[(x, y)] = new CaseInfo((x, y));
            }
        }
    }
    private void PlaceMines((int,int) firstClickPosition)
    {
        var rand = new Random();
        while (Mines.Count < NumberOfMines)
        {
            var position = (rand.Next(Rows), rand.Next(Rows));
            if (!Mines.Contains(position) && position != firstClickPosition)
            {
                Mines.Add(position);
                Cases[position].HasMine = true;
            }
        }
    }
    private void CountAdjacentMines()
    {
        foreach (var mine in Mines)
        {
            DoOnAdjacentCells(mine, cell =>
            {
                cell.AdjacentMines++;
            });
        }

    }
    private void OnClick(ClickMessage message)
    {
        if (Mines.Count == 0)
        {
            PlaceMines(message.Position);
            CountAdjacentMines();
        }
        CaseInfo clickedCell = Cases[message.Position];
        if (CheckDeath(clickedCell))
        {
            StateHasChanged();
            return;
        }

        ClearedCells++;
        if (clickedCell.AdjacentMines == 0) OpenRecursively(message.Position);
        CheckWin();
        StateHasChanged();
    }

    private bool CheckDeath(CaseInfo caseInfo)
    {
        if (!caseInfo.HasMine) return false;

        GameOver = true;
        foreach (var mines in Mines)
        {
            Cases[mines].IsOpen = true;
        }
        return true;
    }

    private void CheckWin()
    {
        if (ClearedCells == Cases.Count - Mines.Count)
        {
            GameOver = true;
            GameWin = true;
            foreach (var mines in Mines)
            {
                Cases[mines].IsOpen = true;
                Cases[mines].IsDefused = true;

            }
        }
    }

    private void OpenRecursively((int x, int y) position)
    {
        DoOnAdjacentCells(position, cell =>
        {
            if (!cell.IsOpen && !cell.IsFlagged)
            {
                cell.IsOpen = true;
                ClearedCells++;
                if (cell.AdjacentMines == 0 && !cell.HasMine)
                {
                    OpenRecursively(cell.Position);
                }
            }
        });
    }

    private void DoOnAdjacentCells((int x, int y) position, Action<CaseInfo> action)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue; // Skip the cell itself
                var neighbor = (position.x + dx, position.y + dy);
                if (Cases.ContainsKey(neighbor))
                {
                    action(Cases[neighbor]);
                }
            }
        }
    }

    public void Dispose()
    {
        Mediator<ClickMessage>.Instance.Unsubscribe(OnClick);
    }
}
