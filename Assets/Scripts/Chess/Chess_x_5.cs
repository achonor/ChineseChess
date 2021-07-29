using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 炮
/// </summary>
public class Chess_x_5 : ChessBase {
    protected override void Awake() {
        base.Awake();
        ChType = ChessType.Pao;
    }

    public override int GetScore() {
        return 500;
    }

    public override List<Vector2Int> GetMovePoints() {
        List<Vector2Int> result = new List<Vector2Int>();
        bool hasHill = false;
        //x方向移动
        for (int i = 1; i <= 8; i++) {
            Vector2Int newPoint = PosPoint + new Vector2Int(i, 0);
            if (!CheckPoint(result, newPoint, hasHill)) {
                if (hasHill) {
                    break;
                }
                hasHill = true;
            }
        }
        hasHill = false;
        for (int i = 1; i <= 8; i++) {
            Vector2Int newPoint = PosPoint + new Vector2Int(-i, 0);
            if (!CheckPoint(result, newPoint, hasHill)) {
                if (hasHill) {
                    break;
                }
                hasHill = true;
            }
        }

        //y方向移动
        hasHill = false;
        for (int i = 1; i <= 9; i++) {
            Vector2Int newPoint = PosPoint + new Vector2Int(0, i);
            if (!CheckPoint(result, newPoint, hasHill)) {
                if (hasHill) {
                    break;
                }
                hasHill = true;
            }
        }
        hasHill = false;
        for (int i = 1; i <= 9; i++) {
            Vector2Int newPoint = PosPoint + new Vector2Int(0, -i);
            if (!CheckPoint(result, newPoint, hasHill)) {
                if (hasHill) {
                    break;
                }
                hasHill = true;
            }
        }
        return result;
    }

    private bool CheckPoint(List<Vector2Int> resultList, Vector2Int point, bool hasHill) {
        ChessBase chess = Board.Instance.GetChessByPoint(point);
        if (hasHill) {
            //翻过山了
            if (null != chess && chess.IsRedChess != chess) {
                //可以打
                resultList.Add(point);
            }
        }
        if (null != chess) {
            return false;
        }
        resultList.Add(point);
        return true;
    }

}
