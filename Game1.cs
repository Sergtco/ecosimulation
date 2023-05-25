using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using simulation.Input;
using simulation.Display;
// using System;
using System.Collections.Generic;

namespace simulation;
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    public List<Object> objects = new List<Object>();

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
        for (int i = 0; i < 100; i++)
        {
            System.Threading.Thread.Sleep(10);
            int x = Rand.get(0, _graphics.PreferredBackBufferWidth - 100);
            int y = Rand.get(0, _graphics.PreferredBackBufferHeight - 100);
            Bunny bunny = new Bunny(this, new Vector2(x, y));
            objects.Add(bunny);
        }
        for (int i = 0; i < 1; i++)
        {
            System.Threading.Thread.Sleep(10);
            int x = Rand.get(0, _graphics.PreferredBackBufferWidth - 100);
            int y = Rand.get(0, _graphics.PreferredBackBufferHeight - 100);
            Wolf wolf = new Wolf(this, new Vector2(x, y));
            objects.Add(wolf);
        }

    }

    protected override void Update(GameTime gameTime)
    {

        InputManager.Update();
        Cursor.Update();

        for (int i = 0; i < objects.Count; i++) {
            objects[i].Update(gameTime);
            if ((objects[i]).CurrState == State.Dead && (objects[i]).StateTime >= 15) {
                objects.RemoveAt(i);
            }
        }
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Green);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        foreach (Animal animal in objects)
        {
            animal.Draw(_spriteBatch);
        }
        Cursor.Draw(_spriteBatch);
        _spriteBatch.End();

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
    public List<Object> getObjects() {
        return objects;
    }
}
