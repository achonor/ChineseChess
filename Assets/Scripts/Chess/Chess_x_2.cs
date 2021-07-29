using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 相
/// </summary>
public class Chess_x_2 : ChessBase {
    public List<Vector2Int> MoveDir = new List<Vector2Int>() {
        new Vector2Int(2, 2),
        new Vector2Int(-2, 2),
        new Vector2Int(-2, -2),
        new Vector2Int(2, -2),
    };

    protected override void Awake() {
        base.Awake();
        ChType = ChessType.Xiang;
    }

    public override int GetScore() {
        return 150;
    }
}
