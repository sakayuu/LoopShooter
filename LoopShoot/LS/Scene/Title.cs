using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LS.Device;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LS.Scene
{
    class Title : IScene
    {
        private bool IsEndFlag;
        private Sound sound;

        float rotation;
        float alpha;
        float x;

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
            renderer.DrawTexture("titleBG", Vector2.Zero);
            renderer.DrawTexture("titleback", new Vector2(650, 440), null, rotation, new Vector2(400, 400), Vector2.One, SpriteEffects.None, 0);
            renderer.DrawTexture("titleLogo", new Vector2(200, 270));
            renderer.DrawTexture("titleStart", new Vector2(250, 850), alpha);
            renderer.End();
        }

        public void Initialize()
        {
            IsEndFlag = false;
            rotation = 0;
            alpha = 0;
            x = 1;
        }

        public bool IsEnd()
        {
            return IsEndFlag;
        }

        public Scene Next()
        {
            return Scene.StageSelect;
        }

        public void Shutdown()
        {
            //sound.StopBGM();
        }

        public void Update(GameTime gameTime)
        {
            sound.PlayBGM("titlebgm");
            rotation += 0.03f;
            alpha = InvisibleImage(alpha);
            if (Input.IsMouseLBottonDown())
            {
                IsEndFlag = true;
                sound.PlaySE("titlese");
            }
        }

        public float InvisibleImage(float alpha)
        {
            alpha += 0.03f * x;
            if (alpha <= 0)
                x = 1;
            else if (alpha >= 1)
                x = -1;

            return alpha;
        }
    }
}
