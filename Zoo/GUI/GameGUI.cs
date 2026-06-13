using System.Numerics;
using System.Threading.Tasks;
using Raylib_cs;
using Zoo.GUI;

namespace Zoo;

public class GameGUI
{
    private GameController controller;
    private GUIState state = new GUIState();

    public GameGUI(GameController controller) => this.controller = controller;

    public void AddPopup(string message)
    {
        state.PopupQueue.Enqueue(message);
    }

    public void Start() => Task.Run(RunLoop);

    private void RunLoop()
    {
        int monitor = Raylib.GetCurrentMonitor();
        int width = Raylib.GetMonitorWidth(monitor);
        int height = Raylib.GetMonitorHeight(monitor);

        Raylib.InitWindow(800, 600, "Zoo");
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
                TopUIRenderer.Draw(screenHeight, state);
                MapRenderer.Draw(controller, screenWidth, screenHeight, mousePos, isClicked, state);
                RightPanelRenderer.Draw(screenWidth, screenHeight, mousePos, isClicked, state);
                ConsoleRenderer.Draw(controller, screenWidth, screenHeight);
                ContextMenuRenderer.Draw(controller, screenWidth, screenHeight, mousePos, isClicked, state);
            }
            else if (state.CurrentViewMode == ViewMode.HabitatView)
            {
                HabitatRenderer.Draw(controller, screenWidth, screenHeight, mousePos, isClicked, state);
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