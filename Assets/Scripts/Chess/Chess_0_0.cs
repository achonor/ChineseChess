using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 帅
/// </summary>
public class Chess_0_0 : Chess_x_0 {
    protected override void Awake() {
        base.Awake();
        IsRedChess = true;
        SetPosPint(new Vector2Int(0, -4));
    }
    public override List<Vector2Int> GetMovePoints(){
        List<Vector2Int> result = new List<Vector2Int>();
        for (int i = 0; i < MoveDir.Count; i++) {
            Vector2Int newPoint = PosPoint + MoveDir[i];
            if (newPoint.x < -1 || 1 < newPoint.x || -2 < newPoint.y || (!IsCanStay(newPoint))) {
                continue;
            }
            result.Add(newPoint);
        }
        return result;
    }
}
