using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common {
    public static class BoardTools {
        private static Vector2[,] PointToPositionArray = new Vector2[9,10];
        private static ChessType[] ChessTypeArray = new ChessType[32];
        private static sbyte[,] PointKeyArray = new sbyte[9,10];
        private static Vector2Byte[] PointByKeyArray = new Vector2Byte[90];
        /// <summary>
        /// 初始化所有预处理数据
        /// </summary>
        public static void InitData() {
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

        public static ChessType GetChessType(sbyte chessID) {
            return ChessTypeArray[chessID];
        }

        public static sbyte GetPointKey(Vector2Byte point) {
            return PointKeyArray[point.x, point.y];
        }

        public static Vector2Byte GetPointByKey(sbyte pointKey) {
            return PointByKeyArray[pointKey];
        }
    }
}
