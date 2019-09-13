using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LS.Device;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LS.Scene
{
    class Title : IScene
    {
        private bool IsEndFlag;
        IScene backGroundScene;
        private Sound sound;

        public Title()
        {
            IsEndFlag = false;
            var gameDevice = GameDevice.Instance();
            sound = gameDevice.GetSound();
            
        }

        public void Draw(Renderer renderer)
        {
            //backGroundScene.Draw(renderer);

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
            return Scene.GamePlay;
        }

        public void Shutdown()
        {
            sound.StopBGM();
        }

        public void Update(GameTime gameTime)
        {
            sound.PlayBGM("titlebgm");
            if (Input.GetKeyTrigger(Keys.Space))
            {
                IsEndFlag = true;
            }
        }
    }
}
