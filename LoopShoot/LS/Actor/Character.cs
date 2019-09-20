using LS.Device;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Actor
{
    enum Status
    {
        prototype,
        upgrade,
    };

    /// <summary>
    /// キャラクター抽象クラス
    /// </summary>
    abstract class Character
    {
        public Vector2 position; //位置
        protected string name; //画像の名前
        protected bool isDeadFlag; //死亡フラグ
        public int life; //体力
        public int damageNum;

        public bool putPossibleFlag;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">画像の名前</param>
        public Character(string name)
        {
            this.name = name;
            position = Vector2.Zero;
            isDeadFlag = false;

        }

        /// <summary>
        /// 衝突判定（2点間の距離と円の半径）
        /// </summary>
        /// <parm name="other"></parm>
        /// <returns></returns>
        public bool IsCollision(Character other)
        {
            //自分と相手の位置の長さを計算（2点間の距離）
            float length = (position - other.position).Length();
            //画像のサイズにより変化
            //自分半径と相手半径の和
            float radiusSum = 32f + 32f;
            //半径の和と距離を比べて、等しいかまたは小さいか（以下か）
            if (length <= radiusSum)
            {
                return true;
            }
            return false;
        }

        public bool CollisionChackPillar(Character col)
        {
            //自分と相手の位置の長さを計算（2点間の距離）
            float length = (position - col.position).Length();
            //画像のサイズにより変化
            //自分半径と相手半径の和
            float radiusSum = 32f + 32f;
            //半径の和と距離を比べて、等しいかまたは小さいか（以下か）
            if (length <= radiusSum && col is Pillar)
            {
                return true;
            }
            return false;
        }

        public abstract void Initialize();//初期化
        public abstract void Update(GameTime gameTime);//更新
        public abstract void Shutdown();//終了
        public abstract void Hit(Character other);//ヒット通知

        public abstract int Damage(int damage); //ダメージを受ける
        public abstract int GetStatus(); //自分のステータスを渡す
        public abstract void Move(Vector2 tPos); //移動

        /// <summary>
        /// 死んでいるか？
        /// </summary>
        /// <returns></returns>
        public bool IsDead()
        {
            return isDeadFlag;
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer">描画クラス</param>
        public virtual void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position);
        }

        /// <summary>
        /// 位置の受け渡し
        /// （引数で渡された変数に自分の位置を渡す）
        /// </summary>
        /// <param name="other">位置を送りたい相手</param>
        public void SetPosition(ref Vector2 other)
        {
            other = position;
        }

    }
}
