using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LS.Scene
{
    class Title : IScene
    {
        private bool IsEndFlag;
        public Title()
        {

        }

        //public void Draw(Renderer renderer)
        //{
        //    renderer.Bigin();
        //    renderer.End();
        //}

        public void Initialize()
        {
            IsEndFlag = false;
        }

        public bool IsEnd()
        {
            throw new NotImplementedException();
        }

        public Scene Next()
        {
            return Scene.GamePlay;
        }

        public void Shutdown()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            
        }
    }
}
