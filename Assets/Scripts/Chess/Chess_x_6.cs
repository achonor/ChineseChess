using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 兵卒
/// </summary>
public class Chess_x_6 : ChessBase {
    public List<Vector2Int> MoveDir = new List<Vector2Int>() {
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1)
    };

    protected override void Awake() {
        base.Awake();
        ChType = ChessType.Bing;
    }

    public override int GetScore() {
        return IsPassRiver() ? 200 : 100;
    }
}
