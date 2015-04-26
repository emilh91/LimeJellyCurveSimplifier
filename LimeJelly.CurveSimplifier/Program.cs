using System;

namespace LimeJelly.CurveSimplifier
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var app = new Game1())
            {
                app.Run();
            }
        }
    }
}
