using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红仕
/// </summary>
public class Chess_0_1 : Chess_x_1 {
    protected override void Awake() {
        base.Awake();
        IsRedChess = true;
    }
}
