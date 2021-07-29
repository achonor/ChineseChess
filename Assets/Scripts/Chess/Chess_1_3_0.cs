using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑左马
/// </summary>
public class Chess_1_3_0 : Chess_1_3 {


    protected override void Awake() {
        base.Awake();
        SetPosPint(new Vector2Int(3, 5));
    }
}
