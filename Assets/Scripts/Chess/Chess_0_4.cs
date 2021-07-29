using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红车
/// </summary>
public class Chess_0_4 : Chess_x_4 {
    protected override void Awake() {
        base.Awake();
        IsRedChess = true;
    }
}
