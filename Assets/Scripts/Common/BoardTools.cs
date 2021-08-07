using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common {
    public static class BoardTools {
        private static byte[] BinaryOneCount = new byte[1024];

        private static Vector2[,] PointToPositionArray = new Vector2[9,10];
        private static ChessType[] ChessTypeArray = new ChessType[32];
        private static sbyte[,] PointKeyArray = new sbyte[9,10];
        private static bool[,] InJiuGongArray = new bool[9,10];
        private static Vector2Byte[] PointByKeyArray = new Vector2Byte[90];
        private static List<Vector2Byte>[,] MovePointsList = new List<Vector2Byte>[32, 90];

        /// <summary>
        /// 初始化所有预处理数据
        /// </summary>
        public static void InitData() {
            for (int i = 0; i < 1024; i++) {
                int temp = i;
                BinaryOneCount[i] = 0;
                while (0 < temp) {
                    BinaryOneCount[i] += (byte)(temp & 1);
                    temp >>= 1;
                }
            }
            for (int i = 0; i < 9; i++) {
                for (int k = 0; k < 10; k++) {
                    PointToPositionArray[i, k] = new Vector2(i, k);
                }
            }
            for (int i = 0; i < 32; i++) {
                if (0x0B <= (i & 0x0F)) {
                    ChessTypeArray[i] = ChessType.Bing;
                } else {
                    ChessTypeArray[i] = (ChessType)(((i & 0x0F) + 1) >> 1);
                }
            }
            for (int i = 0; i < 9; i++) {
                for (int k = 0; k < 10; k++) {
                    PointKeyArray[i, k] = (sbyte)(i * 10 + k);
                    PointByKeyArray[PointKeyArray[i, k]] = new Vector2Byte(i, k);
                }
            }
            for (int i = 0; i < 9; i++) {
                for (int k = 0; k < 10; k++) {
                    InJiuGongArray[i, k] = (3 <= i && i <= 5) && ((0 <= k && k <= 2) || (7 <= k && k <= 9));
                }
            }
            for (sbyte i = 0; i < 32; i++) {
                for (sbyte k = 0; k < 90; k++) {
                    MovePointsList[i, k] = CalcMovePoints(i, k);
                }
            }
        }

        public static byte GetBinaryOneCount(int number) {
            return BinaryOneCount[number];
        }

        /// <summary>
        /// 转换到坐标
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector2 PointToPosition(Vector2Byte point) {
            return PointToPositionArray[point.x, point.y];
        }

        /// <summary>
        /// 是否是红棋
        /// </summary>
        /// <param name="chessID"></param>
        /// <returns></returns>
        public static bool IsRedChess(sbyte chessID) {
            return (0 == (chessID & 0x10));
        }

        public static bool IsInBoard(Vector2Byte point) {
            if (point.x < 0 || 8 < point.x || point.y < 0 || 9 < point.y) {
                return false;
            }
            return true;
        }

        public static bool IsInJiuGong(Vector2Byte point) {
            return InJiuGongArray[point.x, point.y];
        }

        public static bool IsInRedRange(Vector2Byte point) {
            return point.y <= 4;
        }

        public static ChessType GetChessType(sbyte chessID) {
            return ChessTypeArray[chessID];
        }

        public static sbyte GetPointKey(Vector2Byte point) {
            return PointKeyArray[point.x, point.y];
        }

        public static Vector2Byte GetPointByKey(sbyte pointKey) {
            return PointByKeyArray[pointKey];
        }


        /// <summary>
        /// 获取棋子能到达的点
        /// </summary>
        /// <param name="chessID"></param>
        /// <param name="pointKey"></param>
        /// <returns></returns>
        public static List<Vector2Byte> GetMovePoints(sbyte chessID, sbyte pointKey) {
            return MovePointsList[chessID, pointKey];
        }

        /// <summary>
        /// 计算可以到达的点
        /// </summary>
        /// <param name="chessID"></param>
        /// <returns></returns>
        public static List<Vector2Byte> CalcMovePoints(sbyte chessID, sbyte pointKey) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            Vector2Byte point = GetPointByKey(pointKey);
            ChessType chessType = GetChessType(chessID);
            if (chessType == ChessType.Shuai) {
                result = CalcMovePoints_Shuai(chessID, point);
            } else if (chessType == ChessType.Shi) {
                result = CalcMovePoints_Shi(chessID, point);
            } else if (chessType == ChessType.Xiang) {
                result = CalcMovePoints_Xiang(chessID, point);
            } else if (chessType == ChessType.Ma) {
                result = CalcMovePoints_Ma(chessID, point);
            } else if (chessType == ChessType.Che) {
                result = CalcMovePoints_Che(chessID, point);
            } else if (chessType == ChessType.Pao) {
                result = CalcMovePoints_Pao(chessID, point);
            } else if (chessType == ChessType.Bing) {
                result = CalcMovePoints_Bing(chessID, point);
            }
            return result;
        }

        /// <summary>
        /// 帅将的移动向量
        /// </summary>
        private static List<Vector2Byte> MoveDir_Shuai = new List<Vector2Byte>() {
            new Vector2Byte(1, 0),
            new Vector2Byte(0, 1),
            new Vector2Byte(-1, 0),
            new Vector2Byte(0, -1)
        };
        public static List<Vector2Byte> CalcMovePoints_Shuai(sbyte chessID, Vector2Byte point) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            for (int i = 0; i < MoveDir_Shuai.Count; i++) {
                Vector2Byte newPoint = point + MoveDir_Shuai[i];
                if ((!IsInBoard(newPoint)) || (!IsInJiuGong(newPoint))) {
                    continue;
                }
                result.Add(newPoint);
            }
            return result;
        }

        /// <summary>
        /// 士的移动向量
        /// </summary>
        private static List<Vector2Byte> MoveDir_Shi = new List<Vector2Byte>() {
            new Vector2Byte(1, 1),
            new Vector2Byte(-1, 1),
            new Vector2Byte(-1, -1),
            new Vector2Byte(1, -1)
        };
        public static List<Vector2Byte> CalcMovePoints_Shi(sbyte chessID, Vector2Byte point) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            for (int i = 0; i < MoveDir_Shi.Count; i++) {
                Vector2Byte newPoint = point + MoveDir_Shi[i];
                if ((!IsInBoard(newPoint)) || (!IsInJiuGong(newPoint))) {
                    continue;
                }
                result.Add(newPoint);
            }
            return result;
        }


        /// <summary>
        /// 相的移动向量
        /// </summary>
        private static List<Vector2Byte> MoveDir_Xiang = new List<Vector2Byte>() {
            new Vector2Byte(2, 2),
            new Vector2Byte(-2, 2),
            new Vector2Byte(-2, -2),
            new Vector2Byte(2, -2),
        };

        public static List<Vector2Byte> CalcMovePoints_Xiang(sbyte chessID, Vector2Byte point) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            for (int i = 0; i < MoveDir_Xiang.Count; i++) {
                Vector2Byte newPoint = point + MoveDir_Xiang[i];
                if (!IsInBoard(newPoint)) {
                    continue;
                }
                if (IsRedChess(chessID) != IsInRedRange(newPoint)) {
                    continue;
                }
                result.Add(newPoint);
            }
            return result;
        }


        /// <summary>
        /// 马的移动向量
        /// </summary>
        private static List<Vector2Byte> MoveDir_Ma = new List<Vector2Byte>() {
            new Vector2Byte(2, 1),
            new Vector2Byte(1, 2),
            new Vector2Byte(-1, 2),
            new Vector2Byte(-2, 1),
            new Vector2Byte(-2, -1),
            new Vector2Byte(-1, -2),
            new Vector2Byte(1, -2),
            new Vector2Byte(2, -1)
        };

        public static List<Vector2Byte> CalcMovePoints_Ma(sbyte chessID, Vector2Byte point) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            for (int i = 0; i < MoveDir_Ma.Count; i++) {
                Vector2Byte newPoint = point + MoveDir_Ma[i];
                if (!IsInBoard(newPoint)) {
                    continue;
                }
                result.Add(newPoint);
            }
            return result;
        }

        public static List<Vector2Byte> CalcMovePoints_Che(sbyte chessID, Vector2Byte point) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            //x方向移动
            for (int i = 1; i <= 8; i++) {
                Vector2Byte newPoint = point + new Vector2Byte(i, 0);
                if (!IsInBoard(newPoint)) {
                    break;
                }
                result.Add(newPoint);
            }
            for (int i = 1; i <= 8; i++) {
                Vector2Byte newPoint = point + new Vector2Byte(-i, 0);
                if (!IsInBoard(newPoint)) {
                    break;
                }
                result.Add(newPoint);
            }
            //y方向移动
            for (int i = 1; i <= 9; i++) {
                Vector2Byte newPoint = point + new Vector2Byte(0, i);
                if (!IsInBoard(newPoint)) {
                    break;
                }
                result.Add(newPoint);
            }
            for (int i = 1; i <= 9; i++) {
                Vector2Byte newPoint = point + new Vector2Byte(0, -i);
                if (!IsInBoard(newPoint)) {
                    break;
                }
                result.Add(newPoint);
            }
            return result;
        }

        public static List<Vector2Byte> CalcMovePoints_Pao(sbyte chessID, Vector2Byte point) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            //x方向移动
            for (int i = 1; i <= 8; i++) {
                Vector2Byte newPoint = point + new Vector2Byte(i, 0);
                if (!IsInBoard(newPoint)) {
                    break;
                }
                result.Add(newPoint);
            }
            for (int i = 1; i <= 8; i++) {
                Vector2Byte newPoint = point + new Vector2Byte(-i, 0);
                if (!IsInBoard(newPoint)) {
                    break;
                }
                result.Add(newPoint);
            }
            //y方向移动
            for (int i = 1; i <= 9; i++) {
                Vector2Byte newPoint = point + new Vector2Byte(0, i);
                if (!IsInBoard(newPoint)) {
                    break;
                }
                result.Add(newPoint);
            }
            for (int i = 1; i <= 9; i++) {
                Vector2Byte newPoint = point + new Vector2Byte(0, -i);
                if (!IsInBoard(newPoint)) {
                    break;
                }
                result.Add(newPoint);
            }
            return result;
        }

        /// <summary>
        /// 兵的移动向量
        /// </summary>
        private static List<Vector2Byte> MoveDir_Bing = new List<Vector2Byte>() {
            new Vector2Byte(1, 0),
            new Vector2Byte(0, 1),
            new Vector2Byte(-1, 0),
            new Vector2Byte(0, -1)
        };
        public static List<Vector2Byte> CalcMovePoints_Bing(sbyte chessID, Vector2Byte point) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            bool isRedChess = IsRedChess(chessID);
            for (int i = 0; i < MoveDir_Bing.Count; i++) {
                Vector2Byte newPoint = point + MoveDir_Bing[i];
                if (!IsInBoard(newPoint)) {
                    continue;
                }
                if (isRedChess) {
                    if (-1 == MoveDir_Bing[i].y) {
                        continue;
                    }
                    if (IsInRedRange(newPoint) && 0 != MoveDir_Bing[i].x) {
                        continue;
                    }
                }
                if ((!isRedChess)) {
                    if (-1 == MoveDir_Bing[i].y) {
                        continue;
                    }
                    if ((!IsInRedRange(newPoint)) && 0 != MoveDir_Bing[i].x) {
                        continue;
                    }
                }
                result.Add(newPoint);
            }
            return result;
        }


        public static Dictionary<ChessType, string[]> ChessName = new Dictionary<ChessType, string[]>() {
            {ChessType.Shuai,   new string[]{ "帅", "将" }},
            {ChessType.Shi,     new string[]{ "仕", "士" }},
            {ChessType.Xiang,   new string[]{ "相", "象" }},
            {ChessType.Ma,      new string[]{ "馬", "马" }},
            {ChessType.Che,     new string[]{ "車", "车" }},
            {ChessType.Pao,     new string[]{ "軳", "炮" }},
            {ChessType.Bing,    new string[]{ "兵", "卒" }}
        };
        public static string PrintStep(sbyte chessID, sbyte pointKey1, sbyte pointKey2) {
            StringBuilder result = new StringBuilder();
            bool isRedChess = BoardTools.IsRedChess(chessID);
            ChessType chessType = BoardTools.GetChessType(chessID);
            result.Append(ChessName[chessType][isRedChess ? 0 : 1]);
            Vector2Byte point1 = BoardTools.GetPointByKey(pointKey1);
            Vector2Byte point2 = BoardTools.GetPointByKey(pointKey2);

            result.Append(GetXIndexName(isRedChess, point1.x).ToString());
            if (point1.y == point2.y) {
                result.Append("平");
            } else if (point1.y < point2.y) {
                result.Append(isRedChess ? "进" : "退");
            } else {
                result.Append(isRedChess ? "退" : "进");
            }
            if (chessType == ChessType.Shuai || chessType == ChessType.Che || chessType == ChessType.Pao || chessType == ChessType.Bing) {
                if (point1.y == point2.y) {
                    result.Append(GetXIndexName(isRedChess, point2.x).ToString());
                } else {
                    result.Append(Math.Abs(point1.y - point2.y).ToString());
                }
            } else {
                result.Append(GetXIndexName(isRedChess, point2.x).ToString());
            }
            return result.ToString();
        }
        public static string GetXIndexName(bool isRedChess, int index) {
            index += 5;
            if (!isRedChess) {
                index = 10 - index;
            }
            return index.ToString();
        }
    }
}
