using LS.Device;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Actor
{
    class CharacterManager
    {
        public Tower tower;
        public MouseCol mouseCol;
        public List<Character> pillars;
        public List<Character> bullets;
        public List<Character> enemies;
        public List<Character> addNewCharacters;
        public List<TurnPoint> turnPoints;
        public List<RayShot> rayShots;


        bool wFlag1, wFlag2, wFlag3;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CharacterManager()
        {
            Initialize(); //初期化
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            //各リストを生成とクリア
            if (tower != null)
                tower.Initialize();

            if (mouseCol != null)
                mouseCol.Initialize();

            if (rayShots != null)
                rayShots.Clear();
            else
                rayShots = new List<RayShot>();

            if (pillars != null)
                pillars.Clear();
            else
                pillars = new List<Character>();

            if (bullets != null)
                bullets.Clear();
            else
                bullets = new List<Character>();

            if (enemies != null)
                enemies.Clear();
            else
                enemies = new List<Character>();

            if (turnPoints != null)
                turnPoints.Clear();
            else
                turnPoints = new List<TurnPoint>();
            
            if (addNewCharacters != null)
                addNewCharacters.Clear();
            else
                addNewCharacters = new List<Character>();
            wFlag1 = true; wFlag2 = true; wFlag3 = true;
        }

        public void Add(Character character)
        {
            //早期リターンで処理短縮
            if (character == null)
                return;
            //とりあえずは追加リストに追加
            addNewCharacters.Add(character);
        }

        public void AddTower(Tower tower)
        {
            tower.Initialize();
            this.tower = tower;
        }

        public void AddMouseCol(MouseCol mouseCol)
        {
            mouseCol.Initialize();
            this.mouseCol = mouseCol;
        }

        public void AddRay(RayShot rayShot)
        {
            if (rayShot == null)
                return;
            rayShots.Add(rayShot);
        }

        public void AddTurnPoint(TurnPoint turnPoint)
        {
            if (turnPoint == null)
                return;
            turnPoint.Initialize();
            turnPoints.Add(turnPoint);
        }
        
        private void HitToCharacters()
        {
            foreach (var pillar in pillars)
            {
                foreach (var bullet in bullets)
                {
                    if (bullet.IsCollision(pillar))
                    {
                        pillar.Hit(bullet);
                    }
                    foreach (var enemy in enemies)
                    {
                        if (enemy.IsDead())
                            continue;
                        //弾が敵に当たってるか？
                        if (bullet.IsCollision(enemy))
                        {
                            bullet.Hit(enemy);
                            enemy.damageNum = bullet.GetStatus() + 1;
                            enemy.Hit(bullet);
                        }
                        //タワーに敵が当たってるか？
                        if (tower.IsCollision(enemy))
                        {
                            tower.Hit(enemy);
                            tower.st = enemy.GetStatus();
                            enemy.Hit(tower);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// エネミーに道を歩かせる処理
        /// </summary>
        public void HitToWay()
        {
            foreach (var p in turnPoints)
            {
                foreach (var enemy in enemies)
                {
                    if (p.IsCollision(enemy))
                    {
                        p.Hit(enemy);
                        enemy.Hit(p);
                        enemy.Move(p.NextPoint(enemy.position));
                    }
                }
            }
        }

        /// <summary>
        /// 道があるとピラー置けなくする処理
        /// </summary>
        public void HitToWaysAndMouse()
        {
            if (wFlag1 && wFlag2 && wFlag3)
                mouseCol.putPossibleFlag = true;
            else
                mouseCol.putPossibleFlag = false;
            
            foreach (var tp in turnPoints)
            {
                if (tp.IsCollision(mouseCol))
                {
                    mouseCol.Hit(tp);
                    wFlag2 = false;
                }
                else
                    wFlag2 = true;
            }
            foreach (var pillar in pillars)
            {
                foreach (var rs in rayShots)
                {
                    if (tower.IsCollision(rs))
                        mouseCol.Hit(pillar);
                    else if (rs.IsCollision(mouseCol))
                        rs.Hit(mouseCol);
                }
                if (pillar.IsCollision(mouseCol))
                {
                    mouseCol.Hit(pillar);
                    wFlag3 = false;
                }
                else
                    wFlag3 = true;
            }
        }

        /// <summary>
        /// 死亡キャラの削除
        /// </summary>
        private void RemoveDeadCharacters()
        {
            //死んでいたら、リストから削除
            enemies.RemoveAll(e => e.IsDead());
            bullets.RemoveAll(b => b.IsDead());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">ゲーム時間</param>
        public void Update(GameTime gameTime)
        {
            //全キャラクター更新
            tower.Update(gameTime);
            mouseCol.Update(gameTime);
            //rayShot.Update(gameTime);
            foreach (var tp in turnPoints)
                tp.Update(gameTime);
            foreach (var e in enemies)
                e.Update(gameTime);
            foreach (var b in bullets)
                b.Update(gameTime);
            foreach (var p in pillars)
                p.Update(gameTime);
            
            //追加候補者をリストに追加
            foreach (var newChara in addNewCharacters)
            {
                //キャラがプレイヤーだったらプレイやリストに登録
                if (newChara is Bullet)
                {
                    newChara.Initialize();
                    bullets.Add(newChara);
                }
                else if (newChara is Pillar)
                {
                    newChara.Initialize();
                    pillars.Add(newChara);
                }
                //それ以外は敵リストに登録
                else
                {
                    newChara.Initialize();//初期化
                    enemies.Add(newChara);//登録
                }
            }
            //追加処理後、追加リストはクリア
            addNewCharacters.Clear();

            //当たり判定
            HitToWay();
            HitToWaysAndMouse();
            HitToCharacters();

            //死亡フラグが立っていたら削除
            RemoveDeadCharacters();
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer">描画オブジェクト</param>
        public void Draw(Renderer renderer)
        {
            //全キャラ描画
            tower.Draw(renderer);
            mouseCol.Draw(renderer);
            //foreach (var rs in rayShots)
            //    rs.Draw(renderer);
            foreach (var p in pillars)
                p.Draw(renderer);
            foreach (var e in enemies)
                e.Draw(renderer);
            foreach (var b in bullets)
                b.Draw(renderer);

        }

    }
}
