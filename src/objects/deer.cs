using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
// using Microsoft.Xna.Framework.Input;
using MonoGame.Spritesheet;
using simulation.Display;
using simulation.Input;
using simulation.Shapes;
using System;


namespace simulation;
class Deer : Animal
{

    public override State CurrState
    {
        get { return state; }
    }
    State state;
    public override float StateTime { get { return stateTime; } }
    float stateTime;

    public override Type Type { get { return type; } }
    Type type = Type.Herbivore;

    public override Animal Hunter { get { return hunter; } }
    Animal hunter = null;

    public override Vector2 HitboxOffset { get { return hitboxOffset * scale; } }
    Vector2 hitboxOffset = new Vector2(9, 17);

    public override Vector2 Size { get { return size * scale; } }
    Vector2 size = new Vector2(14, 14);
    public override float Scale { get { return scale; } }
    float scale = DisplayManager.SCALE * 2.5f;

    public override Circle VisionCircle { get { return visionCircle; } }

    public override float Hunger { get { return hunger; } }
    float hunger = 75f;

    float time;
    GridSheet sheet;
    SpriteEffects horizontalFlip = SpriteEffects.FlipHorizontally;
    bool dir; // true - right, false - left
    Animation idleAnimation;
    Animation walkAnimation;
    Animation deadAnimation;

    public Deer(Game1 game) : base(game)
    {
        speed = Vector2.Zero;
        chagePosition(1);
    }

    public Deer(Game1 game, Vector2 position) : this(game)
    {
        this.position = position;
        texturePosition = position - hitboxOffset;
        chagePosition(1);
    }

    protected override void LoadContent()
    {
        sheet = game.Content.Load<GridSheet>("sprites/deer/MiniDeer1");
        idleAnimation = new Animation(sheet, 0, 0.3f, 4, true);
        walkAnimation = new Animation(sheet, 1, 0.2f, 4, true);
        deadAnimation = new Animation(sheet, 5, 0.2f, 4, false);
    }


    public override void changeState(State newState)
    {
        switch (newState)
        {
            case State.Idle:
                speed = Vector2.Zero;
                idleAnimation.FrameNum = 0;
                break;
            case State.Walk:
                move(speed: new Vector2(Rand.get(-100, 100), Rand.get(-100, 100)));
                walkAnimation.FrameNum = 0;
                break;
            case State.Danger:
                break;
            case State.Grabbed:
                speed = Vector2.Zero;
                break;
            case State.Dead:
                speed = Vector2.Zero;
                deadAnimation.FrameNum = 0;
                break;
        }
        state = newState;
        stateTime = 0;
    }

    public override void isPrey(Animal hunter)
    {
        if (this.hunter == null)
        {
            this.hunter = hunter;
            changeState(State.Danger);
        }
    }
    protected override void search() { }
    private void run(Animal animal)
    {
        Vector2 dir = position - animal.Position;
        dir.Normalize();
        speed = dir * 150;
    }
    public override void Update(GameTime gameTime)
    {
        time += (float)gameTime.ElapsedGameTime.TotalSeconds;
        stateTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        grab();
        godKill();

        switch (CurrState)
        {
            case State.Idle:
                if (hunger < 50)
                {
                    hunger += 25;
                }
                if (stateTime > 4 && stateTime % 5 < 1 && Rand.get(0, 5) == 1)
                {
                    changeState(State.Walk);
                }
                else if (stateTime > 4 && stateTime % 5 < 1 && Rand.get(0, 10) == 2)
                {
                    changeState(State.Howl);
                }
                break;
            case State.Walk:
                if (stateTime > 3 && stateTime % 3 < 0.1 && Rand.get(0, 5) == 1)
                {

                    changeState(State.Idle);
                }
                break;
            case State.Danger:
                run(hunter);
                break;
        }
        chagePosition((float)gameTime.ElapsedGameTime.TotalSeconds);
        if (speed.X < 0)
        {
            dir = false;
        }
        else if (speed.X > 0)
        {
            dir = true;
        }

        // Console.WriteLine(InputManager.MouseState.Position.ToVector2());
        // Console.WriteLine(hitbox);
        // Console.WriteLine(position);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        Animation animation = animate();
        if (!dir)
        {
            horizontalFlip = SpriteEffects.FlipHorizontally;
        }
        else
        {
            horizontalFlip = SpriteEffects.None;
        }
        spriteBatch.Draw(animation.SpriteSheet.Texture, texturePosition, animation.SpriteSheet[animation.FrameNum, animation.AnimRow], Color.White, 0f, new Vector2(0, 0), scale, horizontalFlip, 1f);
        // spriteBatch.Draw(new Texture2D(spriteBatch.GraphicsDevice, hitbox.Width, hitbox.Height), position, Color.White);
    }
    private Animation animate()
    {
        Animation animation = idleAnimation;
        switch (state)
        {
            case State.Idle:
                animation = idleAnimation;
                break;
            case State.Walk:
                animation = walkAnimation;
                break;
            case State.Danger:
                animation = walkAnimation;
                break;
            case State.Dead:
                animation = deadAnimation;
                break;

        }

        while (time > animation.FrameTime)
        {
            time -= animation.FrameTime;

            if (animation.IsLooping)
            {
                animation.FrameNum++;
            }
            else
            {
                animation.FrameNum = Math.Min(animation.FrameNum + 1, animation.FrameCount - 1);
            }
        }
        return animation;

    }
}

