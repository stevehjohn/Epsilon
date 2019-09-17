using System;

namespace Epsilon.Infrastructure
{
    public static class EntryPoint
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
