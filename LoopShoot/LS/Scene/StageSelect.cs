using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LS.Device;
using Microsoft.Xna.Framework;

namespace LS.Scene
{
    class StageSelect : IScene
    {
        private bool IsEndFlag;
        private Sound sound;

        public StageSelect()
        {
            IsEndFlag = false;
            var gameDevice = GameDevice.Instance();
            sound = gameDevice.GetSound();
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin();
            renderer.DrawTexture("selectBG", Vector2.Zero);
            renderer.DrawTexture("selectLogo", new Vector2(250, 60));
            renderer.DrawTexture("1", new Vector2(300, 300));
            renderer.DrawTexture("2", new Vector2(600, 300));
            renderer.DrawTexture("3", new Vector2(880, 300));
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
            //sound.PlayBGM("titlebgm");
            if (Input.IsMouseLBottonDown())
            {
                if (IsHitCol(Input.MousePosition, new Vector2(300, 430)))
                    GamePlay.stage = Stage.S1;
                else if (IsHitCol(Input.MousePosition, new Vector2(600, 430)))
                    GamePlay.stage = Stage.S2;
                else if (IsHitCol(Input.MousePosition, new Vector2(880, 430)))
                    GamePlay.stage = Stage.S3;
                else
                    return;
                IsEndFlag = true;
            }

        }

        public bool IsHitCol(Vector2 pos, Vector2 pos2)
        {
            //自分と相手の位置の長さを計算（2点間の距離）
            float length = (pos - pos2).Length();
            //画像のサイズにより変化
            //自分半径と相手半径の和
            float radiusSum = 50f + 50f;
            //半径の和と距離を比べて、等しいかまたは小さいか（以下か）
            if (length <= radiusSum)
            {
                return true;
            }
            return false;
        }
    }
}
