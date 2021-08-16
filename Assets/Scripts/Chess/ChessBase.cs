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
    public int PosPoint {
        get;
        protected set;
    }

    private Transform mObject;

    protected override void Start() {
        base.Start();
        if (null == mObject) {
            mObject = transform.Find("Object");
        }
    }


    public void SetDeath(bool isDeath) {
        IsDeath = isDeath;
        gameObject.SetActive(!isDeath);
    }


    public void SetPosPoint(int point) {
        PosPoint = point;
        transform.localPosition = (Vector2)BoardTools.PointToPosition(point);
    }

    public void SetFilpChess(bool isFilp) {
        mObject.localEulerAngles = new Vector3(0, 0, isFilp ? 180 : 0);
    }
}
