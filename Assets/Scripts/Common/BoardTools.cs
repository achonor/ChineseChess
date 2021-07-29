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

        public static bool IsInBoard(Vector2Int point) {
            if (point.x < -4 || 4 < point.x) {
                return false;
            }
            if (point.y < -4 || 5 < point.y) {
                return false;
            }
            return true;
        }

        public static int GetPointKey(Vector2Int point) {
            return (point.x + 4) * 100 + (point.y + 4);
        }

        public static Vector2Int GetPointByKey(int pointKey) {
            return new Vector2Int((pointKey / 100) - 4, (pointKey % 100) - 4);
        }
    }
}
