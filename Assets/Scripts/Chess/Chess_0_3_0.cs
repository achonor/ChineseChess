using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红左马
/// </summary>
public class Chess_0_3_0 : Chess_0_3 {

    protected override void Awake() {
        base.Awake();
        SetPosPint(new Vector2Int(-3, -4));
    }
}
