using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红炮
/// </summary>
public class Chess_0_5 : Chess_x_5 {
    protected override void Awake() {
        base.Awake();
        IsRedChess = true;
    }
}
