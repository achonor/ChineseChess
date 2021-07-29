using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 车
/// </summary>
public class Chess_x_4 : ChessBase {
    protected override void Awake() {
        base.Awake();
        ChType = ChessType.Che;
    }

    public override int GetScore() {
        return 1000;
    }

    public override List<Vector2Int> GetMovePoints(){
        List<Vector2Int> result = new List<Vector2Int>();
        //x方向移动
        for (int i = 1; i <= 8; i++) {
            Vector2Int newPoint = PosPoint + new Vector2Int(i, 0);
            if (!CheckPoint(result, newPoint)) {
                break;
            }
        }
        for (int i = 1; i <= 8; i++) {
            Vector2Int newPoint = PosPoint + new Vector2Int(-i, 0);
            if (!CheckPoint(result, newPoint)) {
                break;
            }
        }

        //y方向移动
        for (int i = 1; i <= 9; i++) {
            Vector2Int newPoint = PosPoint + new Vector2Int(0, i);
            if (!CheckPoint(result, newPoint)) {
                break;
            }
        }
        for (int i = 1; i <= 9; i++) {
            Vector2Int newPoint = PosPoint + new Vector2Int(0, -i);
            if (!CheckPoint(result, newPoint)) {
                break;
            }
        }
        return result;
    }

    private bool CheckPoint(List<Vector2Int> resultList, Vector2Int point) {
        if (IsCanStay(point)) {
            return false;
        }
        resultList.Add(point);
        ChessBase chess = Board.Instance.GetChessByPoint(point);
        if (null != chess) {
            return false;
        }
        return true;
    }

}
