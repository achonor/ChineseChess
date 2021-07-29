using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红右炮
/// </summary>
public class Chess_0_5_1 : Chess_0_5 {
    protected override void Awake() {
        base.Awake();
        SetPosPint(new Vector2Int(3, -2));
    }
}
