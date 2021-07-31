using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 棋子的基类
/// </summary>
public class ChessBase : BaseBehaviour {
    /// <summary>
    /// 棋子类型
    /// </summary>
    public ChessType ChType {
        get;
        protected set;
    }

    /// <summary>
    /// 是否阵亡
    /// </summary>
    public bool IsDeath {
        get;
        protected set;
    }

    /// <summary>
    /// 棋子阵营
    /// </summary>
    public bool IsRedChess {
        get;
        protected set;
    }

    /// <summary>
    /// 当前位置
    /// </summary>
    public Vector2Byte PosPoint {
        get;
        protected set;
    }

    protected override void Start() {
        base.Start();
        SetPosPoint(PosPoint);
    }


    public void SetDeath(bool isDeath) {
        IsDeath = isDeath;
        gameObject.SetActive(!isDeath);
    }


    public void SetPosPoint(Vector2Byte point) {
        PosPoint = point;
        transform.localPosition = BoardTools.PointToPosition(point);
    }
}
