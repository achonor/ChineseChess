using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红相
/// </summary>
public class Chess_0_2 : Chess_x_2 {

    protected override void Awake() {
        base.Awake();
        IsRedChess = true;
    }
}
