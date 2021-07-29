using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑1路兵
/// </summary>
public class Chess_1_6_4 : Chess_1_6 {
    protected override void Awake() {
        base.Awake();
        SetPosPint(new Vector2Int(-4, 2));
    }
}
