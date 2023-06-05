using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace simulation.Input;

public static class InputManager
{
    public static bool leftClicked = false;
    public static bool leftHold = false;
    public static bool middleClicked= false;
    private static MouseState mouseState = new MouseState(), prevMouseState;
    static KeyboardState kbState;
    static KeyboardState prevKbState;
    public static MouseState MouseState { get => mouseState; set { } }
    public static void Update()
    {
        prevMouseState = mouseState;
        mouseState = Mouse.GetState();
        prevKbState = kbState;
        kbState = Keyboard.GetState();

        leftClicked = mouseState.LeftButton != ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Pressed;
        leftHold = mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Pressed;
        middleClicked = mouseState.MiddleButton != ButtonState.Pressed && prevMouseState.MiddleButton == ButtonState.Pressed;
    }
    public static bool Hover(Rectangle rect)
    {
        return rect.Contains(mouseState.Position);
    }
    public static bool backClicked() {
        if (kbState.IsKeyDown(Keys.Back) && !prevKbState.IsKeyDown(Keys.Back)) {
            return true;
        }
        return false;
    }
    public static string getInput()
    {
        var keys = kbState.GetPressedKeys();
        bool shiftPressed = kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift);
        if (keys.Length > 0)
        {
            foreach (Keys key in keys)
            {
                string str = key.ToString();
                if (str.Length == 1 || str.Length == 2 && str[0] == 'D')
                {
                    if (!prevKbState.IsKeyDown(key))
                    {
                        if (str.Length == 2)
                        {
                            str = str[1].ToString();
                        }
                        if (shiftPressed)
                        {
                            return str.ToUpper();
                        }
                        return str;
                    }
                }
            }
        }
        return null;
    }
}
