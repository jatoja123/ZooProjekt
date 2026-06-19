using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Raylib_cs;
using Zoo.GameEvents;
using Zoo.GUI;

namespace Zoo;

public class GameGUI : IObserver
{
    private readonly GameController controller;
    private GUIState state;
    private readonly InputHandler inputHandler;
    private readonly AssetLoader assetLoader;

    private readonly List<IRenderer> mainMapRenderers;
    private readonly List<IRenderer> habitatRenderers;
    private readonly PopupRenderer popupRenderer;

    public static ConcurrentQueue<Func<GUIState, GUIState>> StateDispatches = new();

    public GameGUI(GameController controller, IMenuStrategy mainMapStrategy, IMenuStrategy habitatStrategy)
    {
        this.controller = controller;
        this.state = new GUIState();
        this.inputHandler = new InputHandler();
        this.assetLoader = new AssetLoader();

        mainMapRenderers = new List<IRenderer>
        {
            new TopUIRenderer(),
            new MapRenderer(assetLoader),
            new RightPanelRenderer(),
            new ConsoleRenderer(false),
            new ContextMenuRenderer(mainMapStrategy),
            new LeftContextMenu()
        };

        habitatRenderers = new List<IRenderer>
        {
            new HabitatRenderer(assetLoader),
            new ConsoleRenderer(true),
            new ContextMenuRenderer(habitatStrategy)
        };

        popupRenderer = new PopupRenderer();
    }

    public void ReceiveEvent(NotifyEvent notifyEvent)
    {
        if (notifyEvent is TurnEvent turnEvent)
        {
            StateDispatches.Enqueue(s => s.SetTurn(turnEvent.Turn));
        }
        else if (notifyEvent is GameStartEvent gameStartEvent)
        {
            StateDispatches.Enqueue(s => s.SetTotalTurns(gameStartEvent.TotalTurns));
        }
    }

    public void AddPopup(string message)
    {
        StateDispatches.Enqueue(s => s.EnqueuePopup(message));
    }

    public void AddDecision(PendingDecision decision)
    {
        state.DecisionQueue.Enqueue(decision);
    }

    public Task Start() => Task.Run(RunLoop);

    private void RunLoop()
    {
        Raylib.SetConfigFlags((ConfigFlags)(1024 | 8 | 64));
        Raylib.InitWindow(1800, 1000, "Zoo");
        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose() && state.KeepRunning)
        {
            while (StateDispatches.TryDequeue(out var updateFunc))
            {
                state = updateFunc(state);
            }

            int screenWidth = Raylib.GetScreenWidth();
            int screenHeight = Raylib.GetScreenHeight();
            Vector2 mousePos = Raylib.GetMousePosition();
            bool isClicked = Raylib.IsMouseButtonPressed(MouseButton.Left);

            state = state.SetClickHandled(false);

            if (!state.IsPopupOpen)
            {
                state = inputHandler.HandleKeyboard(state);
            }

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RayWhite);

            var activeRenderers = state.CurrentViewMode == ViewMode.MainMap ? mainMapRenderers : habitatRenderers;

            foreach (var renderer in activeRenderers)
            {
                state = renderer.Draw(controller, screenWidth, screenHeight, mousePos, isClicked, state);
            }

            if (state.IsPopupOpen)
            {
                state = popupRenderer.Draw(controller, screenWidth, screenHeight, mousePos, isClicked, state);
            }

            Raylib.EndDrawing();
        }

        assetLoader.UnloadAllTextures();
        Raylib.CloseWindow();
    }
}