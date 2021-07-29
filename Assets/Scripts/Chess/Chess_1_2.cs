using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑象
/// </summary>
public class Chess_1_2 : Chess_x_2 {

    protected override void Awake() {
        base.Awake();
        IsRedChess = false;
    }

    public override List<Vector2Int> GetMovePoints(){
        List<Vector2Int> result = new List<Vector2Int>();
        for (int i = 0; i < MoveDir.Count; i++) {
            Vector2Int newPoint = PosPoint + MoveDir[i];
            if (newPoint.y < 1 || (!IsCanStay(newPoint))) {
                continue;
            }
            result.Add(newPoint);
        }
        return result;
    }
}
