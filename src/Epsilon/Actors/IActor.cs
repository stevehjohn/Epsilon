namespace Epsilon.Actors
{
    public interface IActor
    {
        void Initialise();

        void UpdateState();

        float Render(float depth);
    }
}