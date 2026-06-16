using System.Numerics;
using System.Threading.Tasks;
using Raylib_cs;
using Zoo.GUI;

namespace Zoo;

public class GameGUI : IObserver
{
    private GameController controller;
    private GUIState state = new GUIState();

    private int lastSavedTurn = 0;
    private int maxTurns = 0;

    public GameGUI(GameController controller) => this.controller = controller;

    public void AddPopup(string message)
    {
        state.PopupQueue.Enqueue(message);
    }

    public void Start() => Task.Run(RunLoop);

    public void ReceiveEvent(NotifyEvent notifyEvent)
    {
        if (notifyEvent is TurnEvent turnEvent)
        {
            lastSavedTurn = turnEvent.Turn;
        }
        else if (notifyEvent is GameStartEvent gameStartEvent)
        {
            maxTurns = gameStartEvent.TotalTurns;
        }
    }

    private void RunLoop()
    {
        Raylib.InitWindow(1800, 1000, "Zoo");
        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose() && state.KeepRunning)
        {
            int screenWidth = Raylib.GetScreenWidth();
            int screenHeight = Raylib.GetScreenHeight();
            Vector2 mousePos = Raylib.GetMousePosition();
            bool isClicked = Raylib.IsMouseButtonPressed(MouseButton.Left);

            state.ClickHandled = false;

            if (!state.IsPopupOpen && state.PopupQueue.Count > 0)
            {
                state.IsPopupOpen = true;
                state.CurrentPopupMessage = state.PopupQueue.Dequeue();
            }

            if (!state.IsPopupOpen)
            {
                InputHandler.HandleKeyboard(state);
            }

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RayWhite);

            if (state.CurrentViewMode == ViewMode.MainMap)
            {
                TopUIRenderer.Draw(controller, screenHeight, state, lastSavedTurn, maxTurns);
                MapRenderer.Draw(controller, screenWidth, screenHeight, mousePos, isClicked, state);
                RightPanelRenderer.Draw(screenWidth, screenHeight, mousePos, isClicked, state);
                ConsoleRenderer.Draw(controller, 20, screenHeight - 440, screenWidth - 350, 300, false);
                ContextMenuRenderer.Draw(controller, screenWidth, screenHeight, mousePos, isClicked, state);
                LeftContextMenu.Draw(controller, screenWidth, screenHeight, mousePos, isClicked, state);
            }
            else if (state.CurrentViewMode == ViewMode.HabitatView)
            {
                HabitatRenderer.Draw(controller, screenWidth, screenHeight, mousePos, isClicked, state);
                ConsoleRenderer.Draw(controller, 20, screenHeight - 340, screenWidth - 40, 320, false);
                ContextMenuRenderer.Draw(controller, screenWidth, screenHeight, mousePos, isClicked, state);
            }

            if (state.IsPopupOpen)
            {
                PopupRenderer.Draw(screenWidth, screenHeight, mousePos, isClicked, state);
            }

            Raylib.EndDrawing();
        }

        AssetLoader.UnloadAllTextures();
        Raylib.CloseWindow();
    }
}