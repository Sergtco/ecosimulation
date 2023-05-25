using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
// using Microsoft.Xna.Framework.Input;
// using MonoGame.Spritesheet;
using simulation.Input;
using simulation.Display;
// using System;

namespace simulation;

public static class Cursor
{
    enum State
    {
        Idle,
        Grab,
        Hover
    }
    static private State state = State.Idle;
    static private float scale = DisplayManager.SCALE ;
    static public Object grabObject = null;
    static private Vector2 size;
    static private Texture2D idleTexture;
    static private Texture2D grabTexture;
    static private Vector2 position;
    static private Game game;
    static public void init(Game gam)
    {
        game = gam;
        position = InputManager.MouseState.Position.ToVector2();
        idleTexture = game.Content.Load<Texture2D>("sprites/cursor/back/back1");
        grabTexture = game.Content.Load<Texture2D>("sprites/cursor/back/back21");
        size = new Vector2(idleTexture.Width, idleTexture.Height) * scale;
    }
    static public void Update()
    {
        position = (InputManager.MouseState.Position.ToVector2() - size/2);
        if (InputManager.leftHold) {
            state = State.Grab;
        } else {
            state = State.Idle;
        }
    }
    static public void Draw(SpriteBatch spriteBatch)
    {
        Texture2D texture = idleTexture;
        switch (state)
        {
            case State.Idle:
                texture = idleTexture;
                break;
            case State.Grab:
                texture = grabTexture;
                break;
        }
        spriteBatch.Draw(texture, position, texture.Bounds, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
    }
}
