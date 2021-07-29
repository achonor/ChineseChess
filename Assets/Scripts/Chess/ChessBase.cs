using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ӵĻ���
/// </summary>
public class ChessBase : BaseBehaviour {
    /// <summary>
    /// ��������
    /// </summary>
    public ChessType ChType {
        get;
        protected set;
    }

    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool IsDeath {
        get;
        protected set;
    }

    /// <summary>
    /// ������Ӫ
    /// </summary>
    public bool IsRedChess {
        get;
        protected set;
    }

    /// <summary>
    /// ��ǰλ��
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
    /// ��������
    /// </summary>
    /// <returns></returns>
    public virtual int GetScore() {
        return 0;
    }

    /// <summary>
    /// �����ܵ���ĵ�
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
    /// �Ƿ����
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
