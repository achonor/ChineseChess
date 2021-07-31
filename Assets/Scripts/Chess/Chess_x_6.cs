using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 兵卒
/// </summary>
public class Chess_x_6 : ChessBase {
    protected override void Awake() {
        base.Awake();
        ChType = ChessType.Bing;
    }
}
