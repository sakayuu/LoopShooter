using LS.Device;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Scene
{
    class SceneManager
    {
        //シーン管理用ディクショナリ
        private Dictionary<Scene, IScene> scenes = new Dictionary<Scene, IScene>();
        //現在のシーン
        private IScene currentScene = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SceneManager() { }

        /// <summary>
        /// シーンの追加
        /// </summary>
        /// <param name="name">シーン名</param>
        /// <param name="scene">具体的なシーンオブジェクト</param>
        public void Add(Scene name, IScene scene)
        {
            //すでにシーン名が登録されていたら
            if (scenes.ContainsKey(name))
            {
                return;//何もしない
            }
            scenes.Add(name, scene);//シーンの追加
        }

        public void Change(Scene name)
        {
            //何かシーンが登録されていたら
            if (currentScene != null)
            {
                currentScene.Shutdown();//現在のシーンの終了処理
            }

            //ディクショナリから次のシーンを取り出し
            //現在のシーンに設定
            currentScene = scenes[name];
            //シーンの初期化
            currentScene.Initialize();
        }

        public void Update(GameTime gameTime)
        {
            //シーンが登録されていないか？
            if (currentScene == null) { return; }//何もしない
            //現在のシーンの更新
            currentScene.Update(gameTime);
            //現在のシーンが終了しているか？
            if (currentScene.IsEnd())
            {
                Change(currentScene.Next());//次のシーンを取り出し、シーン切り替え
            }
        }

        public void Draw(Renderer renderer)
        {
            //現在のシーンがまだないか？
            if (currentScene == null) { return; }//何もしない
            //現在のシーンを描画
            currentScene.Draw(renderer);
        }
    }
}
