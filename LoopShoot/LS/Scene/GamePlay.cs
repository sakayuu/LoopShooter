using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LS.Device;
using LS.Def;

using Microsoft.Xna.Framework;
using LS.Actor;
using System.Threading;
using LS.MapSystem;

namespace LS.Scene
{

    class GamePlay : IScene
    {
        private CharacterManager characterManager;
        private Timer timer;

        private bool IsEndFlag;
        private Sound sound;

        public Life life;
        private MapLoad mapLoad;//

        int pillarCnt;
        int maxPillarCnt;

        int maxBulletCnt;

        float timeCounter;

        int i;

        public GamePlay()
        {
            IsEndFlag = false;
            var gameDevice = GameDevice.Instance();
            sound = gameDevice.GetSound();
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin();

            renderer.DrawTexture("stage", Vector2.Zero);
            mapLoad.Draw(renderer);
            characterManager.Draw(renderer);
            life.Draw(renderer);

            renderer.End();
        }

        public void Initialize()
        {

            IsEndFlag = false;

            characterManager = new CharacterManager();
            mapLoad = new MapLoad();

            mapLoad.LoadMap(1);


            Tower tower = new Tower(3, Vector2.Zero);
            characterManager.AddTower(tower);

            life = new Life(characterManager.tower.life, new Vector2(50, 800));

            pillarCnt = 0;
            maxPillarCnt = 3;
            maxBulletCnt = 1;
            timeCounter = 0;
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
            timeCounter += 0.1f;

            characterManager.Update(gameTime);

            life.GetLife(characterManager.tower.life);
            life.Update(gameTime);

            if (characterManager.tower.life <= 0)
                IsEndFlag = true;

            if (timeCounter >= 20) //敵を出現
            {
                characterManager.Add(new Enemy("particleSmall", new Vector2(1200, 300), 1, 0));
                timeCounter = 0;
            }

            if (pillarCnt < maxPillarCnt
                && Input.IsMouseLBottonDown())
                characterManager.Add(new Pillar("white", Input.MousePosition));
            else if (characterManager.pillars.Count >= 3
                && characterManager.bullets.Count < maxBulletCnt
                && Input.IsMouseRBottonDown())
                characterManager.Add(new Bullet("black", Input.MousePosition));

            if (characterManager.bullets.Count <= 1)
            {
                foreach (var e in characterManager.enemies)
                {
                    e.Move(characterManager.tower.position);
                }


                foreach (var b in characterManager.bullets)
                {
                    if (i >= maxPillarCnt)
                        i = 0;
                    b.Move(characterManager.pillars[i].position);
                    if (characterManager.pillars[i].damageNum != 0)
                    {
                        characterManager.pillars[i].damageNum = 0;
                        i++;
                    }

                }

            }

        }


    }

}
