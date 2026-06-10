using System.Collections.Generic;
using Raylib_cs;

namespace Zoo.GUI;

public class GUIState
{
    public string InputBuffer { get; set; } = "";
    public string StatusMessage { get; set; } = "Wybierz akcje";
    public int SelectedX { get; set; } = -1;
    public int SelectedY { get; set; } = -1;
    public int HoveredX { get; set; } = -1;
    public int HoveredY { get; set; } = -1;
    public bool KeepRunning { get; set; } = true;
    public bool IsContextMenuOpen { get; set; } = false;
    public float ContextMenuX { get; set; } = 0;
    public float ContextMenuY { get; set; } = 0;
    public Rectangle ContextMenuRect { get; set; } = new Rectangle(0, 0, 0, 0);
    public bool ClickHandled { get; set; } = false;
    public bool IsSubMenuOpen { get; set; } = false;
    public Rectangle SubMenuRect { get; set; } = new Rectangle(0, 0, 0, 0);
    public dynamic SelectedCommand { get; set; } = null;
    public List<string> CurrentSubOptions { get; set; } = new List<string>();
}