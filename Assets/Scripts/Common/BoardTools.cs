using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common {
    public static class BoardTools {

        public static Vector2 PointToPosition(Vector2Byte point) {
            return new Vector2(point.x, point.y);
        }

        public static bool IsRedChess(sbyte chessID) {
            return (0 == (chessID & 0x10));
        }

        public static bool IsInBoard(Vector2Byte point) {
            if (point.x < -4 || 4 < point.x) {
                return false;
            }
            if (point.y < -4 || 5 < point.y) {
                return false;
            }
            return true;
        }

        public static ChessType GetChessType(sbyte chessID) {
            if (0x0B <= (chessID & 0x0F)) {
                return ChessType.Bing;
            }
            return (ChessType)(((chessID & 0x0F) + 1) >> 1);
        }

        public static sbyte GetPointKey(Vector2Byte point) {
            return (sbyte)((point.x + 4) * 10 + (point.y + 4));
        }

        public static Vector2Byte GetPointByKey(sbyte pointKey) {
            return new Vector2Byte((sbyte)((pointKey / 10) - 4), (sbyte)((pointKey % 10) - 4));
        }
    }
}
