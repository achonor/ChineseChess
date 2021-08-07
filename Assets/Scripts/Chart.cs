using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts {

    /// <summary>
    /// 记录操作的类
    /// 表示A棋子从A点走到B点吃掉了B棋子
    /// </summary>
    public class Record {
        public sbyte AChessID;
        public sbyte APoint;
        public sbyte BChessID;
        public sbyte BPoint;
    }

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
        /// 行状态
        /// </summary>
        public int[] ChartStatusX = new int[9];

        public int[] ChartStatusY = new int[10];


        /// <summary>
        /// 是否红方下
        /// </summary>
        private bool mIsRedPlayChess = true;
        public bool IsRedPlayChess {
            get {
                return mIsRedPlayChess;
            }
        }

        private Stack<Record> RecordsStack = new Stack<Record>();
        public List<Record> Records {
            get {
                return RecordsStack.ToList();
            }
        }


        public Chart() {
            //红旗先下
            mIsRedPlayChess = true;
            //添加红棋
            ChessPointKeys[0] = BoardTools.GetPointKey(new Vector2Byte(4, 0));
            ChessPointKeys[1] = BoardTools.GetPointKey(new Vector2Byte(3, 0));
            ChessPointKeys[2] = BoardTools.GetPointKey(new Vector2Byte(5, 0));
            ChessPointKeys[3] = BoardTools.GetPointKey(new Vector2Byte(2, 0));
            ChessPointKeys[4] = BoardTools.GetPointKey(new Vector2Byte(6, 0));
            ChessPointKeys[5] = BoardTools.GetPointKey(new Vector2Byte(1, 0));
            ChessPointKeys[6] = BoardTools.GetPointKey(new Vector2Byte(7, 0));
            ChessPointKeys[7] = BoardTools.GetPointKey(new Vector2Byte(0, 0));
            ChessPointKeys[8] = BoardTools.GetPointKey(new Vector2Byte(8, 0));
            ChessPointKeys[9] = BoardTools.GetPointKey(new Vector2Byte(1, 2));
            ChessPointKeys[10] = BoardTools.GetPointKey(new Vector2Byte(7, 2));
            ChessPointKeys[11] = BoardTools.GetPointKey(new Vector2Byte(0, 3));
            ChessPointKeys[12] = BoardTools.GetPointKey(new Vector2Byte(2, 3));
            ChessPointKeys[13] = BoardTools.GetPointKey(new Vector2Byte(4, 3));
            ChessPointKeys[14] = BoardTools.GetPointKey(new Vector2Byte(6, 3));
            ChessPointKeys[15] = BoardTools.GetPointKey(new Vector2Byte(8, 3));

            //添加黑棋
            ChessPointKeys[16] = BoardTools.GetPointKey(new Vector2Byte(4, 9));
            ChessPointKeys[17] = BoardTools.GetPointKey(new Vector2Byte(5, 9));
            ChessPointKeys[18] = BoardTools.GetPointKey(new Vector2Byte(3, 9));
            ChessPointKeys[19] = BoardTools.GetPointKey(new Vector2Byte(6, 9));
            ChessPointKeys[20] = BoardTools.GetPointKey(new Vector2Byte(2, 9));
            ChessPointKeys[21] = BoardTools.GetPointKey(new Vector2Byte(7, 9));
            ChessPointKeys[22] = BoardTools.GetPointKey(new Vector2Byte(1, 9));
            ChessPointKeys[23] = BoardTools.GetPointKey(new Vector2Byte(8, 9));
            ChessPointKeys[24] = BoardTools.GetPointKey(new Vector2Byte(0, 9));
            ChessPointKeys[25] = BoardTools.GetPointKey(new Vector2Byte(7, 7));
            ChessPointKeys[26] = BoardTools.GetPointKey(new Vector2Byte(1, 7));
            ChessPointKeys[27] = BoardTools.GetPointKey(new Vector2Byte(8, 6));
            ChessPointKeys[28] = BoardTools.GetPointKey(new Vector2Byte(6, 6));
            ChessPointKeys[29] = BoardTools.GetPointKey(new Vector2Byte(4, 6));
            ChessPointKeys[30] = BoardTools.GetPointKey(new Vector2Byte(2, 6));
            ChessPointKeys[31] = BoardTools.GetPointKey(new Vector2Byte(0, 6));

            UpdatePointKeyDict();
        }

        public Chart(Chart chart) {
            mIsRedPlayChess = chart.mIsRedPlayChess;
            Array.Copy(chart.ChessPointKeys, ChessPointKeys, ChessPointKeys.Length);
            UpdatePointKeyDict();
        }

        //public override string ToString() {
        //    return Achonor.Function.ToString(ChessPointKeys, "|");
        //}

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
            for (sbyte i = 0; i < 9; i++) {
                for (sbyte k = 0; k < 10; k++) {
                    Vector2Byte point = new Vector2Byte(i, k);
                    SetChartStatus(point, PointHasChess(point));
                }
            }
        }

        public void SetChartStatus(sbyte pointKey, bool status) {
            SetChartStatus(BoardTools.GetPointByKey(pointKey), status);
        }

        public void SetChartStatus(Vector2Byte point, bool status) {
            if (status) {
                ChartStatusX[point.x] |= (1 << point.y);
                ChartStatusY[point.y] |= (1 << point.x);
            } else {
                ChartStatusX[point.x] &= ((~(1 << point.y)) & 0x3FF);
                ChartStatusY[point.y] &= ((~(1 << point.x)) & 0x1FF);
            }
        }
        /// <summary>
        /// 线段上是否有棋子
        /// </summary>
        /// <returns></returns>
        public bool LineHasChess(Vector2Byte aPoint, Vector2Byte bPoint) {
            return 0 < GetLineChessCount(aPoint, bPoint);
        }

        /// <summary>
        /// 线段上的棋子数量
        /// </summary>
        /// <param name="aPoint"></param>
        /// <param name="bPoint"></param>
        /// <returns></returns>
        public byte GetLineChessCount(Vector2Byte aPoint, Vector2Byte bPoint) {
            if (aPoint.x == bPoint.x) {
                int rangeLen = Math.Abs(aPoint.y - bPoint.y) - 1;
                return BoardTools.GetBinaryOneCount(ChartStatusX[aPoint.x] & ((0X3FF >> (10 - rangeLen)) << Math.Min(aPoint.y, bPoint.y) + 1));
            } else if (aPoint.y == bPoint.y) {
                int rangeLen = Math.Abs(aPoint.x - bPoint.x) - 1;
                return BoardTools.GetBinaryOneCount(ChartStatusY[aPoint.y] & ((0X1FF >> (10 - rangeLen)) << Math.Min(aPoint.x, bPoint.x) + 1));
            }
            return 0;
        }


        public sbyte GetChessByPointKey(sbyte pointKey) {
            if (!PointKey2ChessDict.ContainsKey(pointKey)) {
                return -1;
            }
            return PointKey2ChessDict[pointKey];
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
            return (0 != (ChartStatusX[point.x] & (1 << point.y)));
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
            int canMoveCount = GetMovePoints(chessID, false).Count;
            int selfChessCount = GetInRangeChess(chessID, isRedChess).Count;
            int enemyChessCount = GetInRangeChess(chessID, !isRedChess).Count;
            if (chessType == ChessType.Shuai) {
                return 10000 + canMoveCount * 2 + selfChessCount * 10 + enemyChessCount * 20;
            } else if (chessType == ChessType.Shi) {
                return 250 + canMoveCount * 2 + selfChessCount * 10 + enemyChessCount * 20;
            } else if (chessType == ChessType.Xiang) {
                return 200 + canMoveCount * 2 + selfChessCount * 20 + enemyChessCount * 50;
            } else if (chessType == ChessType.Ma) {
                return 450 + ((32 - PointKey2ChessDict.Count) * 4) + canMoveCount * 20 + selfChessCount * 20 + enemyChessCount * 50;
            } else if (chessType == ChessType.Che) {
                return 1000 + canMoveCount * 8 + selfChessCount * 20 + enemyChessCount * 50;
            } else if (chessType == ChessType.Pao) {
                return 500 + canMoveCount * 8 + selfChessCount * 20 + enemyChessCount * 50;
            } else {
                return 100 + canMoveCount * 20 + selfChessCount * 20 + enemyChessCount * 50;
            }
        }

        /// <summary>
        /// 移动棋子
        /// </summary>
        /// <param name="chessID"></param>
        /// <param name="point"></param>
        public void MoveChess(sbyte chessID, Vector2Byte point) {
            sbyte oldChessID = -1;
            if (GetChessByPoint(point, out oldChessID)) {
                //吃掉棋子
                ChessPointKeys[oldChessID] = -1;
            }
            //记录
            Record record = new Record();
            record.AChessID = chessID;
            record.APoint = ChessPointKeys[chessID];
            record.BChessID = oldChessID;
            record.BPoint = BoardTools.GetPointKey(point);
            RecordsStack.Push(record);

            //移动棋子
            PointKey2ChessDict.Remove(ChessPointKeys[chessID]);
            ChessPointKeys[chessID] = record.BPoint;
            Achonor.Function.Update(PointKey2ChessDict, ChessPointKeys[chessID], chessID);
            mIsRedPlayChess = !mIsRedPlayChess;
            //更新状态
            SetChartStatus(ChessPointKeys[chessID], false);
            SetChartStatus(point, true);
        }

        /// <summary>
        /// 返回上一步
        /// </summary>
        public bool BackStep() {
            if (RecordsStack.Count <= 0) {
                return false;
            }
            Record record = RecordsStack.Pop();
            if (-1 != record.BChessID) {
                ChessPointKeys[record.BChessID] = record.BPoint;
                PointKey2ChessDict[record.BPoint] = record.BChessID;
                SetChartStatus(record.BPoint, true);
            } else {
                PointKey2ChessDict.Remove(record.BPoint);
                SetChartStatus(record.BPoint, false);
            }
            ChessPointKeys[record.AChessID] = record.APoint;
            Achonor.Function.Update(PointKey2ChessDict, record.APoint, record.AChessID);
            mIsRedPlayChess = !mIsRedPlayChess;
            SetChartStatus(record.APoint, true);
            return true;
        }

        public void PrintStep() {
            StringBuilder printText = new StringBuilder();
            List<Record> records = RecordsStack.ToList();
            records.Reverse(0, RecordsStack.Count);
            for (int i = 0; i < records.Count; i++) {
                Record record = records[i];
                printText.Append("->");
                printText.Append(BoardTools.PrintStep(record.AChessID, record.APoint, record.BPoint));
            }
            Debug.Log(printText.ToString());
        }

        /// <summary>
        /// 获取可以走的点
        /// </summary>
        /// <param name="chessID"></param>
        /// <returns></returns>
        public List<Vector2Byte> GetMovePoints(sbyte chessID, bool isTrue = true) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            ChessType chessType = BoardTools.GetChessType(chessID);
            sbyte pointKey = ChessPointKeys[chessID];
            if (-1 == pointKey) {
                return result;
            }
            if (chessType == ChessType.Shuai) {
                result = GetMovePoints_Shuai(this, chessID);
            } else if (chessType == ChessType.Shi) {
                result = GetMovePoints_Shi(this, chessID);
            } else if (chessType == ChessType.Xiang) {
                result = GetMovePoints_Xiang(this, chessID);
            } else if (chessType == ChessType.Ma) {
                result = GetMovePoints_Ma(this, chessID);
            } else if (chessType == ChessType.Che) {
                result = GetMovePoints_Che(this, chessID);
            } else if (chessType == ChessType.Pao) {
                result = GetMovePoints_Pao(this, chessID);
            } else if (chessType == ChessType.Bing) {
                result = GetMovePoints_Bing(this, chessID);
            }
            return result;
        }

        /// <summary>
        /// 获取可行走范围内指定颜色的棋
        /// </summary>
        /// <param name="chessID"></param>
        /// <param name="isRedChess"></param>
        /// <returns></returns>
        public List<sbyte> GetInRangeChess(sbyte chessID, bool isRedChess) {
            List<sbyte> result = new List<sbyte>();
            if (ChessPointKeys.Length <= chessID) {
                return result;
            }
            sbyte pointKey = ChessPointKeys[chessID];
            if (-1 == pointKey) {
                //阵亡
                return result;
            }
            ChessType chessType = BoardTools.GetChessType(chessID);
            if (chessType == ChessType.Shuai) {
                result = GetInRangeChess_Shuai(this, chessID, isRedChess);
            } else if (chessType == ChessType.Shi) {
                result = GetInRangeChess_Shi(this, chessID, isRedChess);
            } else if (chessType == ChessType.Xiang) {
                result = GetInRangeChess_Xiang(this, chessID, isRedChess);
            } else if (chessType == ChessType.Ma) {
                result = GetInRangeChess_Ma(this, chessID, isRedChess);
            } else if (chessType == ChessType.Che) {
                result = GetInRangeChess_Che(this, chessID, isRedChess);
            } else if (chessType == ChessType.Pao) {
                result = GetInRangeChess_Pao(this, chessID, isRedChess);
            } else if (chessType == ChessType.Bing) {
                result = GetInRangeChess_Bing(this, chessID, isRedChess);
            }
            return result;
        }


        /// <summary>
        /// 帅将能移动的点
        /// </summary>
        public static List<Vector2Byte> GetMovePoints_Shuai(Chart chart, sbyte chessID) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            sbyte pointKey = chart.ChessPointKeys[chessID];
            List<Vector2Byte> movePoints = BoardTools.GetMovePoints(chessID, pointKey);
            for (int i = 0; i < movePoints.Count; i++) {
                Vector2Byte newPoint = movePoints[i];
                if (chart.PointHasChess(newPoint)) {
                    if (BoardTools.IsRedChess(chessID) == BoardTools.IsRedChess(chart.GetChessByPointKey(pointKey))) {
                        continue;
                    }
                }
                result.Add(newPoint);
            }
            //飞将的情况
            Vector2Byte point = BoardTools.GetPointByKey(pointKey);
            Vector2Byte enemyShuaiPoint = chart.GetChessPoint((sbyte)(chessID ^ 16));
            if (null != enemyShuaiPoint && enemyShuaiPoint.x == point.x) {
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

        public static List<sbyte> GetInRangeChess_Shuai(Chart chart, sbyte chessID, bool isRedChess) {
            List<sbyte> result = new List<sbyte>();
            sbyte pointKey = chart.ChessPointKeys[chessID];
            List<Vector2Byte> movePoints = BoardTools.GetMovePoints(chessID, pointKey);
            for (int i = 0; i < movePoints.Count; i++) {
                Vector2Byte newPoint = movePoints[i];
                sbyte tempChessID;
                if (!chart.GetChessByPoint(newPoint, out tempChessID)) {
                    continue;
                }
                if (isRedChess != BoardTools.IsRedChess(tempChessID)) {
                    continue;
                }
                result.Add(tempChessID);
            }
            return result;
        }

        public static List<Vector2Byte> GetMovePoints_Shi(Chart chart, sbyte chessID) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            sbyte pointKey = chart.ChessPointKeys[chessID];
            List<Vector2Byte> movePoints = BoardTools.GetMovePoints(chessID, pointKey);
            for (int i = 0; i < movePoints.Count; i++) {
                Vector2Byte newPoint = movePoints[i];
                if (chart.PointHasChess(newPoint)) {
                    if (BoardTools.IsRedChess(chessID) == BoardTools.IsRedChess(chart.GetChessByPointKey(pointKey))) {
                        continue;
                    }
                }
                result.Add(newPoint);
            }
            return result;
        }

        public static List<sbyte> GetInRangeChess_Shi(Chart chart, sbyte chessID, bool isRedChess) {
            List<sbyte> result = new List<sbyte>();
            sbyte pointKey = chart.ChessPointKeys[chessID];
            List<Vector2Byte> movePoints = BoardTools.GetMovePoints(chessID, pointKey);
            for (int i = 0; i < movePoints.Count; i++) {
                Vector2Byte newPoint = movePoints[i];
                sbyte tempChessID;
                if (!chart.GetChessByPoint(newPoint, out tempChessID)) {
                    continue;
                }
                if (isRedChess != BoardTools.IsRedChess(tempChessID)) {
                    continue;
                }
                result.Add(tempChessID);
            }
            return result;
        }

        public static List<Vector2Byte> GetMovePoints_Xiang(Chart chart, sbyte chessID) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            sbyte pointKey = chart.ChessPointKeys[chessID];
            List<Vector2Byte> movePoints = BoardTools.GetMovePoints(chessID, pointKey);

            Vector2Byte point = BoardTools.GetPointByKey(pointKey);
            for (int i = 0; i < movePoints.Count; i++) {
                Vector2Byte newPoint = movePoints[i];
                if (chart.PointHasChess(newPoint)) {
                    if (BoardTools.IsRedChess(chessID) == BoardTools.IsRedChess(chart.GetChessByPointKey(pointKey))) {
                        continue;
                    }
                }
                if (chart.PointHasChess((point + newPoint) / 2)) {
                    continue;
                }
                result.Add(newPoint);
            }
            return result;
        }

        public static List<sbyte> GetInRangeChess_Xiang(Chart chart, sbyte chessID, bool isRedChess) {
            List<sbyte> result = new List<sbyte>();
            sbyte pointKey = chart.ChessPointKeys[chessID];
            List<Vector2Byte> movePoints = BoardTools.GetMovePoints(chessID, pointKey);

            Vector2Byte point = BoardTools.GetPointByKey(pointKey);
            for (int i = 0; i < movePoints.Count; i++) {
                Vector2Byte newPoint = movePoints[i];
                if (chart.PointHasChess((point + newPoint) / 2)) {
                    continue;
                }
                sbyte tempChessID;
                if (!chart.GetChessByPoint(newPoint, out tempChessID)) {
                    continue;
                }
                if (isRedChess != BoardTools.IsRedChess(tempChessID)) {
                    continue;
                }
                result.Add(tempChessID);
            }
            return result;
        }

        public static List<Vector2Byte> GetMovePoints_Ma(Chart chart, sbyte chessID) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            sbyte pointKey = chart.ChessPointKeys[chessID];
            List<Vector2Byte> movePoints = BoardTools.GetMovePoints(chessID, pointKey);

            Vector2Byte point = BoardTools.GetPointByKey(pointKey);
            for (int i = 0; i < movePoints.Count; i++) {
                Vector2Byte newPoint = movePoints[i];
                if (chart.PointHasChess(newPoint)) {
                    if (BoardTools.IsRedChess(chessID) == BoardTools.IsRedChess(chart.GetChessByPointKey(pointKey))) {
                        continue;
                    }
                }
                if (chart.PointHasChess((point + newPoint) / 2)) {
                    continue;
                }
                result.Add(newPoint);
            }
            return result;
        }


        public static List<sbyte> GetInRangeChess_Ma(Chart chart, sbyte chessID, bool isRedChess) {
            List<sbyte> result = new List<sbyte>();
            sbyte pointKey = chart.ChessPointKeys[chessID];
            List<Vector2Byte> movePoints = BoardTools.GetMovePoints(chessID, pointKey);

            Vector2Byte point = BoardTools.GetPointByKey(pointKey);
            for (int i = 0; i < movePoints.Count; i++) {
                Vector2Byte newPoint = movePoints[i];
                if (chart.PointHasChess((point + newPoint) / 2)) {
                    continue;
                }
                sbyte tempChessID;
                if (!chart.GetChessByPoint(newPoint, out tempChessID)) {
                    continue;
                }
                if (isRedChess != BoardTools.IsRedChess(tempChessID)) {
                    continue;
                }
                result.Add(tempChessID);
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
        public static List<Vector2Byte> GetMovePoints_Che(Chart chart, sbyte chessID) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            sbyte pointKey = chart.ChessPointKeys[chessID];
            Vector2Byte point = BoardTools.GetPointByKey(pointKey);
            List<Vector2Byte> movePoints = BoardTools.GetMovePoints(chessID, pointKey);
            for (int i = 0; i < movePoints.Count; i++) {
                Vector2Byte newPoint = movePoints[i];
                if (chart.PointHasChess(newPoint)) {
                    if (BoardTools.IsRedChess(chessID) == BoardTools.IsRedChess(chart.GetChessByPointKey(pointKey))) {
                        continue;
                    }
                }
                if (chart.LineHasChess(point, newPoint)) {
                    continue;
                }
                result.Add(newPoint);
            }
            return result;
        }

        public static List<sbyte> GetInRangeChess_Che(Chart chart, sbyte chessID, bool isRedChess) {
            List<sbyte> result = new List<sbyte>();
            sbyte pointKey = chart.ChessPointKeys[chessID];
            Vector2Byte point = BoardTools.GetPointByKey(pointKey);
            List<Vector2Byte> movePoints = BoardTools.GetMovePoints(chessID, pointKey);
            for (int i = 0; i < movePoints.Count; i++) {
                Vector2Byte newPoint = movePoints[i];
                if (chart.LineHasChess(point, newPoint)) {
                    continue;
                }
                sbyte tempChessID;
                if (!chart.GetChessByPoint(newPoint, out tempChessID)) {
                    continue;
                }
                if (isRedChess == BoardTools.IsRedChess(tempChessID)) {
                    result.Add(tempChessID);
                }
            }
            return result;
        }

        public static List<Vector2Byte> GetMovePoints_Pao(Chart chart, sbyte chessID) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            sbyte pointKey = chart.ChessPointKeys[chessID];
            Vector2Byte point = BoardTools.GetPointByKey(pointKey);
            List<Vector2Byte> movePoints = BoardTools.GetMovePoints(chessID, pointKey);
            for (int i = 0; i < movePoints.Count; i++) {
                Vector2Byte newPoint = movePoints[i];
                byte lineChessCount = chart.GetLineChessCount(point, newPoint);
                if (1 < lineChessCount) {
                    continue;
                }
                if (1 == lineChessCount) {
                    if (!chart.PointHasChess(newPoint)) {
                        continue;
                    }
                    if (BoardTools.IsRedChess(chessID) == BoardTools.IsRedChess(chart.GetChessByPointKey(pointKey))) {
                        continue;
                    }
                }
                if (!chart.PointHasChess(newPoint)) {
                    result.Add(newPoint);
                }
            }
            return result;
        }

        public static List<sbyte> GetInRangeChess_Pao(Chart chart, sbyte chessID, bool isRedChess) {
            List<sbyte> result = new List<sbyte>();
            sbyte pointKey = chart.ChessPointKeys[chessID];
            Vector2Byte point = BoardTools.GetPointByKey(pointKey);
            List<Vector2Byte> movePoints = BoardTools.GetMovePoints(chessID, pointKey);
            for (int i = 0; i < movePoints.Count; i++) {
                Vector2Byte newPoint = movePoints[i];
                byte lineChessCount = chart.GetLineChessCount(point, newPoint);
                if (1 < lineChessCount) {
                    continue;
                }
                if (0 == lineChessCount) {
                    continue;
                }
                sbyte tempChessID;
                if (!chart.GetChessByPoint(newPoint, out tempChessID)) {
                    continue;
                }
                if (isRedChess != BoardTools.IsRedChess(tempChessID)) {
                    continue;
                }
                result.Add(tempChessID);
            }
            return result;
        }

        public static List<Vector2Byte> GetMovePoints_Bing(Chart chart, sbyte chessID) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            sbyte pointKey = chart.ChessPointKeys[chessID];
            List<Vector2Byte> movePoints = BoardTools.GetMovePoints(chessID, pointKey);
            for (int i = 0; i < movePoints.Count; i++) {
                Vector2Byte newPoint = movePoints[i];
                if (chart.PointHasChess(newPoint)) {
                    if (BoardTools.IsRedChess(chessID) == BoardTools.IsRedChess(chart.GetChessByPointKey(pointKey))) {
                        continue;
                    }
                }
                result.Add(newPoint);
            }
            return result;
        }

        public static List<sbyte> GetInRangeChess_Bing(Chart chart, sbyte chessID, bool isRedChess) {
            List<sbyte> result = new List<sbyte>();
            sbyte pointKey = chart.ChessPointKeys[chessID];
            List<Vector2Byte> movePoints = BoardTools.GetMovePoints(chessID, pointKey);
            for (int i = 0; i < movePoints.Count; i++) {
                Vector2Byte newPoint = movePoints[i];
                sbyte tempChessID;
                if (!chart.GetChessByPoint(newPoint, out tempChessID)) {
                    continue;
                }
                if (isRedChess != BoardTools.IsRedChess(tempChessID)) {
                    continue;
                }
                result.Add(tempChessID);
            }
            return result;
        }
    }
}
