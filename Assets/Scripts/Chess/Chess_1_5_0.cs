using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑左炮
/// </summary>
public class Chess_1_5_0 : Chess_0_5 {
    protected override void Awake() {
        base.Awake();
        SetPosPoint(new Vector2Byte(3, 3));
    }
}
