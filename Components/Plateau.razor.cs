using blazorEx.Models.Entities;

namespace blazorEx.Components;

public partial class Plateau
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
    private void PlaceMines((int, int) firstClickPosition)
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
    private void OnClick((int, int) position)
    {
        if (Mines.Count == 0)
        {
            PlaceMines(position);
            CountAdjacentMines();
        }
        CaseInfo clickedCell = Cases[position];
        if (CheckDeath(clickedCell))
        {
            StateHasChanged();
            return;
        }

        ClearedCells++;
        if (clickedCell.AdjacentMines == 0) OpenRecursively(position);
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
        var queue = new Queue<(int x, int y)>();
        queue.Enqueue(position);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            DoOnAdjacentCells(current, cell =>
            {
                if (!cell.IsOpen && !cell.IsFlagged)
                {
                    cell.IsOpen = true;
                    ClearedCells++;

                    if (cell.AdjacentMines == 0 && !cell.HasMine)
                    {
                        queue.Enqueue(cell.Position);
                    }
                }
            });
        }
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

}
