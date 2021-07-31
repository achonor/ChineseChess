using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑左象
/// </summary>
public class Chess_1_2_0 : Chess_1_2 {
    protected override void Awake() {
        base.Awake();
        SetPosPoint(new Vector2Byte(2, 5));
    }
}
