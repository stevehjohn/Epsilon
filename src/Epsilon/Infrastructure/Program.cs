using System;

namespace Epsilon.Infrastructure
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using (var game = new Epsilon())
            {
                game.Run();
            }
        }
    }
}
