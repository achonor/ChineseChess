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
    public Vector2Int PosPoint {
        get;
        protected set;
    }

    protected override void Start() {
        base.Start();
        SetPosPint(PosPoint);
    }


    /// <summary>
    /// 棋子评分
    /// </summary>
    /// <returns></returns>
    public virtual int GetScore() {
        return 0;
    }

    /// <summary>
    /// 棋子能到达的点
    /// </summary>
    /// <returns></returns>
    public virtual List<Vector2Int> GetMovePoints() {
        return new List<Vector2Int>();
    }

    protected bool IsCanStay(Vector2Int point) {
        if (!BoardTools.IsInBoard(point)) {
            return false;
        }
        ChessBase chess = Board.Instance.GetChessByPoint(point);
        if (null != chess && chess.IsRedChess == IsRedChess) {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 是否过河
    /// </summary>
    /// <returns></returns>
    public bool IsPassRiver() {
        if (IsRedChess) {
            return 0 < PosPoint.y;
        } else {
            return PosPoint.y < 1;
        }
    }


    public void SetDeath(bool isDeath) {
        IsDeath = isDeath;
        gameObject.SetActive(!isDeath);
    }


    public void SetPosPint(Vector2Int point) {
        PosPoint = point;
        transform.position = BoardTools.PointToWorldPos(point);
    }
}
