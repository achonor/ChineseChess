using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common {
    public static class BoardTools {

        public static Dictionary<ChessType, string[]> ChessName = new Dictionary<ChessType, string[]>() {
            {ChessType.Shuai,   new string[]{ "帅", "将" }},
            {ChessType.Shi,     new string[]{ "仕", "士" }},
            {ChessType.Xiang,   new string[]{ "相", "象" }},
            {ChessType.Ma,      new string[]{ "馬", "马" }},
            {ChessType.Che,     new string[]{ "車", "车" }},
            {ChessType.Pao,     new string[]{ "軳", "炮" }},
            {ChessType.Bing,    new string[]{ "兵", "卒" }}
        };
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
            }else{
                result.Append(GetXIndexName(isRedChess, point2.x).ToString());
            }
            return result.ToString();
        }

        public static string GetXIndexName( bool isRedChess, int index) {
            index += 5;
            if (!isRedChess) {
                index = 10 - index;
            }
            return index.ToString();
        }

        public static sbyte GetPointKey(Vector2Byte point) {
            return (sbyte)((point.x + 4) * 10 + (point.y + 4));
        }

        public static Vector2Byte GetPointByKey(sbyte pointKey) {
            return new Vector2Byte((sbyte)((pointKey / 10) - 4), (sbyte)((pointKey % 10) - 4));
        }
    }
}
