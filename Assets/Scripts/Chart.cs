using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts {
    /// <summary>
    /// 当前棋谱
    /// </summary>

    public class Chart {
        /// <summary>
        /// 棋子的位置
        /// </summary>
        public sbyte[] ChessPointKeys = new sbyte[32];

        /// <summary>
        /// 点上面的棋子
        /// </summary>
        public Dictionary<sbyte, sbyte> PointKey2ChessDict;

        /// <summary>
        /// 是否红方下
        /// </summary>
        private bool mIsRedPlayChess = true;
        public bool IsRedPlayChess {
            get {
                return mIsRedPlayChess;
            }
        }

        public Chart() {
            //红旗先下
            mIsRedPlayChess = true;
            //添加红棋
            ChessPointKeys[0] = BoardTools.GetPointKey(new Vector2Byte(0, -4));
            ChessPointKeys[1] = BoardTools.GetPointKey(new Vector2Byte(-1, -4));
            ChessPointKeys[2] = BoardTools.GetPointKey(new Vector2Byte(1, -4));
            ChessPointKeys[3] = BoardTools.GetPointKey(new Vector2Byte(-2, -4));
            ChessPointKeys[4] = BoardTools.GetPointKey(new Vector2Byte(2, -4));
            ChessPointKeys[5] = BoardTools.GetPointKey(new Vector2Byte(-3, -4));
            ChessPointKeys[6] = BoardTools.GetPointKey(new Vector2Byte(3, -4));
            ChessPointKeys[7] = BoardTools.GetPointKey(new Vector2Byte(-4, -4));
            ChessPointKeys[8] = BoardTools.GetPointKey(new Vector2Byte(4, -4));
            ChessPointKeys[9] = BoardTools.GetPointKey(new Vector2Byte(-3, -2));
            ChessPointKeys[10] = BoardTools.GetPointKey(new Vector2Byte(3, -2));
            ChessPointKeys[11] = BoardTools.GetPointKey(new Vector2Byte(-4, -1));
            ChessPointKeys[12] = BoardTools.GetPointKey(new Vector2Byte(-2, -1));
            ChessPointKeys[13] = BoardTools.GetPointKey(new Vector2Byte(0, -1));
            ChessPointKeys[14] = BoardTools.GetPointKey(new Vector2Byte(2, -1));
            ChessPointKeys[15] = BoardTools.GetPointKey(new Vector2Byte(4, -1));

            //添加黑棋
            ChessPointKeys[16] = BoardTools.GetPointKey(new Vector2Byte(0, 5));
            ChessPointKeys[17] = BoardTools.GetPointKey(new Vector2Byte(1, 5));
            ChessPointKeys[18] = BoardTools.GetPointKey(new Vector2Byte(-1, 5));
            ChessPointKeys[19] = BoardTools.GetPointKey(new Vector2Byte(2, 5));
            ChessPointKeys[20] = BoardTools.GetPointKey(new Vector2Byte(-2, 5));
            ChessPointKeys[21] = BoardTools.GetPointKey(new Vector2Byte(3, 5));
            ChessPointKeys[22] = BoardTools.GetPointKey(new Vector2Byte(-3, 5));
            ChessPointKeys[23] = BoardTools.GetPointKey(new Vector2Byte(4, 5));
            ChessPointKeys[24] = BoardTools.GetPointKey(new Vector2Byte(-4, 5));
            ChessPointKeys[25] = BoardTools.GetPointKey(new Vector2Byte(3, 3));
            ChessPointKeys[26] = BoardTools.GetPointKey(new Vector2Byte(-3, 3));
            ChessPointKeys[27] = BoardTools.GetPointKey(new Vector2Byte(4, 2));
            ChessPointKeys[28] = BoardTools.GetPointKey(new Vector2Byte(2, 2));
            ChessPointKeys[29] = BoardTools.GetPointKey(new Vector2Byte(0, 2));
            ChessPointKeys[30] = BoardTools.GetPointKey(new Vector2Byte(-2, 2));
            ChessPointKeys[31] = BoardTools.GetPointKey(new Vector2Byte(-4, 2));
            
            UpdatePointKeyDict();
        }

        public Chart(Chart chart) {
            mIsRedPlayChess = chart.mIsRedPlayChess;
            Array.Copy(chart.ChessPointKeys, ChessPointKeys, ChessPointKeys.Length);
            UpdatePointKeyDict();
        }


        public static Chart Clone(Chart chart) {
            return new Chart(chart);
        }

        public string GetChartKey() {
            byte[] bytes = new byte[ChessPointKeys.Length + 1];

            bytes[0] = (byte)(IsRedPlayChess ? 0 : 1);
            for (int i = 0; i < ChessPointKeys.Length; i++) {
                bytes[i + 1] = (byte)(ChessPointKeys[i] + 128);
            }
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 更新字典
        /// </summary>
        public void UpdatePointKeyDict() {
            if (null == PointKey2ChessDict) {
                PointKey2ChessDict = new Dictionary<sbyte, sbyte>();
            }
            PointKey2ChessDict.Clear();
            for (int i = 0; i < ChessPointKeys.Length; i++) {
                if (-1 == ChessPointKeys[i]) {
                    //阵亡
                    continue;
                }
                if (PointKey2ChessDict.ContainsKey(ChessPointKeys[i])) {
                    continue;
                }
                PointKey2ChessDict.Add(ChessPointKeys[i], (sbyte)i);
            }
        }

        /// <summary>
        /// 获取点上的象棋
        /// </summary>
        /// <param name="point"></param>
        /// <param name="chessID"></param>
        /// <returns></returns>
        public bool GetChessByPoint(Vector2Byte point, out sbyte chessID) {
            chessID = -1;
            sbyte pointKey = BoardTools.GetPointKey(point);
            if (!PointKey2ChessDict.ContainsKey(pointKey)) {
                return false;
            }
            chessID = PointKey2ChessDict[pointKey];
            return true;
        }

        /// <summary>
        /// 判断point是否有棋子
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool PointHasChess(Vector2Byte point) {
            sbyte chessID;
            return GetChessByPoint(point, out chessID);
        }

        public Vector2Byte GetChessPoint(sbyte chessID) {
            if (-1 == ChessPointKeys[chessID]) {
                return null;
            }
            return BoardTools.GetPointByKey(ChessPointKeys[chessID]);
        }


        /// <summary>
        /// 能否停留
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsCanStay(bool isRedChess, Vector2Byte point) {
            if (!BoardTools.IsInBoard(point)) {
                return false;
            }
            sbyte chessID;
            if (GetChessByPoint(point, out chessID)) {
                return isRedChess != BoardTools.IsRedChess(chessID);
            }
            return true;
        }

        /// <summary>
        /// 是否过河
        /// </summary>
        /// <returns></returns>
        public bool IsPassRiver(sbyte chessID) {
            Vector2Byte point = GetChessPoint(chessID);
            bool isRedChess = BoardTools.IsRedChess(chessID);
            if (isRedChess) {
                return 0 < point.y;
            } else {
                return point.y < 1;
            }
        }

        /// <summary>
        /// 是否在将军
        /// </summary>
        /// <param name="isRedChess"></param>
        /// <returns></returns>
        public bool IsJiangJun(bool isRedChess) {
            Vector2Byte enemyShuaiPoint = GetShuaiPoint(!isRedChess);
            for (int i = 0; i < 16; i++) {
                sbyte tempID = (sbyte)(i | (isRedChess ? 0 : 16));
                List<Vector2Byte> tempPoints = GetMovePoints(tempID);
                for (int k = 0; k < tempPoints.Count; k++) {
                    if (enemyShuaiPoint.IsEqules(tempPoints[k])) {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否绝杀
        /// </summary>
        /// <param name="isRedChess"></param>
        /// <returns></returns>
        public bool IsJueSha(bool isRedChess) {
            for (int i = 0; i < 16; i++) {
                sbyte chessID = (sbyte)(i | (isRedChess ? 16 : 0));
                List<Vector2Byte> tempPoints = GetMovePoints(chessID);
                for (int k = 0; k < tempPoints.Count; k++) {
                    //判断移动之后是否还是被将军
                    Chart chart = Chart.Clone(this);
                    chart.MoveChess(chessID, tempPoints[k]);
                    if (!chart.IsJiangJun(isRedChess)) {
                        return false;   
                    }
                }
            }
            return true;
        }

        public Vector2Byte GetShuaiPoint(bool isRedChess) {
            return GetChessPoint((sbyte)(isRedChess ? 0 : 16));
        }

        /// <summary>
        /// 获取棋谱评分
        /// </summary>
        /// <returns></returns>
        public int GetScore(bool isRedChess) {
            int redScore = 0;
            for (int i = 0; i < 16; i++) {
                redScore += GetChessScore((sbyte)i);
            }
            int blockScore = 0;
            for (int i = 16; i < 32; i++) {
                blockScore += GetChessScore((sbyte)i);
            }
            return isRedChess ? (redScore - blockScore) : (blockScore - redScore);
        }

        /// <summary>
        /// 获取棋子评分
        /// </summary>
        /// <returns></returns>
        public int GetChessScore(sbyte chessID) {
            bool isRedChess = BoardTools.IsRedChess(chessID);
            ChessType chessType = BoardTools.GetChessType(chessID);
            if (null == GetChessPoint(chessID)) {
                return 0;
            }
            if (chessType == ChessType.Shuai) {
                return 10000;
            } else if (chessType == ChessType.Shi) {
                Vector2Byte enemyChe1 = GetChessPoint((sbyte)(isRedChess ? 23 : 7));
                Vector2Byte enemyChe2 = GetChessPoint((sbyte)(isRedChess ? 24 : 8));
                if (null != enemyChe1 && null != enemyChe2) {
                    return 400;
                } else if (null == enemyChe1 && null == enemyChe2) {
                    return 150;
                } else {
                    return 300;
                }
            } else if (chessType == ChessType.Xiang) {
                Vector2Byte enemyPao1 = GetChessPoint((sbyte)(isRedChess ? 25 : 9));
                Vector2Byte enemyPao2 = GetChessPoint((sbyte)(isRedChess ? 26 : 10));
                if (null != enemyPao1 && null != enemyPao2) {
                    return 300;
                } else if (null == enemyPao1 && null == enemyPao2) {
                    return 150;
                } else {
                    return 200;
                }
            } else if (chessType == ChessType.Ma) {
                return 500;
            } else if (chessType == ChessType.Che) {
                return 1000;
            } else if (chessType == ChessType.Pao) {
                return 500;
            } else {
                return IsPassRiver(chessID) ? 200 : 100;
            }
        }

        public void MoveChess(sbyte chessID, Vector2Byte point) {
            sbyte oldChessID;
            if (GetChessByPoint(point, out oldChessID)) {
                //吃掉棋子
                ChessPointKeys[oldChessID] = -1;
            }
            //移动棋子
            ChessPointKeys[chessID] = BoardTools.GetPointKey(point);
            mIsRedPlayChess = !mIsRedPlayChess;
            UpdatePointKeyDict();
        }


        /// <summary>
        /// 获取可以走的点
        /// </summary>
        /// <param name="chessID"></param>
        /// <returns></returns>
        public List<Vector2Byte> GetMovePoints(sbyte chessID) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            if (ChessPointKeys.Length <= chessID) {
                return result;
            }
            Vector2Byte point = GetChessPoint(chessID);
            if (null == point) {
                //阵亡
                return result;
            }
            ChessType chessType = BoardTools.GetChessType(chessID);
            if (chessType == ChessType.Shuai) {
                result = GetMovePoints_Shuai(this, chessID, point);
            } else if (chessType == ChessType.Shi) {
                result = GetMovePoints_Shi(this, chessID, point);
            } else if (chessType == ChessType.Xiang) {
                result = GetMovePoints_Xiang(this, chessID, point);
            } else if (chessType == ChessType.Ma) {
                result = GetMovePoints_Ma(this, chessID, point);
            } else if (chessType == ChessType.Che) {
                result = GetMovePoints_Che(this, chessID, point);
            } else if (chessType == ChessType.Pao) {
                result = GetMovePoints_Pao(this, chessID, point);
            } else if (chessType == ChessType.Bing) {
                result = GetMovePoints_Bing(this, chessID, point);
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
        public static List<Vector2Byte> GetMovePoints_Shuai(Chart chart, sbyte chessID, Vector2Byte point) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            bool isRedChess = BoardTools.IsRedChess(chessID);
            for (int i = 0; i < MoveDir_Shuai.Count; i++) {
                Vector2Byte newPoint = point + MoveDir_Shuai[i];
                if (newPoint.x < -1 || 1 < newPoint.x || (!chart.IsCanStay(isRedChess, newPoint))) {
                    continue;
                }
                if (isRedChess) {
                    if (-2 < newPoint.y) {
                        continue;
                    }
                } else{
                    if (newPoint.y < 3) {
                        continue;
                    }
                }
                result.Add(newPoint);
            }
            //飞将的情况
            Vector2Byte enemyShuaiPoint = chart.GetChessPoint((sbyte)(chessID ^ 16));
            if (enemyShuaiPoint.x == point.x) {
                bool canFly = true;
                int startY = Math.Min(point.y, enemyShuaiPoint.y);
                int endY = Math.Max(point.y, enemyShuaiPoint.y);
                for (int i = startY + 1; i < endY; i++) {
                    if (chart.PointHasChess(new Vector2Byte(point.x, i))) {
                        canFly = false;
                        break;
                    }
                }
                if (canFly) {
                    result.Add(enemyShuaiPoint);
                }
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
        public static List<Vector2Byte> GetMovePoints_Shi(Chart chart, sbyte chessID, Vector2Byte point) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            bool isRedChess = BoardTools.IsRedChess(chessID);
            for (int i = 0; i < MoveDir_Shi.Count; i++) {
                Vector2Byte newPoint = point + MoveDir_Shi[i];
                if (newPoint.x < -1 || 1 < newPoint.x || (!chart.IsCanStay(isRedChess, newPoint))) {
                    continue;
                }
                if (isRedChess) {
                    if (-2 < newPoint.y) {
                        continue;
                    }
                } else {
                    if (newPoint.y < 3) {
                        continue;
                    }
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
        /// <summary>
        /// 相眼向量
        /// </summary>
        private static List<Vector2Byte> XiangEyeDir = new List<Vector2Byte>() {
            new Vector2Byte(1, 1),
            new Vector2Byte(-1, 1),
            new Vector2Byte(-1, -1),
            new Vector2Byte(1, -1),
        };

        public static List<Vector2Byte> GetMovePoints_Xiang(Chart chart, sbyte chessID, Vector2Byte point) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            bool isRedChess = BoardTools.IsRedChess(chessID);
            for (int i = 0; i < MoveDir_Xiang.Count; i++) {
                Vector2Byte newPoint = point + MoveDir_Xiang[i];
                Vector2Byte xiangEye = point + XiangEyeDir[i];
                if (!chart.IsCanStay(isRedChess, newPoint)) {
                    continue;
                }
                if (chart.PointHasChess(xiangEye)) {
                    continue;
                }
                if (isRedChess) {
                    if (0 < newPoint.y) {
                        continue;
                    }
                } else {
                    if (newPoint.y < 1) {
                        continue;
                    }
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
        /// <summary>
        /// 马脚
        /// </summary>
        private static List<Vector2Byte> MaFootDir = new List<Vector2Byte>() {
            new Vector2Byte(1, 0),
            new Vector2Byte(0, 1),
            new Vector2Byte(0, 1),
            new Vector2Byte(-1, 0),
            new Vector2Byte(-1, 0),
            new Vector2Byte(0, -1),
            new Vector2Byte(0, -1),
            new Vector2Byte(1, 0)
        };
        public static List<Vector2Byte> GetMovePoints_Ma(Chart chart, sbyte chessID, Vector2Byte point) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            bool isRedChess = BoardTools.IsRedChess(chessID);
            for (int i = 0; i < MoveDir_Ma.Count; i++) {
                Vector2Byte newPoint = point + MoveDir_Ma[i];
                Vector2Byte maFoot = point + MaFootDir[i];
                if (!chart.IsCanStay(isRedChess, newPoint)) {
                    continue;
                }
                if (chart.PointHasChess(maFoot)) {
                    continue;
                }
                result.Add(newPoint);
            }
            return result;
        }


        /// <summary>
        /// 获取车的移动点
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="chessID"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static List<Vector2Byte> GetMovePoints_Che(Chart chart, sbyte chessID, Vector2Byte point) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            bool isRedChess = BoardTools.IsRedChess(chessID);
            //x方向移动
            for (int i = 1; i <= 8; i++) {
                Vector2Byte newPoint = point + new Vector2Byte(i, 0);
                if (!CheckPoint_Che(chart, isRedChess, result, newPoint)) {
                    break;
                }
            }
            for (int i = 1; i <= 8; i++) {
                Vector2Byte newPoint = point + new Vector2Byte(-i, 0);
                if (!CheckPoint_Che(chart, isRedChess, result, newPoint)) {
                    break;
                }
            }
            //y方向移动
            for (int i = 1; i <= 9; i++) {
                Vector2Byte newPoint = point + new Vector2Byte(0, i);
                if (!CheckPoint_Che(chart, isRedChess, result, newPoint)) {
                    break;
                }
            }
            for (int i = 1; i <= 9; i++) {
                Vector2Byte newPoint = point + new Vector2Byte(0, -i);
                if (!CheckPoint_Che(chart, isRedChess, result, newPoint)) {
                    break;
                }
            }
            return result;
        }

        private static bool CheckPoint_Che(Chart chart, bool isRedChess, List<Vector2Byte> resultList, Vector2Byte point) {
            if (!chart.IsCanStay(isRedChess, point)) {
                return false;
            }
            resultList.Add(point);
            if (chart.PointHasChess(point)) {
                return false;
            }
            return true;
        }

        public static List<Vector2Byte> GetMovePoints_Pao(Chart chart, sbyte chessID, Vector2Byte point) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            bool isRedChess = BoardTools.IsRedChess(chessID);
            bool hasHill = false;
            //x方向移动
            for (int i = 1; i <= 8; i++) {
                Vector2Byte newPoint = point + new Vector2Byte(i, 0);
                if (!BoardTools.IsInBoard(newPoint)) {
                    break;
                }
                if (!CheckPoint_Pao(chart, isRedChess, result, newPoint, hasHill)) {
                    if (hasHill) {
                        break;
                    }
                    hasHill = true;
                }
            }
            hasHill = false;
            for (int i = 1; i <= 8; i++) {
                Vector2Byte newPoint = point + new Vector2Byte(-i, 0);
                if (!BoardTools.IsInBoard(newPoint)) {
                    break;
                }
                if (!CheckPoint_Pao(chart, isRedChess, result, newPoint, hasHill)) {
                    if (hasHill) {
                        break;
                    }
                    hasHill = true;
                }
            }

            //y方向移动
            hasHill = false;
            for (int i = 1; i <= 9; i++) {
                Vector2Byte newPoint = point + new Vector2Byte(0, i);
                if (!BoardTools.IsInBoard(newPoint)) {
                    break;
                }
                if (!CheckPoint_Pao(chart, isRedChess, result, newPoint, hasHill)) {
                    if (hasHill) {
                        break;
                    }
                    hasHill = true;
                }
            }
            hasHill = false;
            for (int i = 1; i <= 9; i++) {
                Vector2Byte newPoint = point + new Vector2Byte(0, -i);
                if(!BoardTools.IsInBoard(newPoint)){
                    break;
                }
                if (!CheckPoint_Pao(chart, isRedChess, result, newPoint, hasHill)) {
                    if (hasHill) {
                        break;
                    }
                    hasHill = true;
                }
            }
            return result;
        }

        private static bool CheckPoint_Pao(Chart chart, bool isRedChess, List<Vector2Byte> resultList, Vector2Byte point, bool hasHill) {
            sbyte chessID;
            bool hasChess = chart.GetChessByPoint(point, out chessID);
            if (hasHill) {
                //翻过山了
                if (hasChess) {
                    if (isRedChess != BoardTools.IsRedChess(chessID)) {
                        //可以打
                        resultList.Add(point);
                    }
                    return false;
                }
            } else {
                if (hasChess) {
                    return false;
                }
                resultList.Add(point);
            }
            return true;
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

        public static List<Vector2Byte> GetMovePoints_Bing(Chart chart, sbyte chessID, Vector2Byte point) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            bool isRedChess = BoardTools.IsRedChess(chessID);
            for (int i = 0; i < MoveDir_Bing.Count; i++) {
                Vector2Byte moveDir = MoveDir_Bing[i];
                if (isRedChess) {
                    if (-1 == moveDir.y) {
                        //兵不能后退
                        continue;
                    }
                } else {
                    if (1 == moveDir.y) {
                        //卒不能后退
                        continue;
                    }
                }
                if (!chart.IsPassRiver(chessID) && 0 != moveDir.x) {
                    //没过河的兵卒不能左右
                    continue;
                }
                result.Add(point + moveDir);
            }
            return result;
        }
    }
}
