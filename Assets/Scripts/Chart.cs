using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts {


    public class Chart {
        /// <summary>
        /// 棋子的位置
        /// </summary>
        public List<byte> ChessPointKeys = new List<byte>();

        /// <summary>
        /// 点上面的棋子
        /// </summary>
        public Dictionary<byte, byte> PointKey2ChessDict;

        public static Chart Clone(Chart chart) {
            Chart result = new Chart();
            for (int i = 0; i < chart.ChessPointKeys.Count; i++) {
                result.ChessPointKeys.Add(chart.ChessPointKeys[i]);
            }
            return result;
        }

        /// <summary>
        /// 更新字典
        /// </summary>
        public void UpdatePointKeyDict() {
            if (null == PointKey2ChessDict) {
                PointKey2ChessDict = new Dictionary<byte, byte>();
            }
            PointKey2ChessDict.Clear();
            for (int i = 0; i < ChessPointKeys.Count; i++) {
                if (byte.MaxValue == ChessPointKeys[i]) {
                    //阵亡
                    continue;
                }
                PointKey2ChessDict.Add(ChessPointKeys[i], (byte)i);
            }
        }

        /// <summary>
        /// 获取点上的象棋
        /// </summary>
        /// <param name="point"></param>
        /// <param name="chessID"></param>
        /// <returns></returns>
        public bool GetChessByPoint(Vector2Byte point, out byte chessID) {
            chessID = byte.MaxValue;
            byte pointKey = BoardTools.GetPointKey(point);
            if (!PointKey2ChessDict.ContainsKey(pointKey)) {
                return false;
            }
            chessID = PointKey2ChessDict[pointKey];
            return true;
        }

        public Vector2Byte GetChessPoint(byte chessID) {
            if (byte.MaxValue == ChessPointKeys[chessID]) {
                return null;
            }
            return BoardTools.GetPointByKey(ChessPointKeys[chessID]);
        }

        /// <summary>
        /// 获取可以走的点
        /// </summary>
        /// <param name="chessID"></param>
        /// <returns></returns>
        public List<Vector2Byte> GetMovePoints(byte chessID) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            if (ChessPointKeys.Count <= chessID) {
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
        public static List<Vector2Byte> GetMovePoints_Shuai(Chart chart, byte chessID, Vector2Byte point) {
            List<Vector2Byte> result = new List<Vector2Byte>();
            for (int i = 0; i < MoveDir_Shuai.Count; i++) {
                Vector2Int newPoint = point + MoveDir_Shuai[i];
                if (newPoint.x < -1 || 1 < newPoint.x || -2 < newPoint.y || (!IsCanStay(newPoint))) {
                    continue;
                }
                result.Add(newPoint);
            }


            return result;
        }
    }
}
