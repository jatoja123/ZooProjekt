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
    private GUIState state;
    private readonly InputHandler inputHandler;
    private readonly AssetLoader assetLoader;

    private readonly List<IRenderer> mainMapRenderers;
    private readonly List<IRenderer> habitatRenderers;
    private readonly PopupRenderer popupRenderer;
    private readonly DecisionPopupRenderer decisionPopupRenderer;
    private readonly GameEndMessage gameEndMessageRenderer;

    public static ConcurrentQueue<Func<GUIState, GUIState>> StateDispatches = new();

    public static void EnqueuePopup(string message)
    {
        StateDispatches.Enqueue(s => s.EnqueuePopup(message));
    }

    // Backward-compatible alias for existing references with older casing.
    public static void Enqueuepopup(string message)
    {
        EnqueuePopup(message);
    }

    public GameGUI(IMenuStrategy mainMapStrategy, IMenuStrategy habitatStrategy)
    {
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
        decisionPopupRenderer = new DecisionPopupRenderer();
        gameEndMessageRenderer = new GameEndMessage();
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

    public void ShowGameEndScreen()
    {
        StateDispatches.Enqueue(s => s.ShowGameEndScreen());
    }

    public Task Start() => Task.Run(RunLoop);

    private void RunLoop()
    {
        Raylib.SetConfigFlags((ConfigFlags)(1024 | 8 | 64));
        Raylib.InitWindow(1800, 1000, "Zoo");
        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose() && state.KeepRunning)
        {
            var controller = GameController.Instance;
            while (StateDispatches.TryDequeue(out var updateFunc))
            {
                state = updateFunc(state);
            }

            int screenWidth = Raylib.GetScreenWidth();
            int screenHeight = Raylib.GetScreenHeight();
            Vector2 mousePos = Raylib.GetMousePosition();
            bool isClicked = Raylib.IsMouseButtonPressed(MouseButton.Left);

            state = state.SetClickHandled(false);

            if (!state.IsPopupOpen && !state.IsGameEndScreenVisible)
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

            if (state.IsGameEndScreenVisible)
            {
                state = gameEndMessageRenderer.Draw(controller, screenWidth, screenHeight, mousePos, isClicked, state);
            }
            else
            {
                if (state.IsDecisionOpen)
                {
                    state = decisionPopupRenderer.Draw(controller, screenWidth, screenHeight, mousePos, isClicked, state);
                }
                else if (state.IsPopupOpen)
                {
                    state = popupRenderer.Draw(controller, screenWidth, screenHeight, mousePos, isClicked, state);
                }
            }

            Raylib.EndDrawing();
        }

        assetLoader.UnloadAllTextures();
        Raylib.CloseWindow();
    }
}
