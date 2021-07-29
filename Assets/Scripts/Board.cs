using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts {
    public class Board  : SingleInstance<Board>{
        [SerializeField]
        private List<ChessBase> AllChess = new List<ChessBase>();

        private Dictionary<int, ChessBase> Point2ChessDict = new Dictionary<int, ChessBase>();

        /// <summary>
        /// 是否红方下
        /// </summary>
        private bool mIsRedPlayChess = true;

        private ChessBase SelectedChess = null;

        private void Start() {
            for (int i = 0; i < AllChess.Count; i++) {
                int pointKey = BoardTools.GetPointKey(AllChess[i].PosPoint);
                Point2ChessDict.Add(pointKey, AllChess[i]);
            }
        }

        public ChessBase GetChessByPoint(Vector2Int point) {
            int pointKey = BoardTools.GetPointKey(point);
            if (!Point2ChessDict.ContainsKey(pointKey)) {
                return null;
            }
            if (Point2ChessDict[pointKey].IsDeath) {
                return null;
            }
            return Point2ChessDict[pointKey];
        }
    }
}
