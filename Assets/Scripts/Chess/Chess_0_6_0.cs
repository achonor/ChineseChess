using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红9路兵
/// </summary>
public class Chess_0_6_0 : Chess_0_6 {
    protected override void Awake() {
        base.Awake();
        SetPosPoint(new Vector2Byte(-4, -1));
    }
}
