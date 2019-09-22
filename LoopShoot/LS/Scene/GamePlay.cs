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
using Microsoft.Xna.Framework.Graphics;

namespace LS.Scene
{
    enum Stage
    {
        S1, //ステージ1
        S2, //ステージ2
        S3, //ステージ3
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
        int waveCnt; //現在のWave

        int enemyCnt; //現在何体敵が出現したか

        Dictionary<Vector2, int> points = new Dictionary<Vector2, int>();
        Dictionary<string, List<Vector2>> spP = new Dictionary<string, List<Vector2>>();

        bool clearFlag; //ゲームクリア条件を満たしているか

        bool waveClearFlag;

        float rotation;

        public GamePlay()
        {
            IsEndFlag = false;
            var gameDevice = GameDevice.Instance(); //ゲームデバイスクラスを生成
            sound = gameDevice.GetSound(); //サウンドクラス
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin(); //描画開始
            mapLoad.Draw(renderer); //マップ
            characterManager.Draw(renderer); //キャラクター一括描画
            renderer.DrawTexture("enemy", Vector2.Zero, null, rotation, new Vector2(27, 32), Vector2.One, SpriteEffects.None, 0);
            life.Draw(renderer); //体力を描画
            renderer.End(); //描画終了
        }

        public void Initialize()
        {
            IsEndFlag = false;
            clearFlag = false;
            characterManager = new CharacterManager();
            spP.Clear();

            mapLoad = new MapLoad(); //マップクラスの作成
            mapLoad.LoadMap(1); //マップの1番目をロード

            List<TurnPoint> mpts = mapLoad.CreateTP(); //エネミーの通り道
            spP = new Dictionary<string, List<Vector2>>(mapLoad.CreateSpawnP());
            foreach (var mpl in mpts)
                characterManager.AddTurnPoint(mpl);
            Tower tower = new Tower(3, mapLoad.CreateTower()); //タワーを生成
            characterManager.AddTower(tower); //
            life = new Life(characterManager.tower.life, new Vector2(50, 800));

            MouseCol mouseCol = new MouseCol("white", Input.MousePosition);
            characterManager.AddMouseCol(mouseCol);

            stage = Stage.S1; //現在のステージ

            waveCnt = 1; //現在のウェーブ

            pillarCnt = 0; //ピラーを置いた数（0で初期化）

            maxPillarCnt = 3;
            maxBulletCnt = 2; //弾を発射できる最大数

            timeCounter = 0;

            i = 0;
            j = 0;
            maxI = 0;
            maxJ = 0;
            waveClearFlag = false;

            SetPointMaxNum(stage);

            enemyCnt = 0;

        }

        /// <summary>
        /// 現在のシーンを終了
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            return IsEndFlag;
        }

        /// <summary>
        /// 現在のシーンから移行
        /// </summary>
        /// <returns></returns>
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
            rotation += 0.1f;
            timeCounter += 0.1f;
            characterManager.Update(gameTime);

            switch (waveCnt)
            {
                case 1:
                    if (CountCheck(timeCounter))
                    {
                        SpawnEnemy(points[spP["上"][0]], enemyCnt, spP["上"][0]);
                        enemyCnt++;
                        timeCounter = 0;
                    }
                    else if (enemyCnt >= points[spP["上"][0]]
                        && characterManager.enemies.Count == 0)
                    {
                        waveCnt++;
                        enemyCnt = 0;
                        waveClearFlag = true;
                    }
                    break;
                case 2:
                    waveClearFlag = WaveClear(waveClearFlag);
                    if (CountCheck(timeCounter))
                    {
                        SpawnEnemy(points[spP["上"][0]], enemyCnt, spP["上"][0]);
                        SpawnEnemy(points[spP["右"][0]], enemyCnt, spP["右"][0]);
                        enemyCnt++;
                        timeCounter = 0;
                    }
                    else if (enemyCnt >= points[spP["右"][0]]
                        && characterManager.enemies.Count == 0)
                    {
                        waveCnt++;
                        enemyCnt = 0;
                        waveClearFlag = true;
                    }
                    break;
                case 3:
                    waveClearFlag = WaveClear(waveClearFlag);
                    if (CountCheck(timeCounter))
                    {
                        SpawnEnemy(points[spP["上"][0]], enemyCnt, spP["上"][0]);
                        SpawnEnemy(points[spP["右"][0]], enemyCnt, spP["右"][0]);
                        enemyCnt++;
                        timeCounter = 0;
                    }
                    else if (enemyCnt >= points[spP["右"][0]]
                        && characterManager.enemies.Count == 0)
                    {
                        clearFlag = true;
                        IsEndFlag = true;
                    }
                    break;
                default:
                    break;
            }


            life.GetLife(characterManager.tower.life);
            life.Update(gameTime);

            characterManager.mouseCol.position = Input.MousePosition + new Vector2(-32, -32);

            if (characterManager.tower.life <= 0)
                IsEndFlag = true;

            if (pillarCnt >= 1)
                foreach (var p in characterManager.pillars)
                {
                    characterManager.AddRay(new RayShot("particle", p.position));
                    foreach (var rs in characterManager.rayShots)
                        rs.Move(Input.MousePosition);
                }

            if (Input.IsMouseLBottonDown()
                && characterManager.mouseCol.putPossibleFlag)
            {
                characterManager.Add(new Pillar("pillar", Input.MousePosition + new Vector2(-32, -32)));
                pillarCnt++;
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

            if (characterManager.bullets.Count > 0)
            {
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


        public void SpawnEnemy(int maxEnCnt, int enemyCnt, Vector2 poi)
        {
            if (maxEnCnt > enemyCnt)
                characterManager.Add(new Enemy("enemy", poi, 1, 0));
        }

        /// <summary>
        /// エネミーの出現間隔
        /// </summary>
        /// <param name="timeCounter">時間経過</param>
        /// <returns></returns>
        public bool CountCheck(float timeCounter)
        {
            if (timeCounter >= 3) //6で一秒、3で半分の0.5秒
                return true;
            else
                return false;
        }

        /// <summary>
        /// ステージごとの出現最大値をセット
        /// </summary>
        /// <param name="st">ステージ番号</param>
        public void SetPointMaxNum(Stage st)
        {
            if (st == Stage.S1)
            {
                points.Clear();
                points.Add(spP["上"][0], 4);
                points.Add(spP["右"][0], 6);
                points.Add(spP["下"][0], 2);
            }





            //else if (st == Stage.S2)
            //{
            //    point1 = Vector2.Zero;
            //    point2 = Vector2.Zero;
            //    point3 = Vector2.Zero;
            //}
            //else if (st == Stage.S3)
            //{
            //    point1 = Vector2.Zero;
            //    point2 = Vector2.Zero;
            //    point3 = Vector2.Zero;
            //}
        }

        public bool WaveClear(bool clearFlag)
        {
            if (clearFlag)
            {
                characterManager.bullets.Clear();
                characterManager.pillars.Clear();
                
                pillarCnt = 0;
                clearFlag = false;
            }
            return clearFlag;
        }



    }
}