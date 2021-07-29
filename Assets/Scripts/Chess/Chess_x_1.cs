using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 仕
/// </summary>
public class Chess_x_1 : ChessBase {
    public List<Vector2Int> MoveDir = new List<Vector2Int>() {
        new Vector2Int(1, 1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1),
        new Vector2Int(1, -1)
    };

    protected override void Awake() {
        base.Awake();
        ChType = ChessType.Shi;
    }

    public override int GetScore() {
        return 150;
    }
}
