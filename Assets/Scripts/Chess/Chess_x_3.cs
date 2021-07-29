using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 马
/// </summary>
public class Chess_x_3 : ChessBase {
    public List<Vector2Int> MoveDir = new List<Vector2Int>() {
        new Vector2Int(2, 1),
        new Vector2Int(1, 2),
        new Vector2Int(-1, 2),
        new Vector2Int(-2, 1),
        new Vector2Int(-2, -1),
        new Vector2Int(-1, -2),
        new Vector2Int(1, -2),
        new Vector2Int(2, -1)
    };

    public List<Vector2Int> MaFootDir = new List<Vector2Int>() {
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0)
    };


    protected override void Awake() {
        base.Awake();
        ChType = ChessType.Ma;
    }

    public override int GetScore() {
        return 500;
    }

    public override List<Vector2Int> GetMovePoints(){
        List<Vector2Int> result = new List<Vector2Int>();
        for (int i = 0; i < MoveDir.Count; i++) {
            Vector2Int newPoint = PosPoint + MoveDir[i];
            Vector2Int maFoot = PosPoint + MaFootDir[i];
            if (null != Board.Instance.GetChessByPoint(maFoot) || IsCanStay(newPoint)) {
                continue;
            }
            result.Add(newPoint);
        }
        return result;
    }
}
