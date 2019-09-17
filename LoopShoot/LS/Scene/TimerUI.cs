using LS.Device;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LS.Util;

namespace LS.Scene
{
    class TimerUI
    {
        private Timer timer;

        public TimerUI(Timer timer)
        {
            this.timer = timer;
        }

        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("", new Vector2(400, 10));
            renderer.DrawNumber("", new Vector2(600, 13), timer.Now());
        }
    }
}
