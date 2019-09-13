using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LS.Device;
using LS.Def;

using Microsoft.Xna.Framework;
namespace LS.Scene
{
    
    class GamePlay : IScene
    {
        private bool IsEndFlag;
        private Sound sound;
        public GamePlay()
        {
            IsEndFlag = false;
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin();
            renderer.DrawTexture("stage", Vector2.Zero);
            renderer.End();
        }

        public void Initialize()
        {
            IsEndFlag = false;

        }

        public bool IsEnd()
        {
            return IsEndFlag;
        }

        public Scene Next()
        {
            return Scene.Ending;
        }

        public void Shutdown()
        {
        }

        public void Update(GameTime gameTime)
        {
            
        }
    }


}
