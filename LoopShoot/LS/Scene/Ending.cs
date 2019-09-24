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
    class Ending : IScene
    {
        private bool IsEndFlag;
        IScene backGroundScene;
        private Sound sound;


        public Ending(IScene scene)
        {
            IsEndFlag = false;
            backGroundScene = scene;
            var gameDevice = GameDevice.Instance();
            sound = gameDevice.GetSound();

        }

        public void Draw(Renderer renderer)
        {
            backGroundScene.Draw(renderer);

            renderer.Begin();
            renderer.DrawTexture("gameover", new Vector2(150, 150));
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
            return Scene.Title;
        }

        public void Shutdown()
        {
            sound.StopBGM();
        }

        public void Update(GameTime gameTime)
        {
            sound.PlayBGM("endingbgm");

            if (Input.GetKeyTrigger(Keys.Space))
            {
                IsEndFlag = true;
                sound.PlaySE("endingse");
            }
        }
    }
}
