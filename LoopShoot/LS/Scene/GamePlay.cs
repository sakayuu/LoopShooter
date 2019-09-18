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

        int i, j;
        int maxI, maxJ;

        Random rnd;

        Vector2 point1, point2, point3;
        Vector2 p;

        int maxEnemy = 10;
        int enemyCnt;

        public GamePlay()
        {
            IsEndFlag = false;
            var gameDevice = GameDevice.Instance();
            sound = gameDevice.GetSound();
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin();

            renderer.DrawTexture("BG", Vector2.Zero);
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


            Tower tower = new Tower(3, new Vector2(448, 384));
            characterManager.AddTower(tower);

            life = new Life(characterManager.tower.life, new Vector2(50, 800));

            pillarCnt = 0;
            maxPillarCnt = 3;
            maxBulletCnt = 2;
            timeCounter = 0;
            i = 0;
            j = 0;
            maxI = 0;
            maxJ = 0;

            point1 = new Vector2(448, 0);
            point2 = new Vector2(1280, 384);
            point3 = new Vector2(448, 960);

            rnd = new Random();

            enemyCnt = 0;
        }

        public bool IsEnd()
        {
            return IsEndFlag;
        }

        public Scene Next()
        {
            Scene nextScene = Scene.Ending;
            if(enemyCnt >= maxEnemy)
                nextScene = Scene.GameClear;
            return nextScene;
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

            switch (rnd.Next(1, 4))
            {
                case 1:
                    p = point1;
                    break;
                case 2:
                    p = point2;
                    break;
                case 3:
                    p = point3;
                    break;
                default:
                    break;
            }

            if (enemyCnt >= maxEnemy)
                Next();

            if (characterManager.tower.life <= 0)
                IsEndFlag = true;

            if (timeCounter >= 20) //敵を出現
            {
                characterManager.Add(new Enemy("enemy", p, 1, 0));
                timeCounter = 0;
                enemyCnt++;
            }


            if (Input.IsMouseLBottonDown()
                && characterManager.bullets.Count < maxBulletCnt)
            {
                characterManager.Add(new Pillar("pillar", Input.MousePosition));
                pillarCnt++;
            }

            else if (characterManager.pillars.Count >= 3
                && characterManager.bullets.Count < maxBulletCnt
                && Input.IsMouseRBottonDown())
            {
                characterManager.Add(new Bullet("bullet", Input.MousePosition));
                if (maxI == 0)
                    maxI = pillarCnt;
                else if (maxI != 0 && maxJ == 0)
                    maxJ = pillarCnt;
            }




            if (characterManager.bullets.Count >= 0)
            {
                foreach (var e in characterManager.enemies)
                {
                    e.Move(characterManager.tower.position);
                }


                foreach (var b in characterManager.bullets)
                {
                    if (b == characterManager.bullets[0])
                    {
                        if (i >= maxI)
                            i = 0;
                        b.Move(characterManager.pillars[i].position);
                        if (characterManager.pillars[i].IsCollision(characterManager.bullets[0]))
                        {
                            i++;
                        }

                    }

                    else if (b == characterManager.bullets[1])
                    {
                        if (j >= maxJ)
                            j = maxJ - maxI;
                        b.Move(characterManager.pillars[j].position);
                        if (characterManager.pillars[j].IsCollision(characterManager.bullets[1]))
                        {
                            j++;
                        }
                    }
                }

            }

        }


    }

}
