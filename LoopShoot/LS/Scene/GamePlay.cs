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
    enum Stage
    {
        S1,
        S2,
        S3,
    }
    class GamePlay : IScene
    {
        private CharacterManager characterManager; //キャラクター管理クラス
        private Stage stage; //現在のステージ番号
        private bool IsEndFlag; //ステージ終了フラグ
        private Sound sound; //サウンド
        public Life life; //タワーの体力
        private MapLoad mapLoad; //現在のマップ
        int pillarCnt; //ピラーを置いた数
        int maxPillarCnt; //ピラーを置ける最大値
        int maxBulletCnt; //弾を生成できる最大値
        float timeCounter; //時間経過
        int i, j; //バレットのループに使用
        int maxI, maxJ; //バレットループに使用
        int waveCnt; //現在のWaveの
        
        Vector2 point1, point2, point3;
        Vector2 pPos;

        int maxEnemy;
        int maxEnemy2;
        int maxEnemy3;

        int enemyCnt;

        bool clearFlag;

        bool instanceFlag;

        bool putFlag;

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
            clearFlag = false;
            characterManager = new CharacterManager();

            mapLoad = new MapLoad();
            mapLoad.LoadMap(1);

            Tower tower = new Tower(3, new Vector2(448, 384));
            characterManager.AddTower(tower);
            life = new Life(characterManager.tower.life, new Vector2(50, 800));

            MouseCol mouseCol = new MouseCol("white", Vector2.Zero);
            characterManager.AddMouseCol(mouseCol);

            stage = Stage.S1;

            waveCnt = 1;

            pillarCnt = 0;

            maxPillarCnt = 3;
            maxBulletCnt = 2;

            timeCounter = 0;

            i = 0;
            j = 0;
            maxI = 0;
            maxJ = 0;

            SetPoint(stage);
            SetMaxEnemy(stage);

            enemyCnt = 0;

            instanceFlag = true;
            putFlag = false;

        }

        public bool IsEnd()
        {
            return IsEndFlag;
        }

        public Scene Next()
        {
            Scene nextScene = Scene.Ending;
            if (clearFlag)
                nextScene = Scene.GameClear;
            return nextScene;
        }

        public void Shutdown() { }

        public void Update(GameTime gameTime)
        {
            timeCounter += 0.1f;
            switch (waveCnt)
            {
                case 1:
                    if (enemyCnt < maxEnemy) //敵を出現)
                    {
                        timeCounter = SpawnEnemy(timeCounter, point1, point2, point3, stage, waveCnt);
                    }
                    else if (enemyCnt >= maxEnemy
                        && characterManager.enemies.Count == 0)
                    {
                        waveCnt++;
                        enemyCnt = 0;
                    }
                    break;
                case 2:
                    if (enemyCnt < maxEnemy2)
                    {
                        timeCounter = SpawnEnemy(timeCounter, point1, point2, point3, stage, waveCnt);
                    }
                    else if (enemyCnt >= maxEnemy2
                        && characterManager.enemies.Count == 0)
                    {
                        waveCnt++;
                        enemyCnt = 0;
                    }
                    break;
                case 3:
                    if (enemyCnt < maxEnemy3)
                    {
                        timeCounter = SpawnEnemy(timeCounter, point1, point2, point3, stage, waveCnt);
                    }
                    else if (enemyCnt >= maxEnemy3
                        && characterManager.enemies.Count == 0)
                    {
                        clearFlag = true;
                        IsEndFlag = true;
                    }
                    break;
                default:
                    break;
            }

            characterManager.Update(gameTime);

            life.GetLife(characterManager.tower.life);
            life.Update(gameTime);

            characterManager.mouseCol.position = Input.MousePosition + new Vector2(-32, -32);

            if (characterManager.tower.life <= 0)
                IsEndFlag = true;

            if (Input.IsMouseLBottonDown())
            {
                if (pillarCnt <= 0)
                {
                    characterManager.Add(new Pillar("pillar", Input.MousePosition + new Vector2(-32, -32)));
                    putFlag = true;
                    pillarCnt++;
                    instanceFlag = true;
                }
                else
                {
                    if (characterManager.mouseCol.putPossibleFlag)
                    {
                        characterManager.Add(new Pillar("pillar", Input.MousePosition + new Vector2(-32, -32)));
                        putFlag = true;
                        pillarCnt++;
                        instanceFlag = true;
                    }
                }
            }

            if (characterManager.pillars.Count >= 3
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
                    e.Move(characterManager.tower.position);

                foreach (var b in characterManager.bullets)
                {
                    if (b == characterManager.bullets[0])
                    {
                        if (i >= maxI)
                            i = 0;
                        b.Move(characterManager.pillars[i].position);
                        if (characterManager.pillars[i].IsCollision(characterManager.bullets[0]))
                            i++;
                    }

                    else if (b == characterManager.bullets[1])
                    {
                        if (j >= maxJ)
                            j = maxJ - maxI;
                        b.Move(characterManager.pillars[j].position);
                        if (characterManager.pillars[j].IsCollision(characterManager.bullets[1]))
                            j++;
                    }
                }
            }
        }


        public float SpawnEnemy(float timeCounter, Vector2 pos, Vector2 pos2, Vector2 pos3, Stage st, int waveCnt)
        {
            if (CountCheck(timeCounter))
            {
                enemyCnt++;
                if (st == Stage.S1)
                {
                    if (waveCnt == 1)
                        characterManager.Add(new Enemy("enemy", pos, 1, 0));
                    else if (waveCnt == 2)
                    {
                        characterManager.Add(new Enemy("enemy", pos, 1, 0));
                        characterManager.Add(new Enemy("enemy", pos2, 1, 0));
                    }
                    else if (waveCnt == 3)
                    {
                        characterManager.Add(new Enemy("enemy", pos, 1, 0));
                        characterManager.Add(new Enemy("enemy", pos2, 1, 0));
                    }

                }
                else if (st == Stage.S2)
                {
                    characterManager.Add(new Enemy("enemy", pos, 1, 0));
                    characterManager.Add(new Enemy("enemy", pos2, 1, 0));
                }
                timeCounter = 0;
            }
            return timeCounter;
        }

        public bool CountCheck(float timeCounter)
        {
            if (timeCounter >= 3) //6で一秒、3で半分の0.5秒
                return true;
            else
                return false;
        }

        public void SetPoint(Stage st)
        {
            if (st == Stage.S1)
            {
                point1 = new Vector2(448, 0);
                point2 = new Vector2(1280, 384);
                point3 = new Vector2(448, 960);
            }
            else if (st == Stage.S2)
            {
                point1 = Vector2.Zero;
                point2 = Vector2.Zero;
                point3 = Vector2.Zero;
            }
            else if (st == Stage.S3)
            {
                point1 = Vector2.Zero;
                point2 = Vector2.Zero;
                point3 = Vector2.Zero;
            }
        }

        public void SetMaxEnemy(Stage st)
        {
            if (st == Stage.S1)
            {
                maxEnemy = 4;
                maxEnemy2 = 6;
                maxEnemy3 = 8;
            }
            else if (st == Stage.S2)
            {
                maxEnemy = 4;
                maxEnemy2 = 4;
                maxEnemy3 = 4;
            }
            else if (st == Stage.S3)
            {
                maxEnemy = 4;
                maxEnemy2 = 4;
                maxEnemy3 = 4;
            }
        }

        public int SpawnEnemyPos(int spEnCnt, Vector2 pos)
        {
            int i;
            for (i = 0; i < spEnCnt; i++)
            {
                characterManager.Add(new Enemy("enemy", pos, 1, 0));
            }
            return 1;
        }


    }
}