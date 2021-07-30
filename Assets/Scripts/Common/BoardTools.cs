using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common {
    public static class BoardTools {

        public static Vector2 PointToWorldPos(Vector2Int point) {
            return point;
        }

        public static bool IsRedChess(byte chessID) {
            return (1 != (chessID & 0x10));
        }

        public static bool IsInBoard(Vector2Int point) {
            if (point.x < -4 || 4 < point.x) {
                return false;
            }
            if (point.y < -4 || 5 < point.y) {
                return false;
            }
            return true;
        }

        public static ChessType GetChessType(byte chessID) {
            return (ChessType)(((chessID & 0xFF) + 1) >> 1);
        }

        public static byte GetPointKey(Vector2Byte point) {
            return (byte)((point.x + 4) * 20 + (point.y + 4));
        }

        public static Vector2Byte GetPointByKey(byte pointKey) {
            return new Vector2Byte((byte)((pointKey / 20) - 4), (byte)((pointKey % 20) - 4));
        }
    }
}
