using System.Collections.Generic;
using System.Collections.Immutable;
using Raylib_cs;
using Zoo.Commands;
using Zoo.Animals;
using Zoo.GameEvents;

namespace Zoo.GUI;

public enum ViewMode { MainMap, HabitatView }

public record GUIState
{
    public ViewMode CurrentViewMode { get; init; } = ViewMode.MainMap;
    public string InputBuffer { get; init; } = "";
    public string StatusMessage { get; init; } = "Wybierz akcje";
    public bool KeepRunning { get; init; } = true;
    public bool ClickHandled { get; init; } = false;
    public int CurrentTurn { get; init; } = 1;
    public int TotalTurns { get; init; } = 1;
    public int SelectedX { get; init; } = -1;
    public int SelectedY { get; init; } = -1;
    public int HoveredX { get; init; } = -1;
    public int HoveredY { get; init; } = -1;
    public LocationHabitat? SelectedHabitat { get; init; }
    public Animal? SelectedAnimal { get; init; }
    public bool IsContextMenuOpen { get; init; } = false;
    public float ContextMenuX { get; init; } = 0;
    public float ContextMenuY { get; init; } = 0;
    public Rectangle ContextMenuRect { get; init; } = new Rectangle(0, 0, 0, 0);
    public Command? SelectedCommand { get; init; }
    public bool IsSubMenuOpen { get; init; } = false;
    public Rectangle SubMenuRect { get; init; } = new Rectangle(0, 0, 0, 0);
    public ImmutableList<string> CurrentSubOptions { get; init; } = ImmutableList<string>.Empty;
    public bool IsLeftMenuOpen { get; init; } = false;
    public float LeftMenuX { get; init; }
    public float LeftMenuY { get; init; }
    public Rectangle LeftMenuRect { get; init; }
    public Command? LeftSelectedCommand { get; init; }
    public bool LeftOptionsReady { get; init; } = false;
    public bool IsLeftSubMenuOpen { get; init; } = false;
    public Rectangle LeftSubMenuRect { get; init; }
    public ImmutableList<string> LeftSubOptions { get; init; } = ImmutableList<string>.Empty;
    public ImmutableList<string> LeftPendingArgs { get; init; } = ImmutableList<string>.Empty;
    public bool IsPopupOpen { get; init; } = false;
    public string CurrentPopupMessage { get; init; } = "";
    public ImmutableQueue<string> PopupQueue { get; init; } = ImmutableQueue<string>.Empty;

    public GUIState SetStatus(string message) => this with { StatusMessage = message };
    public GUIState SetClickHandled(bool handled) => this with { ClickHandled = handled };
    public GUIState ShutdownGame() => this with { KeepRunning = false };
    public GUIState UpdateHoveredTile(int x, int y) => this with { HoveredX = x, HoveredY = y };
    public GUIState SetTurn(int turn) => this with { CurrentTurn = turn };
    public GUIState SetTotalTurns(int total) => this with { TotalTurns = total };

    public GUIState SwitchView(ViewMode newMode) => (this with
    {
        CurrentViewMode = newMode,
        SelectedAnimal = newMode == ViewMode.MainMap ? null : SelectedAnimal
    }).CloseAllMenus();

    public GUIState SelectTile(int x, int y, LocationHabitat? habitat = null) => this with
    {
        SelectedX = x,
        SelectedY = y,
        SelectedHabitat = habitat
    };

    public GUIState SelectAnimal(Animal animal, float mouseX, float mouseY) => this with
    {
        SelectedAnimal = animal,
        IsContextMenuOpen = true,
        ContextMenuRect = new Rectangle(mouseX, mouseY, 220, 150),
        ClickHandled = true
    };

    public GUIState AppendInputChar(char c) => this with { InputBuffer = this.InputBuffer + c };
    public GUIState BackspaceInput() => this.InputBuffer.Length > 0 ? this with { InputBuffer = this.InputBuffer.Remove(this.InputBuffer.Length - 1) } : this;
    public GUIState ClearInputBuffer() => this with { InputBuffer = "" };

    public GUIState OpenContextMenu(float mouseX, float mouseY, int screenWidth, int screenHeight, int commandCount)
    {
        int itemHeight = 30;
        int menuHeight = (commandCount * itemHeight) + 10;
        
        float menuX = mouseX;
        float menuY = mouseY;
        
        if (menuX + 220 > screenWidth) menuX = screenWidth - 220 - 5;
        if (menuY + menuHeight > screenHeight) menuY = screenHeight - menuHeight - 5;
        
        return this with 
        { 
            IsContextMenuOpen = true, 
            ContextMenuX = mouseX, 
            ContextMenuY = mouseY, 
            ContextMenuRect = new Rectangle(menuX, menuY, 220, menuHeight) 
        };
    }

    public GUIState OpenSubMenu(List<string> options, Command command, int screenWidth, int screenHeight)
    {
        int subMenuHeight = options.Count * 30 + 10;
        float subMenuX = ContextMenuRect.X + ContextMenuRect.Width;
        float subMenuY = ContextMenuRect.Y;
        if (subMenuX + 200 > screenWidth) subMenuX = ContextMenuRect.X - 200;
        if (subMenuY + subMenuHeight > screenHeight) subMenuY = screenHeight - subMenuHeight - 5;
        return this with { IsSubMenuOpen = true, SelectedCommand = command, ClickHandled = true, CurrentSubOptions = options.ToImmutableList(), SubMenuRect = new Rectangle(subMenuX, subMenuY, 200, subMenuHeight) };
    }

    public GUIState OpenLeftMenu(float mouseX, float mouseY, Command command) => this with { IsLeftMenuOpen = true, LeftSelectedCommand = command, LeftMenuX = mouseX, LeftMenuY = mouseY, LeftOptionsReady = false };

    public GUIState SetupLeftSubMenu(List<string> options, int screenWidth, int screenHeight)
    {
        int subMenuHeight = options.Count * 30 + 10;
        float subMenuX = LeftMenuX;
        float subMenuY = LeftMenuY;
        if (subMenuX + 200 > screenWidth) subMenuX = screenWidth - 200 - 5;
        if (subMenuY + subMenuHeight > screenHeight) subMenuY = screenHeight - subMenuHeight - 5;
        Rectangle newRect = new Rectangle(subMenuX, subMenuY, 200, subMenuHeight);
        return this with { LeftSubOptions = options.ToImmutableList(), LeftOptionsReady = true, IsLeftSubMenuOpen = true, LeftSubMenuRect = newRect, LeftMenuRect = newRect };
    }

    public GUIState AddLeftPendingArgument(string arg) => this with { LeftPendingArgs = LeftPendingArgs.Add(arg) };
    public GUIState UpdateLeftSubOptions(List<string> newOptions) => this with { LeftSubOptions = newOptions.ToImmutableList(), LeftOptionsReady = true, ClickHandled = true };

    public GUIState CloseAllMenus() => this with
    {
        InputBuffer = "", SelectedX = -1, SelectedY = -1, IsContextMenuOpen = false, IsSubMenuOpen = false, SelectedCommand = null, CurrentSubOptions = ImmutableList<string>.Empty,
        ContextMenuRect = new Rectangle(0, 0, 0, 0), SubMenuRect = new Rectangle(0, 0, 0, 0), IsLeftMenuOpen = false, IsLeftSubMenuOpen = false, LeftOptionsReady = false,
        LeftSelectedCommand = null, LeftSubOptions = ImmutableList<string>.Empty, LeftPendingArgs = ImmutableList<string>.Empty, LeftMenuRect = new Rectangle(0, 0, 0, 0),
        LeftSubMenuRect = new Rectangle(0, 0, 0, 0), ClickHandled = true
    };

    public GUIState EnqueuePopup(string message) { var nextQueue = PopupQueue.Enqueue(message); return (this with { PopupQueue = nextQueue }).TryDisplayNextPopup(); }

    private GUIState TryDisplayNextPopup()
    {
        if (!IsPopupOpen && !PopupQueue.IsEmpty)
        {
            var nextQueue = PopupQueue.Dequeue(out string nextMessage);
            return this with { IsPopupOpen = true, CurrentPopupMessage = nextMessage, PopupQueue = nextQueue };
        }
        return this;
    }

    public GUIState ClosePopup() => (this with { IsPopupOpen = false, CurrentPopupMessage = "" }).TryDisplayNextPopup();

    public bool IsDecisionOpen { get; init; } = false;
    public PendingDecision? CurrentDecision { get; init; }
    public ImmutableQueue<PendingDecision> DecisionQueue { get; init; } = ImmutableQueue<PendingDecision>.Empty;

    public GUIState EnqueueDecision(PendingDecision decision)
    {
        var nextQueue = DecisionQueue.Enqueue(decision);
        return (this with { DecisionQueue = nextQueue }).TryDisplayNextDecision();
    }

    private GUIState TryDisplayNextDecision()
    {
        if (!IsDecisionOpen && !DecisionQueue.IsEmpty)
        {
            var nextQueue = DecisionQueue.Dequeue(out var next);
            return this with { IsDecisionOpen = true, CurrentDecision = next, DecisionQueue = nextQueue };
        }
        return this;
    }

    public GUIState ResolveDecision(bool accepted)
    {
        if (CurrentDecision != null)
        {
            if (accepted) CurrentDecision.OnYes();
            else CurrentDecision.OnNo();
        }
        return (this with { IsDecisionOpen = false, CurrentDecision = null }).TryDisplayNextDecision();
    }

}