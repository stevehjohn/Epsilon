using Epsilon.Maths;

namespace Epsilon.Environment;

public class Glimmer
{
    public float Alpha { get; set; }

    public float AlphaDelta { get; set; }

    public Coordinates BoardPosition { get; init; }

    public float YOffset { get; set; }

    public float XOffset { get; init; }

    public float Velocity { get; init; }
}