using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红中路兵
/// </summary>
public class Chess_0_6_2 : Chess_0_6 {
    protected override void Awake() {
        base.Awake();
        SetPosPint(new Vector2Int(0, -1));
    }
}
