using blazorEx.Messages;
using blazorEx.Models.Entities;
using blazorEx.Tools;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace blazorEx.Components;
public partial class Cell
{
    [Parameter]
    public CaseInfo? CurrentCase { get; set; }
    [Parameter]
    public EventCallback<(int,int)> OnCellClick { get; set; }

    private void FlagCell(MouseEventArgs e)
    {
        if (CurrentCase is not null)
        {
            if (!CurrentCase.IsOpen)
                CurrentCase.IsFlagged = !CurrentCase.IsFlagged;
        }
    }
    private void OpenCell()
    {
        if (CurrentCase is not null)
        {
            if (!CurrentCase.IsFlagged && !CurrentCase.IsOpen)
            {
                CurrentCase.IsOpen = true;
                // Mediator<ClickMessage>.Instance.Notify(new ClickMessage(CurrentCase.Position));
                OnCellClick.InvokeAsync(CurrentCase.Position);
            }
        }
    }
}
