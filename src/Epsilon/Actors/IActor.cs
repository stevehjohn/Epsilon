namespace Epsilon.Actors
{
    public interface IActor
    {
        void Initialise();

        void LoadContent();

        void UpdateState();

        float Render(float depth);
    }
}