using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common {
    public static class BoardTools {
        /// <summary>
        /// 检测顺序
        /// </summary>
        public static int[] ChessCheckOrder = new int[] { 7, 8, 5, 6, 9, 10, 3, 4, 1, 2, 11, 12, 13, 14, 15, 0 };
        /// <summary>
        /// 棋子在某个点的分数分数加成
        /// </summary>
        public static int[,] ChessInPointScore = new int[,] {
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0,10, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0,10,20,10, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0,30,50,30, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            },{
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0,20, 0,20, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0,50, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0,30, 0,30, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            },{
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0,30, 0, 0, 0,30, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0,10, 0, 0, 0,50, 0, 0, 0,10, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0,30, 0, 0, 0,30, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            },{
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 5,20,20,50,50,50,20,20, 5, 0, 0, 0, 0,
                0, 0, 0, 5,20,80,50,20,50,80,20, 5, 0, 0, 0, 0,
                0, 0, 0, 5,20,60,80,50,80,60,20, 5, 0, 0, 0, 0,
                0, 0, 0, 5,50,20,20,20,20,20,50, 5, 0, 0, 0, 0,
                0, 0, 0, 5,20,40,20,40,20,40,20, 5, 0, 0, 0, 0,
                0, 0, 0,10,10,10,10,20,10,10,10,10, 0, 0, 0, 0,
                0, 0, 0, 5,20, 0,20,10,20,10,20, 5, 0, 0, 0, 0,
                0, 0, 0, 5,10,10,10,10,10,10,10, 5, 0, 0, 0, 0,
                0, 0, 0, 5,10,10,10,10,10,10,10, 5, 0, 0, 0, 0,
                0, 0, 0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            },{
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0,50,50,50,50,50,50,50,50,50, 0, 0, 0, 0,
                0, 0, 0,30,40,40,40,40,40,40,40,30, 0, 0, 0, 0,
                0, 0, 0,30,40,40,40,40,40,40,40,30, 0, 0, 0, 0,
                0, 0, 0,30,50,50,50,50,50,50,50,30, 0, 0, 0, 0,
                0, 0, 0,20,40,40,40,40,40,40,40,20, 0, 0, 0, 0,
                0, 0, 0,30,50,50,50,50,50,50,50,30, 0, 0, 0, 0,
                0, 0, 0,30,40,40,40,40,40,40,40,30, 0, 0, 0, 0,
                0, 0, 0,30,30,30,30,40,30,30,30,30, 0, 0, 0, 0,
                0, 0, 0,30,30,30,30,30,30,30,30,30, 0, 0, 0, 0,
                0, 0, 0,30,30,30,30,30,30,30,30,30, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            },{
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,
                0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,
                0 ,0 ,0,50,30,30,20,20,10,10,10,10,10 ,0 ,0 ,0,
                0 ,0 ,0,50,30,30,20,20,20,10,10,10,10 ,0 ,0 ,0,
                0 ,0 ,0,40,30,30,20,20,10,10,20,10,10 ,0 ,0 ,0,
                0 ,0 ,0,10,10,10,20,20,20,10,10,10,10 ,0 ,0 ,0,
                0 ,0 ,0,10,10,10,50,50,50,50,10,10,10 ,0 ,0 ,0,
                0 ,0 ,0,10,10,10,20,20,20,10,10,10,10 ,0 ,0 ,0,
                0 ,0 ,0,40,40,40,20,20,10,10,20,10,10 ,0 ,0 ,0,
                0 ,0 ,0,50,40,40,20,20,20,10,10,10,10 ,0 ,0 ,0,
                0 ,0 ,0,50,30,30,20,20,10,10,10,10,10 ,0 ,0 ,0,
                0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,
                0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,
                0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,
                0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,
            },{
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0,10,10,10,20,20,20,10,10,10, 0, 0, 0, 0,
                0, 0, 0,10,20,30,00,80,60,30,20,10, 0, 0, 0, 0,
                0, 0, 0,10,30,60,60,60,60,20,10,10, 0, 0, 0, 0,
                0, 0, 0,30,30,30,30,50,30,30,30,30, 0, 0, 0, 0,
                0, 0, 0,20,20,20,20,50,20,20,20,20, 0, 0, 0, 0,
                0, 0, 0,10, 0,20, 0,50, 0,20, 0,10, 0, 0, 0, 0,
                0, 0, 0,10, 0,20, 0,50, 0,20, 0,10, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            }
        };

        /// <summary>
        /// 除了士象其他棋子都会收影响的点
        /// </summary>
        public static int[] AllAffectedPoints = new int[] {
            16, 1, -16, -1
        };
        /// <summary>
        /// 士受影响的点
        /// </summary>
        public static int[] ShiAffectedPoints = new int[] {
            17, -15, -17, 15
        };

        /// <summary>
        /// 象受影响的点
        /// </summary>
        public static int[] XiangAffectedPoints = new int[] {
            17, 34, -15, -30, -17, -34, 15, 30
        };

        /// <summary>
        /// 马受影响的点
        /// </summary>
        public static int[] MaAffectedPoints = new int[] {
            33, 18, -14, -31, -33, -18, 14, 31
        };

        /// <summary>
        /// 点是否在棋盘内
        /// </summary>
        private static bool[] PointIsInBoard = new bool[256];
        /// <summary>
        /// 数字的二进制中一个数量
        /// </summary>
        private static byte[] BinaryOneCount = new byte[65536];
        /// <summary>
        /// 棋盘的坐标转到Point
        /// </summary>
        private static int[,] PositionToPointArray = new int[16, 16];
        /// <summary>
        /// Point转到棋盘的坐标
        /// </summary>
        private static Vector2Int[] PointToPositionArray = new Vector2Int[256];
        /// <summary>
        /// 棋子类型
        /// </summary>
        private static ChessType[] ChessTypeArray = new ChessType[32];
        /// <summary>
        /// 是否在九宫内
        /// </summary>
        private static bool[] InJiuGongArray = new bool[256];
        /// <summary>
        /// 马脚point
        /// </summary>
        private static int[,] MaFootPointArray = new int[256, 256];
        /// <summary>
        /// 可移动点的数组
        /// </summary>
        private static List<int>[,] MovePointsList = new List<int>[32, 256];

        /// <summary>
        /// 初始化所有预处理数据
        /// </summary>
        public static void InitData() {
            for (int i = 0; i < 65536; i++) {
                int temp = i;
                BinaryOneCount[i] = 0;
                while (0 < temp) {
                    BinaryOneCount[i] += (byte)(temp & 1);
                    temp >>= 1;
                }
            }
            for (int i = 0; i < 16; i++) {
                for (int k = 0; k < 16; k++) {
                    PositionToPointArray[i, k] = ((i << 4) | k);
                    PointToPositionArray[GetPointByPosition(i, k)] = new Vector2Int(i, k);
                }
            }
            for (int i = 0; i < 16; i++) {
                for (int k = 0; k < 16; k++) {
                    PointIsInBoard[GetPointByPosition(i, k)] = (3 <= i && i <= 11 && 3 <= k && k <= 12);
                }
            }

            for (int i = 0; i < 32; i++) {
                if (0x0B <= (i & 0x0F)) {
                    ChessTypeArray[i] = ChessType.Bing;
                } else {
                    ChessTypeArray[i] = (ChessType)(((i & 0x0F) + 1) >> 1);
                }
            }

            for (int i = 0; i < 16; i++) {
                for (int k = 0; k < 16; k++) {
                    InJiuGongArray[GetPointByPosition(i, k)] = (6 <= i && i <= 8) && ((3 <= k && k <= 5) || (10 <= k && k <= 12));
                }
            }
            for (int i = 0; i < 255; i++) {
                for (int k = 0; k < 255; k++) {
                    Vector2Int aPoint = new Vector2Int(i >> 4, i & 0xF);
                    Vector2Int bPoint = new Vector2Int(k >> 4, k & 0xF);

                    if (3 != Math.Abs(aPoint.x - bPoint.x) + Math.Abs(aPoint.y - bPoint.y) || aPoint.x == bPoint.x || aPoint.y == bPoint.y) {
                        MaFootPointArray[i, k] = i;
                    } else {
                        Vector2Int footPoint;
                        if (1 == Math.Abs(aPoint.x - bPoint.x)) {
                            footPoint = new Vector2Int(aPoint.x, aPoint.y + (aPoint.y < bPoint.y ? 1 : -1));
                        } else {
                            footPoint = new Vector2Int(aPoint.x + (aPoint.x < bPoint.x ? 1 : -1), aPoint.y);
                        }
                        MaFootPointArray[i, k] = GetPointByPosition(footPoint.x, footPoint.y);
                    }
                }
            }
            for (int i = 0; i < 32; i++) {
                for (int k = 0; k < 256; k++) {
                    if (!IsInBoard(k)) {
                        continue;
                    }
                    MovePointsList[i, k] = CalcMovePoints(i, k);
                }
            }
        }

        /// <summary>
        /// 1024以内数据二进制中1的个数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static byte GetBinaryOneCount(int number) {
            return BinaryOneCount[number];
        }

        public static int GetPointByPosition(int x, int y) {
            return PositionToPointArray[x, y];
        }

        public static int GetMaFootPoint(int aPoint, int bPoint) {
            return MaFootPointArray[aPoint, bPoint];
        }

        /// <summary>
        /// 转换到坐标
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector2Int PointToPosition(int point) {
            return PointToPositionArray[point];
        }

        /// <summary>
        /// 是否是红棋
        /// </summary>
        /// <param name="chessID"></param>
        /// <returns></returns>
        public static bool IsRedChess(int chessID) {
            return (0 == (chessID & 0x10));
        }

        public static bool IsInBoard(int pointKey) {
            return PointIsInBoard[pointKey];
        }

        public static bool IsInJiuGong(int pointKey) {
            return InJiuGongArray[pointKey];
        }

        public static bool IsInRedRange(int pointKey) {
            return (pointKey & 0x0F) <= 7;
        }

        public static ChessType GetChessType(int chessID) {
            return ChessTypeArray[chessID];
        }

        public static int GetChessPointScore(int chessID, int point) {
            bool isRedChess = IsRedChess(chessID);
            int chessType = (int)GetChessType(chessID);
            Vector2Int pointPos = PointToPosition(point);
            point = (pointPos.y << 4) | (pointPos.x);
            if (!isRedChess) {
                return ChessInPointScore[chessType, point];
            } else {
                point = GetPointByPosition(15 - (point >> 4), (point & 0xF));
                return ChessInPointScore[chessType, point];
            }
        }

        /// <summary>
        /// 获取棋子能到达的点
        /// </summary>
        /// <param name="chessID"></param>
        /// <param name="pointKey"></param>
        /// <returns></returns>
        public static List<int> GetMovePoints(int chessID, int pointKey) {
            return MovePointsList[chessID, pointKey];
        }

        /// <summary>
        /// 计算可以到达的点
        /// </summary>
        /// <param name="chessID"></param>
        /// <returns></returns>
        public static List<int> CalcMovePoints(int chessID, int pointKey) {
            List<int> result = new List<int>();
            ChessType chessType = GetChessType(chessID);
            if (chessType == ChessType.Shuai) {
                result = CalcMovePoints_Shuai(chessID, pointKey);
            } else if (chessType == ChessType.Shi) {
                result = CalcMovePoints_Shi(chessID, pointKey);
            } else if (chessType == ChessType.Xiang) {
                result = CalcMovePoints_Xiang(chessID, pointKey);
            } else if (chessType == ChessType.Ma) {
                result = CalcMovePoints_Ma(chessID, pointKey);
            } else if (chessType == ChessType.Che) {
                result = CalcMovePoints_Che(chessID, pointKey);
            } else if (chessType == ChessType.Pao) {
                result = CalcMovePoints_Pao(chessID, pointKey);
            } else if (chessType == ChessType.Bing) {
                result = CalcMovePoints_Bing(chessID, pointKey);
            }
            return result;
        }

        /// <summary>
        /// 帅将的移动向量
        /// </summary>
        private static int[] MoveDir_Shuai = new int[] {
            16, 1, -16, -1
        };
        public static List<int> CalcMovePoints_Shuai(int chessID, int point) {
            List<int> result = new List<int>();
            for (int i = 0; i < MoveDir_Shuai.Length; i++) {
                int newPoint = point + MoveDir_Shuai[i];
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
        private static int[] MoveDir_Shi = new int[] {
            17, -15, -17, 15
        };
        public static List<int> CalcMovePoints_Shi(int chessID, int point) {
            List<int> result = new List<int>();
            for (int i = 0; i < MoveDir_Shi.Length; i++) {
                int newPoint = point + MoveDir_Shi[i];
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
        private static int[] MoveDir_Xiang = new int[] {
            34, -30, -34, 30,
        };

        public static List<int> CalcMovePoints_Xiang(int chessID, int point) {
            List<int> result = new List<int>();
            for (int i = 0; i < MoveDir_Xiang.Length; i++) {
                int newPoint = point + MoveDir_Xiang[i];
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
        private static int[] MoveDir_Ma = new int[] {
            33, 18, -14, -31, -33, -18, 14, 31
        };

        public static List<int> CalcMovePoints_Ma(int chessID, int point) {
            List<int> result = new List<int>();
            for (int i = 0; i < MoveDir_Ma.Length; i++) {
                int newPoint = point + MoveDir_Ma[i];
                if (!IsInBoard(newPoint)) {
                    continue;
                }
                result.Add(newPoint);
            }
            return result;
        }

        public static List<int> CalcMovePoints_Che(int chessID, int point) {
            List<int> result = new List<int>();
            //x方向移动
            for (int i = 1; i <= 8; i++) {
                int newPoint = point + (i * 16);
                if (!IsInBoard(newPoint)) {
                    break;
                }
                result.Add(newPoint);
            }
            for (int i = 1; i <= 8; i++) {
                int newPoint = point - (i * 16);
                if (!IsInBoard(newPoint)) {
                    break;
                }
                result.Add(newPoint);
            }
            //y方向移动
            for (int i = 1; i <= 9; i++) {
                int newPoint = point +i;
                if (!IsInBoard(newPoint)) {
                    break;
                }
                result.Add(newPoint);
            }
            for (int i = 1; i <= 9; i++) {
                int newPoint = point - i;
                if (!IsInBoard(newPoint)) {
                    break;
                }
                result.Add(newPoint);
            }
            return result;
        }

        public static List<int> CalcMovePoints_Pao(int chessID, int point) {
            List<int> result = new List<int>();
            //x方向移动
            for (int i = 1; i <= 8; i++) {
                int newPoint = point + (i * 16);
                if (!IsInBoard(newPoint)) {
                    break;
                }
                result.Add(newPoint);
            }
            for (int i = 1; i <= 8; i++) {
                int newPoint = point - (i * 16);
                if (!IsInBoard(newPoint)) {
                    break;
                }
                result.Add(newPoint);
            }
            //y方向移动
            for (int i = 1; i <= 9; i++) {
                int newPoint = point + i;
                if (!IsInBoard(newPoint)) {
                    break;
                }
                result.Add(newPoint);
            }
            for (int i = 1; i <= 9; i++) {
                int newPoint = point - i;
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
        private static int[] MoveDir_Bing = new int[] {
            16, 1, -16, -1
        };
        public static List<int> CalcMovePoints_Bing(int chessID, int point) {
            List<int> result = new List<int>();
            bool isRedChess = IsRedChess(chessID);
            for (int i = 0; i < MoveDir_Bing.Length; i++) {
                int newPoint = point + MoveDir_Bing[i];
                if (!IsInBoard(newPoint)) {
                    continue;
                }
                if (isRedChess) {
                    if (-1 == MoveDir_Bing[i]) {
                        continue;
                    }
                    if (IsInRedRange(newPoint) && 1 != Math.Abs(MoveDir_Bing[i])) {
                        continue;
                    }
                }
                if (!isRedChess) {
                    if (1 == MoveDir_Bing[i]) {
                        continue;
                    }
                    if ((!IsInRedRange(newPoint)) && 1 != Math.Abs(MoveDir_Bing[i])) {
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
        public static string PrintStep(int chessID, int point1, int point2) {
            StringBuilder result = new StringBuilder();
            bool isRedChess = BoardTools.IsRedChess(chessID);
            ChessType chessType = BoardTools.GetChessType(chessID);
            result.Append(ChessName[chessType][isRedChess ? 0 : 1]);
            int point1Y = (point1 & 0x0F);
            int point2Y = (point2 & 0x0F);

            result.Append(GetXIndexName(isRedChess, (point1 >> 4)).ToString());
            if (point1Y == point2Y) {
                result.Append("平");
            } else if (point1Y < point2Y) {
                result.Append(isRedChess ? "进" : "退");
            } else {
                result.Append(isRedChess ? "退" : "进");
            }
            if (chessType == ChessType.Shuai || chessType == ChessType.Che || chessType == ChessType.Pao || chessType == ChessType.Bing) {
                if (point1Y == point2Y) {
                    result.Append(GetXIndexName(isRedChess, (point2 >> 4)).ToString());
                } else {
                    result.Append(Math.Abs(point1Y - point2Y).ToString());
                }
            } else {
                result.Append(GetXIndexName(isRedChess, (point2 >> 4)).ToString());
            }
            return result.ToString();
        }
        public static string GetXIndexName(bool isRedChess, int index) {
            if (!isRedChess) {
                index = 14 - index;
            }
            index -= 2;
            return index.ToString();
        }
    }
}
