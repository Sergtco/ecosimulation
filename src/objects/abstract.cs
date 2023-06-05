using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using simulation.Shapes;
using simulation.Input;
using simulation.Display;


namespace simulation;
public enum Type
{
    Herbivore,
    Carnivore,
}

public enum State
{
    Idle,
    Walk,
    Danger,
    Howl,
    Hunt,
    Attack,
    Dead,
    Grabbed
}

public abstract class Object
{
    protected Game1 game;

    public abstract State CurrState { get;}

    public abstract float StateTime { get; }

    public abstract Vector2 HitboxOffset { get; }


    public Vector2 Position { get { return position; } }
    protected Vector2 position;

    public Vector2 TexturePosition { get { return texturePosition; } }
    protected Vector2 texturePosition;

    public abstract Vector2 Size { get; }

    public abstract float Scale {get; }

    public Point Center { get { return Hitbox.Center; } }
    public abstract Rectangle Hitbox { get; }

    public abstract Vector2 Speed { get; }

    public Object(Game1 game)
    {
        this.game = game;
        texturePosition = Vector2.Zero;
        position = texturePosition;
        LoadContent();
    }

    protected abstract void LoadContent();
    public abstract void changeState(State newState);
    protected abstract void chagePosition(float elapsedSeconds);
    protected void grab()
    {
        if (CurrState == State.Dead) return;
        if (InputManager.leftHold && InputManager.Hover(Hitbox) && (Cursor.grabObject == null || Cursor.grabObject == this) || InputManager.leftHold && Cursor.grabObject == this)
        {
            Cursor.grabObject = this;
            this.position = InputManager.MouseState.Position.ToVector2() - Size / 2;
            changeState(State.Grabbed);
        }
        else if (!InputManager.Hover(Hitbox) && Cursor.grabObject == this)
        {
            Cursor.grabObject = null;
            changeState(State.Idle);
        }

    }

    public abstract void Update(GameTime gameTime);
    public abstract void Draw(SpriteBatch spriteBatch);
}

public abstract class Animal : Object
{
    public abstract Type Type { get; }

    public abstract Animal Hunter { get; }

    public abstract float Hunger { get; }

    public abstract Circle VisionCircle { get; }
    protected Circle visionCircle;

    public override Vector2 Speed { get { return speed; } }
    protected Vector2 speed;

    public override Rectangle Hitbox { get { return hitbox; } }
    protected Rectangle hitbox;


    public Animal(Game1 game) : base(game) { }
    public abstract void isPrey(Animal hunter);
    protected abstract void search();

    protected override void chagePosition(float elapsedSeconds)
    {
        position += speed * elapsedSeconds;
        if (!DisplayManager.screen.Contains(new Rectangle(position.ToPoint(), Size.ToPoint())))
        {
            position -= speed * elapsedSeconds;
            // move(pos: DisplayManager.screen.Center.ToVector2(), speed: speed);
            speed *= -1;
        }


        texturePosition = position - HitboxOffset;
        hitbox = new Rectangle(position.ToPoint(), (Size).ToPoint());
        visionCircle = new Circle(80 * Scale, hitbox.Center.ToVector2());
    }
    protected void move(Vector2? pos = null, Vector2? speed = null)
    {
        if (speed != null)
        {
            this.speed = (Vector2)speed;
        }
        if (pos != null)
        {
            Vector2 direction = (Vector2)pos - position;
            direction.Normalize();
            this.speed.X *= direction.X;
            this.speed.Y *= direction.Y;
        }
    }
    protected void godKill() {
        if (InputManager.middleClicked && InputManager.Hover(hitbox)) {
            changeState(State.Dead);
        }
    }

}
