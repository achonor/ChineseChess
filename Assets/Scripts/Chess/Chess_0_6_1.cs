using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红7路兵
/// </summary>
public class Chess_0_6_1 : Chess_0_6 {
    protected override void Awake() {
        base.Awake();
        SetPosPint(new Vector2Int(-2, -1));
    }
}
