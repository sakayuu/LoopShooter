using LS.Device;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.MapSystem
{
    class MapLoad
    {
        enum Map
        {
            None,
            Way,
            Tower,
        }

        public static int MapWidth = 100;
        public static int MapHeight = 100;

        public static int CellSize = 64;

        private static Map[,] map;

        public int mapId = 1;
        public static int currentMapId = 1;

        public void LoadMap(int mapId)
        {
            this.mapId = mapId;
            //マップ情報格納用の2次元配列を生成
            map = new Map[MapWidth, MapHeight];

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
                    if (c == ' ')
                        map[x, y] = Map.None;
                    else if (c == 'W')
                        map[x, y] = Map.Way;
                    else if (c == 'T')
                        map[x, y] = Map.Tower;
                    else
                        throw new System.Exception("不正な文字が混入してます：" + c);
                }
            }
        }

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

                    if (map[x, y] == Map.Way)
                    {
                        renderer.DrawTexture("Way", position);
                    }
                }
            }
        }
    }
}
