using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 马
/// </summary>
public class Chess_x_3 : ChessBase {

    protected override void Awake() {
        base.Awake();
        ChType = ChessType.Ma;
    }
}
