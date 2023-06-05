using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using simulation.Input;
using simulation.Display;
using simulation.States;
// using System;
using System.Collections.Generic;

namespace simulation;
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    public List<Object> objects = new List<Object>();


    public GameState _currState;
    public GameState _nextState = null;
    public GameState _buffState;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = DisplayManager.width;
        _graphics.PreferredBackBufferHeight = DisplayManager.height;

        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        Cursor.init(this);
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _currState = new MenuState(this);
        _currState.LoadContent();

    }

    protected override void Update(GameTime gameTime)
    {

        InputManager.Update();
        Cursor.Update();
        if (_nextState != null) {
            _currState = _nextState;
            _nextState = null;
        }
        _currState.Update(gameTime);

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Green);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _currState.Draw(gameTime, _spriteBatch);
        Cursor.Draw(_spriteBatch);
        _spriteBatch.End();

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
    public List<Object> getObjects() {
        return objects;
    }
}
