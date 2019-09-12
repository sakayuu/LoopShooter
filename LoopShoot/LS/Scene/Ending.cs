using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace LS.Scene
{
    class Ending : IScene
    {
        private bool IsEndFlag;

        public Ending()
        {
            IsEndFlag = false;
        }
        public void Initialize()
        {
            
        }

        public bool IsEnd()
        {
            return IsEndFlag;　
        }

        public Scene Next()
        {
            return Scene.Title;
        }

        public void Shutdown()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            
        }
    }
}
