using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红右仕
/// </summary>
public class Chess_0_1_1 : Chess_0_1 {
    protected override void Awake() {
        base.Awake();
        SetPosPint(new Vector2Int(1, -4));
    }
}
