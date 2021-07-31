using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红左相
/// </summary>
public class Chess_0_2_0 : Chess_0_2 {

    protected override void Awake() {
        base.Awake();
        SetPosPoint(new Vector2Byte(-2, -4));
    }
}
