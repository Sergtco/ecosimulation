using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace simulation.Input;

public static class InputManager {
    public static bool leftClicked = false;
    public static bool leftHold = false;
    private static MouseState mouseState = new MouseState(), prevMouseState;
    public static MouseState MouseState {get => mouseState; set {}}
    public static void Update(){
        prevMouseState = mouseState;
        mouseState = Mouse.GetState();
        leftClicked = mouseState.LeftButton != ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Pressed;
        leftHold = mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Pressed;
    }
    public static bool Hover(Rectangle rect) {
        return rect.Contains(mouseState.Position);
    }
}
