using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using Microsoft.Xna.Framework.Input;
// using simulation.Input;
// using simulation.Display;
// using System;


namespace simulation.Shapes;

public class Circle {
    public float Radius{
        get {return radius;}
        set {radius = value;}
    }
    float radius;
    public Vector2 Position{
        get {return pos;}
        set {pos = value;}
    }
    Vector2 pos;
    public Vector2 Center{
        get {return center;}
        set {center = value;}
    }
    Vector2 center;
    public Circle(float radius, Vector2 center) {
        this.radius = radius;
        this.center = center; 
        Position = new Vector2(center.X - radius, center.Y - radius);
    }

    public bool Collides(Rectangle other) {
        Vector2 otherCenter = other.Center.ToVector2();
        if (radius > Vector2.Distance(center, otherCenter)) {
            return true;
        }
        return false;
    }
}
