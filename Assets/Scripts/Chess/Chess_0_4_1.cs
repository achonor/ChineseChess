using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红右车
/// </summary>
public class Chess_0_4_1 : Chess_0_4 {
    protected override void Awake() {
        base.Awake();
        SetPosPoint(new Vector2Byte(4, -4));
    }
}
