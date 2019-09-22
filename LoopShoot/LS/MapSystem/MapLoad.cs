using LS.Actor;
using LS.Device;
using LS.Scene;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.MapSystem
{
    public enum Map
    {
        None,
        Tower,
        TurnPointTop,
        TurnPointBottom,
        TurnPointLeft,
        TurnPointRight,
        SpawnU,
        SpawnD,
        SpawnL,
        SpawnR,
    }
    class MapLoad
    {
        public static int MapWidth = 50;
        public static int MapHeight = 50;

        public static int CellSize = 64;

        public static Map[,] map;

        public int mapId = 1;
        public static int currentMapId = 1;

        public List<TurnPoint> point = new List<TurnPoint>();
        public Vector2 tower;

        public Dictionary<string, List<Vector2>> spawnP = new Dictionary<string, List<Vector2>>();
        public List<Vector2> SpU = new List<Vector2>();
        public List<Vector2> SpD = new List<Vector2>();
        public List<Vector2> SpL = new List<Vector2>();
        public List<Vector2> SpR = new List<Vector2>();


        /// <summary>
        /// テキストからマップを読み込む
        /// </summary>
        /// <param name="mapId">読み込むマップの№</param>
        public void LoadMap(int mapId)
        {
            this.mapId = mapId;
            //マップ情報格納用の2次元配列を生成
            map = new Map[MapWidth, MapHeight];
            tower = Vector2.Zero; //初期化
            point.Clear();
            spawnP.Clear();
            SpU.Clear();
            SpD.Clear();
            SpL.Clear();
            SpR.Clear();
            //ファイル名を指定して、マップデータを読み込む。
            //ReadAllLines()を使うと、1行ごとの配列で読み込まれる。
            string[] lines = File.ReadAllLines("Map/map" + mapId + ".txt");
            //行のループ（縦）
            for (int y = 0; y < lines.Length; y++)
            {
                //見たい1行分の文字列を取り出す
                string line = lines[y];
                //文字のループ（横）
                for (int x = 0; x < line.Length; x++)
                {
                    //文字列からx文字目の文字を取り出す。
                    char c = line[x];
                    // 取り出した文字に応じて、マップ情報をセットする
                    switch (c)
                    {
                        case ' ':
                            map[x, y] = Map.None;
                            break;
                        case '@':
                            map[x, y] = Map.Tower;
                            break;
                        case 'T':
                            map[x, y] = Map.TurnPointTop;
                            break;
                        case 'B':
                            map[x, y] = Map.TurnPointBottom;
                            break;
                        case 'L':
                            map[x, y] = Map.TurnPointLeft;
                            break;
                        case 'R':
                            map[x, y] = Map.TurnPointRight;
                            break;
                        case 'W':
                            map[x, y] = Map.SpawnU;
                            break;
                        case 'A':
                            map[x, y] = Map.SpawnL;
                            break;
                        case 'S':
                            map[x, y] = Map.SpawnD;
                            break;
                        case 'D':
                            map[x, y] = Map.SpawnR;
                            break;
                        default:
                            throw new System.Exception("不正な文字が混入してます：" + c);
                    }
                }
            }
        }

        /// <summary>
        /// マップの描画
        /// </summary>
        /// <param name="renderer">描画クラス</param>
        public void Draw(Renderer renderer)
        {
            // マップ情報の描画。
            // 2次元配列を1マスずつ走査して、壁やゴールを描画していく
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    // 描画位置を計算
                    Vector2 position = new Vector2(CellSize * x, CellSize * y);

                    if (map[x, y] == Map.None)
                        renderer.DrawTexture("field", position);
                    //else if (map[x, y] == Map.TurnPointTop
                    //    || map[x, y] == Map.TurnPointBottom
                    //    || map[x, y] == Map.TurnPointLeft
                    //    || map[x, y] == Map.TurnPointRight)
                    //    renderer.DrawTexture("michi", position);
                    else
                        renderer.DrawTexture("michi", position);

                }
            }
        }

        /// <summary>
        /// エネミーの通り道を計算して渡す
        /// </summary>
        /// <returns></returns>
        public List<TurnPoint> CreateTP()
        {
            for (int y = 0; y < MapHeight; y++)
                for (int x = 0; x < MapWidth; x++)
                    if (map[x, y] == Map.TurnPointTop
                        || map[x, y] == Map.SpawnD)
                        point.Add(new TurnPoint(new Vector2(x * CellSize, y * CellSize), TurnPoint.MyDirection.Up));
                    else if (map[x, y] == Map.TurnPointBottom
                        || map[x, y] == Map.SpawnU)
                        point.Add(new TurnPoint(new Vector2(x * CellSize, y * CellSize), TurnPoint.MyDirection.Down));
                    else if (map[x, y] == Map.TurnPointLeft
                        || map[x, y] == Map.SpawnR)
                        point.Add(new TurnPoint(new Vector2(x * CellSize, y * CellSize), TurnPoint.MyDirection.Left));
                    else if (map[x, y] == Map.TurnPointRight
                        || map[x, y] == Map.SpawnL)
                        point.Add(new TurnPoint(new Vector2(x * CellSize, y * CellSize), TurnPoint.MyDirection.Right));
            return point;
        }

        /// <summary>
        /// タワーの生成場所を計算して渡す
        /// </summary>
        /// <returns></returns>
        public Vector2 CreateTower()
        {
            for (int y = 0; y < MapHeight; y++)
                for (int x = 0; x < MapWidth; x++)
                    if (map[x, y] == Map.Tower)
                        tower = new Vector2(x * CellSize, y * CellSize);
            return tower;
        }

        /// <summary>
        /// エネミーの出現場所を計算して渡す
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<Vector2>> CreateSpawnP()
        {
            for (int y = 0; y < MapHeight; y++)
                for (int x = 0; x < MapWidth; x++)
                {
                    if (map[x, y] == Map.SpawnU)
                        SpU.Add(new Vector2(x * CellSize, y * CellSize));
                    else if (map[x, y] == Map.SpawnL)
                        SpL.Add(new Vector2(x * CellSize, y * CellSize));
                    else if (map[x, y] == Map.SpawnD)
                        SpD.Add(new Vector2(x * CellSize, y * CellSize));
                    else if (map[x, y] == Map.SpawnR)
                        SpR.Add(new Vector2(x * CellSize, y * CellSize));
                }
            spawnP.Add("上", SpU);
            spawnP.Add("左", SpL);
            spawnP.Add("下", SpD);
            spawnP.Add("右", SpR);
            return spawnP;
        }
    }
}
