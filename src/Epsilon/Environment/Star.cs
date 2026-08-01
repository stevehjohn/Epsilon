using Microsoft.Xna.Framework;

namespace Epsilon.Environment;

public class Star
{
    public float AnchorX { get; init; }

    public float AnchorY { get; init; }

    public float X { get; set; }

    public float Y { get; set; }

    public float Velocity { get; init; }

    public int Type { get; init; }

    public Color Color { get; set; }

    public float Intensity { get; init; }
}