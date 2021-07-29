using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑士
/// </summary>
public class Chess_1_1 : Chess_x_1 {

    protected override void Awake() {
        base.Awake();
        IsRedChess = false;
    }

    public override List<Vector2Int> GetMovePoints(){
        List<Vector2Int> result = new List<Vector2Int>();
        for (int i = 0; i < MoveDir.Count; i++) {
            Vector2Int newPoint = PosPoint + MoveDir[i];
            if (newPoint.x < -1 || 1 < newPoint.x || newPoint.y < 3 || (!IsCanStay(newPoint))) {
                continue;
            }
            result.Add(newPoint);
        }
        return result;
    }
}
