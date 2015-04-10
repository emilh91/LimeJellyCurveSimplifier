using System;

namespace LimeJelly.Driver
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
