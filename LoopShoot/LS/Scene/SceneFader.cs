﻿using LS.Def;
using LS.Device;
using LS.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Scene
{
    class SceneFader : IScene
    {
        /// <summary>
        /// フェードシーン状態の列挙型
        /// </summary>
        private enum SceneFaderState
        {
            In,
            Out,
            None
        };

        private Timer timer;//フェード時間
        private readonly float FADE_TIME = 0.5f;//1.0＝１秒で
        private SceneFaderState state;//状態
        private IScene scene;//現在のシーン
        private bool isEndFlag = false;//終了フラグ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="scene">シーン名</param>
        public SceneFader(IScene scene) { this.scene = scene; }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            switch (state)
            {
                case SceneFaderState.In:
                    DrawFadeIn(renderer);
                    break;
                case SceneFaderState.Out:
                    DrawFadeOut(renderer);
                    break;
                case SceneFaderState.None:
                    DrawFadeNone(renderer);
                    break;
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            scene.Initialize();
            state = SceneFaderState.In;
            timer = new CountDownTimer(FADE_TIME);
            isEndFlag = false;
        }

        /// <summary>
        /// 終了か？
        /// </summary>
        /// <returns></returns>
        public bool IsEnd() { return isEndFlag; }

        /// <summary>
        /// 次のシーン名の取得
        /// </summary>
        /// <returns></returns>
        public Scene Next() { return scene.Next(); }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Shutdown() { scene.Shutdown(); }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="gameTime">ゲーム時間</param>
        public void Update(GameTime gameTime)
        {
            switch (state)
            {
                case SceneFaderState.In:
                    UpdateFadeIn(gameTime);
                    break;
                case SceneFaderState.Out:
                    UpdateFadeOut(gameTime);
                    break;
                case SceneFaderState.None:
                    UpdateFadeNone(gameTime);
                    break;
            }
        }

        /// <summary>
        /// フェードイン状態の更新
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateFadeIn(GameTime gameTime)
        {
            scene.Update(gameTime);//シーンの更新
            if (scene.IsEnd()) { state = SceneFaderState.Out; }
            timer.Update(gameTime);//時間の更新
            if (timer.IsTime()) { state = SceneFaderState.None; }
        }

        /// <summary>
        /// フェードイン状態の描画
        /// </summary>
        /// <param name="renderer"></param>
        private void DrawFadeIn(Renderer renderer)
        {
            scene.Draw(renderer);
            DrawEffect(renderer, 1 - timer.Rate());
        }

        /// <summary>
        /// フェードアウト状態の更新
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateFadeOut(GameTime gameTime)
        {
            scene.Update(gameTime);//シーンの更新
            if (scene.IsEnd()) { state = SceneFaderState.Out; }
            timer.Update(gameTime);//時間の更新
            if (timer.IsTime()) { isEndFlag = true; }
        }

        /// <summary>
        /// フェードアウト状態の描画
        /// </summary>
        /// <param name="renderer"></param>
        private void DrawFadeOut(Renderer renderer)
        {
            scene.Draw(renderer);
            DrawEffect(renderer, timer.Rate());
        }

        /// <summary>
        /// フェードなし状態の更新
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateFadeNone(GameTime gameTime)
        {
            scene.Update(gameTime);
            if (scene.IsEnd())
            {
                state = SceneFaderState.Out;
                timer.Initialize();
            }
        }

        /// <summary>
        /// フェードなし状態の描画
        /// </summary>
        /// <param name="renderer"></param>
        private void DrawFadeNone(Renderer renderer)
        { scene.Draw(renderer); }

        /// <summary>
        /// エフェクト描画
        /// </summary>
        /// <param name="renderer">描画オブジェクト</param>
        /// <param name="alpha">透明値</param>
        private void DrawEffect(Renderer renderer, float alpha)
        {
            renderer.Begin();
            renderer.DrawTexture("fade", Vector2.Zero, null, 0.0f, Vector2.Zero,
                new Vector2(Screen.Width, Screen.Height),
                SpriteEffects.None, 0.0f, alpha);
            renderer.End();
        }

    }
}

