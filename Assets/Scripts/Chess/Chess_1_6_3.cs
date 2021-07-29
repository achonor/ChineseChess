using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑3路兵
/// </summary>
public class Chess_1_6_3 : Chess_1_6 {
    protected override void Awake() {
        base.Awake();
        SetPosPint(new Vector2Int(-2, 2));
    }
}
