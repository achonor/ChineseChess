using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 帅
/// </summary>
public class Chess_0_0 : Chess_x_0 {
    protected override void Awake() {
        base.Awake();
        IsRedChess = true;
        SetPosPoint(new Vector2Byte(0, -4));
    }
}
