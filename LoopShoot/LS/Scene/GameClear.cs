using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LS.Device;
using Microsoft.Xna.Framework;

namespace LS.Scene
{
    class GameClear : IScene
    {
        private bool IsEndFlag;//死亡フラグ
        public GameClear(IScene scene)
        {
            IsEndFlag = false;
        }

        public void Draw(Renderer renderer)
        {
            
        }

        public void Initialize()
        {
            
        }

        public bool IsEnd()
        {
            throw new NotImplementedException();
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
