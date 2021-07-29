using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑右象
/// </summary>
public class Chess_1_2_1 : Chess_1_2 {
    protected override void Awake() {
        base.Awake();
        SetPosPint(new Vector2Int(-2, 5));
    }
}
