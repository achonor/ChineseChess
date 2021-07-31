using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红兵
/// </summary>
public class Chess_0_6 : Chess_x_6 {
    protected override void Awake() {
        base.Awake();
        IsRedChess = true;
    }
}
