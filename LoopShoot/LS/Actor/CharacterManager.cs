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
        public List<Character> pillars;
        public List<Character> bullets;
        public List<Character> enemies;
        public List<Character> addNewCharacters;

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

            if (addNewCharacters != null)
                addNewCharacters.Clear();
            else
                addNewCharacters = new List<Character>();
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
            this.tower = tower;
        }

        private void HitToCharacters()
        {
            foreach (var bullet in bullets)
            {
                foreach (var pillar in pillars)
                {
                    if (bullet.IsCollision(pillar))
                    {
                        pillar.Hit(bullet);
                    }
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

            foreach (var e in enemies)
                e.Update(gameTime);
            foreach (var b in bullets)
                b.Update(gameTime);

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
            foreach (var p in pillars)
            {
                p.Draw(renderer);
            }
            foreach (var e in enemies)
            {
                e.Draw(renderer);
            }
            foreach (var b in bullets)
            {
                b.Draw(renderer);
            }
        }

    }
}
