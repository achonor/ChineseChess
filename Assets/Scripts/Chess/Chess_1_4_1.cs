using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑右车
/// </summary>
public class Chess_1_4_1 : Chess_1_4 {
    protected override void Awake() {
        base.Awake();
        SetPosPint(new Vector2Int(-4, 5));
    }
}
