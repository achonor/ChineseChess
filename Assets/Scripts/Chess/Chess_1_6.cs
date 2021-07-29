using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑卒
/// </summary>
public class Chess_1_6 : Chess_x_6 {
    protected override void Awake() {
        base.Awake();
        IsRedChess = false;
    }

    public override List<Vector2Int> GetMovePoints() {
        List<Vector2Int> result = new List<Vector2Int>();
        for (int i = 0; i < MoveDir.Count; i++) {
            Vector2Int dir = MoveDir[i];
            if (1 == dir.y) {
                //卒不能后退
                continue;
            }
            if (!IsPassRiver() && 0 != dir.x) {
                //没过河的卒不能左右
                continue;
            }
            Vector2Int newPoint = PosPoint + dir;
            result.Add(newPoint);
        }
        return result;
    }
}
