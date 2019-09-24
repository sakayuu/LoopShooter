// このファイルで必要なライブラリのnamespaceを指定
using LS.Actor;
using LS.Def;
using LS.Device;
using LS.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

/// <summary>
/// プロジェクト名がnamespaceとなります
/// </summary>
namespace LS
{
    /// <summary>
    /// ゲームの基盤となるメインのクラス
    /// 親クラスはXNA.FrameworkのGameクラス
    /// </summary>
    public class Game1 : Game
    {
        // フィールド（このクラスの情報を記述）
        private GraphicsDeviceManager graphicsDeviceManager;//グラフィックスデバイスを管理するオブジェクト
        private SpriteBatch spriteBatch;//画像をスクリーン上に描画するためのオブジェクト
        private SceneManager sceneManager;
        private GameDevice gameDevice;
        private Renderer renderer;

        private List<Pillar> pillars;

        /// <summary>
        /// コンストラクタ
        /// （new で実体生成された際、一番最初に一回呼び出される）
        /// </summary>
        public Game1()
        {
            //グラフィックスデバイス管理者の実体生成
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            //コンテンツデータ（リソースデータ）のルートフォルダは"Contentに設定
            Content.RootDirectory = "Content";

            graphicsDeviceManager.PreferredBackBufferWidth = Screen.Width;
            graphicsDeviceManager.PreferredBackBufferHeight = Screen.Height;

            IsMouseVisible = true; //マウスの表示

            Window.Title = "LoopShooter";
        }

        /// <summary>
        /// 初期化処理（起動時、コンストラクタの後に1度だけ呼ばれる）
        /// </summary>
        protected override void Initialize()
        {
            // この下にロジックを記述
            gameDevice = GameDevice.Instance(Content, GraphicsDevice);

            sceneManager = new SceneManager();
            sceneManager.Add(Scene.Scene.Title, new SceneFader(new Title()));
            sceneManager.Add(Scene.Scene.StageSelect, new SceneFader(new StageSelect()));
            IScene addScene = new GamePlay();

            sceneManager.Add(Scene.Scene.GamePlay, addScene);
            sceneManager.Add(Scene.Scene.Ending, new SceneFader(new Ending(addScene)));
            sceneManager.Add(Scene.Scene.GameClear, new SceneFader(new GameClear(addScene)));

            sceneManager.Change(Scene.Scene.Title);

            // この上にロジックを記述
            base.Initialize();// 親クラスの初期化処理呼び出し。絶対に消すな！！
        }

        /// <summary>
        /// コンテンツデータ（リソースデータ）の読み込み処理
        /// （起動時、１度だけ呼ばれる）
        /// </summary>
        protected override void LoadContent()
        {
            // 画像を描画するために、スプライトバッチオブジェクトの実体生成
            renderer = gameDevice.GetRenderer();

            // この下にロジックを記述
            string filepath = "./Texture/";
            renderer.LoadContent("black", filepath);
            renderer.LoadContent("ending", filepath);
            renderer.LoadContent("number", filepath);
            renderer.LoadContent("particle", filepath);
            renderer.LoadContent("particleBlue", filepath);
            renderer.LoadContent("particleSmall", filepath);
            renderer.LoadContent("BG", filepath);
            renderer.LoadContent("white", filepath);
            renderer.LoadContent("oikake_enemy_4anime", filepath);
            renderer.LoadContent("LS", filepath);
            renderer.LoadContent("tower", filepath);
            renderer.LoadContent("HeartLife", filepath);
            renderer.LoadContent("michi", filepath);
            renderer.LoadContent("bullet", filepath);
            renderer.LoadContent("enemy", filepath);
            renderer.LoadContent("pillar", filepath);
            renderer.LoadContent("stage", filepath);
            renderer.LoadContent("good", filepath);
            renderer.LoadContent("field", filepath);
            renderer.LoadContent("Way", filepath);
            renderer.LoadContent("titleBG", filepath);
            renderer.LoadContent("titleback", filepath);
            renderer.LoadContent("titleLogo", filepath);
            renderer.LoadContent("titleStart", filepath);
            renderer.LoadContent("selectBG", filepath);
            renderer.LoadContent("selectLogo", filepath);
            renderer.LoadContent("1", filepath);
            renderer.LoadContent("2", filepath);
            renderer.LoadContent("3", filepath);
            renderer.LoadContent("gameclear", filepath);
            renderer.LoadContent("gameover", filepath);
            renderer.LoadContent("clickUI", filepath);



            //1ピクセルの黒画像（シーンフェーダー用）
            Texture2D fade = new Texture2D(GraphicsDevice, 1, 1);
            Color[] colors = new Color[1 * 1];
            colors[0] = new Color(0, 0, 0);
            fade.SetData(colors);
            renderer.LoadContent("fade", fade);


            Sound sound = gameDevice.GetSound();
            string filepath2 = "./Sound/";
            sound.LoadBGM("congratulation", filepath2);
            sound.LoadBGM("endingbgm", filepath2);
            sound.LoadBGM("gameplaybgm", filepath2);
            sound.LoadBGM("resultBGM", filepath2);
            sound.LoadBGM("titlebgm", filepath2);
            sound.LoadSE("endingse", filepath2);
            sound.LoadSE("gameplayse", filepath2);
            sound.LoadSE("titlese", filepath2);

            
            sound.LoadBGM("build", filepath2);
            sound.LoadBGM("damage", filepath2);
            sound.LoadBGM("hit", filepath2);
            sound.LoadBGM("shot", filepath2);



            // この上にロジックを記述
        }

        /// <summary>
        /// コンテンツの解放処理
        /// （コンテンツ管理者以外で読み込んだコンテンツデータを解放）
        /// </summary>
        protected override void UnloadContent()
        {
            // この下にロジックを記述


            // この上にロジックを記述
        }

        /// <summary>
        /// 更新処理
        /// （1/60秒の１フレーム分の更新内容を記述。音再生はここで行う）
        /// </summary>
        /// <param name="gameTime">現在のゲーム時間を提供するオブジェクト</param>
        protected override void Update(GameTime gameTime)
        {
            // ゲーム終了処理（ゲームパッドのBackボタンかキーボードのエスケープボタンが押されたら終了）
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) ||
                 (Keyboard.GetState().IsKeyDown(Keys.Escape)))
            {
                Exit();
            }


            // この下に更新ロジックを記述
            gameDevice.Update(gameTime);

            sceneManager.Update(gameTime);
            // この上にロジックを記述
            base.Update(gameTime); // 親クラスの更新処理呼び出し。絶対に消すな！！
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="gameTime">現在のゲーム時間を提供するオブジェクト</param>
        protected override void Draw(GameTime gameTime)
        {
            // 画面クリア時の色を設定
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // この下に描画ロジックを記述
            sceneManager.Draw(renderer);

            //この上にロジックを記述
            base.Draw(gameTime); // 親クラスの更新処理呼び出し。絶対に消すな！！
        }
    }
}
