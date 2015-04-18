using System;

namespace LimeJelly.CurveSimplifier
{
    static class Program
    {
#if NETFX_CORE
        [MTAThread]
#else
        [STAThread]
#endif
        static void Main()
        {
            using (var app = new Game1())
            {
                app.Run();
            }
        }
    }
}
