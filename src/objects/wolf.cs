using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
// using Microsoft.Xna.Framework.Input;
using MonoGame.Spritesheet;
using simulation.Display;
// using simulation.Input;
using simulation.Shapes;
using System;


namespace simulation;
class Wolf : Animal
{
    public override State CurrState
    {
        get { return state; }
    }
    State state;
    public override float StateTime { get { return stateTime; } }
    float stateTime;

    public override Type Type { get { return type; } }
    Type type = Type.Carnivore;

    public Animal Prey { get { return prey; } }
    Animal prey;

    public override Animal Hunter { get { return hunter; } }
    Animal hunter;

    public override Vector2 HitboxOffset { get { return hitboxOffset * scale; } }
    Vector2 hitboxOffset = new Vector2(8, 20);

    public override Vector2 Size { get { return size * scale; } }
    Vector2 size = new Vector2(17, 11);
    public override float Scale { get { return scale; } }
    float scale = DisplayManager.SCALE * 2.5f;

    public override Circle VisionCircle { get { return visionCircle; } }

    public override float Hunger { get { return hunger; } }
    float hunger = 100f;

    float time;
    GridSheet sheet;
    SpriteEffects horizontalFlip = SpriteEffects.FlipHorizontally;
    bool dir; // true - right, false - left
    Animation idleAnimation;
    Animation howlAnimation;
    Animation walkAnimation;
    Animation attackAnimation;
    Animation runAnimation;
    Animation deadAnimation;

    public Wolf(Game1 game) : base(game)
    {
        speed = Vector2.Zero;
        chagePosition(1);
    }

    public Wolf(Game1 game, Vector2 position) : this(game)
    {
        this.position = position;
        texturePosition = position - hitboxOffset;
        chagePosition(1);
    }

    protected override void LoadContent()
    {
        sheet = game.Content.Load<GridSheet>("sprites/wolf/MiniWolf");
        idleAnimation = new Animation(sheet, 0, 0.3f, 4, true);
        walkAnimation = new Animation(sheet, 1, 0.1f, 6, true);
        howlAnimation = new Animation(sheet, 5, 0.4f, 7, false);
        attackAnimation = new Animation(sheet, 4, 0.2f, 5, false);
        deadAnimation = new Animation(sheet, 7, 0.2f, 3, false);
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
            case State.Hunt:
                move(pos: Prey.Position);
                break;
            case State.Grabbed:
                speed = Vector2.Zero;
                break;
            case State.Howl:
                howlAnimation.FrameNum = 0;
                speed = Vector2.Zero;
                break;
            case State.Dead:
                deadAnimation.FrameNum = 0;
                speed = Vector2.Zero;
                break;
        }
        state = newState;
        stateTime = 0;
    }

    public override void isPrey(Animal hunter)
    {
        this.hunter = hunter;
    }

    protected override void search()
    {
        if (state != State.Grabbed)
        {
            if ((state != State.Hunt) && (hunger < 50))
            {
                foreach (Animal animal in game.getObjects())
                {
                    if (animal.Type == Type.Herbivore && animal.CurrState != State.Dead && visionCircle.Collides(animal.Hitbox))
                    {
                        state = State.Hunt;
                        prey = animal;
                        prey.isPrey(this);
                        move(pos: prey.Position, speed: new Vector2(200, 200));
                    }

                }
            }
        }

    }
    public override void Update(GameTime gameTime)
    {
        time += (float)gameTime.ElapsedGameTime.TotalSeconds;
        hunger -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        stateTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        grab();
        godKill();

        // Console.WriteLine(hunger);
        switch (CurrState)
        {
            case State.Idle:
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
            case State.Howl:
                if (howlAnimation.FrameNum == howlAnimation.FrameCount - 1)
                {
                    changeState(State.Idle);
                }
                break;
            case State.Hunt:
                if (prey.CurrState != State.Dead)
                {
                    move(pos: prey.Position, speed: new Vector2(200, 200));
                    if (attackAnimation.FrameNum == attackAnimation.FrameCount - 1)
                    {
                        attackAnimation.FrameNum = 0;
                    }
                    if (prey.Hitbox.Intersects(hitbox) && attackAnimation.FrameNum == 2)
                    {
                        prey.changeState(State.Dead);
                    }
                }
                else if (attackAnimation.FrameNum == attackAnimation.FrameCount - 1)
                {
                    changeState(State.Idle);
                    hunger += 25;
                    attackAnimation.FrameNum = 0;
                }
                break;
        }
        search();

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
        // spriteBatch.Draw(new Texture2D(spriteBatch.GraphicsDevice, (int)visionCircle.Radius * 2, (int)visionCircle.Radius * 2), visionCircle.Position, Color.White);
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
            case State.Howl:
                animation = howlAnimation;
                break;
            case State.Dead:
                animation = deadAnimation;
                break;
            case State.Hunt:
                animation = walkAnimation;
                if (Vector2.Distance(prey.Hitbox.Center.ToVector2(), hitbox.Center.ToVector2()) < prey.Hitbox.Width)
                {
                    animation = attackAnimation;
                }
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

