using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Spritesheet;
using simulation.Input;
using System;

namespace simulation.Display;
public static class DisplayManager {
    public const float SCALE = 1.4f;
    public static int width = 1920;
    public static int height = 1080;
    public static Rectangle screen = new Rectangle(0, 0, width, height);
}
