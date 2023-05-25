// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using Microsoft.Xna.Framework.Input;
using MonoGame.Spritesheet;

namespace simulation;
class Animation
{
    public GridSheet SpriteSheet
    {
        get { return spriteSheet; }
    }
    GridSheet spriteSheet;

    public int AnimRow
    {
        get { return animRow; }
    }
    int animRow;

    public int FrameNum 
    {
        get {return frameNum;}
        set {
            frameNum = value % frameCount;
        }
    }
    int frameNum;

    public float FrameTime
    {
        get { return frameTime; }
    }
    float frameTime;

    public int FrameCount
    {
        get { return frameCount; }
    }
    int frameCount;

    public bool IsLooping
    {
        get { return isLooping; }
    }
    bool isLooping;

    public Animation(GridSheet spritesheet, int animRow, float frameTime, int frameCount, bool isLooping)
    {
        this.spriteSheet = spritesheet;
        this.animRow = animRow;
        this.frameNum = 0;
        this.frameTime = frameTime;
        this.frameCount = frameCount;
        this.isLooping = isLooping;
    }

}
