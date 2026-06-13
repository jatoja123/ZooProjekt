using System.Collections.Generic;
using Raylib_cs;
using Zoo.Commands;
using Zoo;
using Zoo.Animals;

namespace Zoo.GUI;

public enum ViewMode
{
    MainMap,
    HabitatView
}

public class GUIState
{
    public ViewMode CurrentViewMode { get; set; } = ViewMode.MainMap;
    public string InputBuffer { get; set; } = "";
    public string StatusMessage { get; set; } = "Wybierz akcje";
    public int SelectedX { get; set; } = -1;
    public int SelectedY { get; set; } = -1;
    public int HoveredX { get; set; } = -1;
    public int HoveredY { get; set; } = -1;
    public LocationHabitat? SelectedHabitat { get; set; }
    public Command? SelectedCommand { get; set; }
    public Animal? SelectedAnimal { get; set; }
    public bool KeepRunning { get; set; } = true;
    public bool IsContextMenuOpen { get; set; } = false;
    public float ContextMenuX { get; set; } = 0;
    public float ContextMenuY { get; set; } = 0;
    public Rectangle ContextMenuRect { get; set; } = new Rectangle(0, 0, 0, 0);
    public bool ClickHandled { get; set; } = false;
    public bool IsSubMenuOpen { get; set; } = false;
    public Rectangle SubMenuRect { get; set; } = new Rectangle(0, 0, 0, 0);
    public List<string> CurrentSubOptions { get; set; } = new List<string>();

    public Queue<string> PopupQueue { get; set; } = new Queue<string>();
    public bool IsPopupOpen { get; set; } = false;
    public string CurrentPopupMessage { get; set; } = "";
}