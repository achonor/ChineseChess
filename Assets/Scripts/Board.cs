using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace Assets.Scripts {
    public class Board  : SingleInstance<Board>{
        [SerializeField]
        private Camera mMainCamera;
        [SerializeField]
        private List<ChessBase> AllChess = new List<ChessBase>();

        [SerializeField]
        private Transform mColliderParent;

        [SerializeField]
        private Transform mEffectParent;

        private Chart mChart;

        private List<GameObject> mAllMovePointEffect = new List<GameObject>();
        private GameObject mNewPointEffect = null;
        private GameObject mLastPointEffect = null;

        private sbyte SelectedChessID = -1;
        private ChessBase SelectedChess {
            get {
                return GetChess(SelectedChessID);
            }
        }

        private bool mIsChessMoving = false;


        protected void Start() {
            SetChart(new Chart());
            //生成Collider
            for (sbyte i = 0; i < 90; i++) {
                GameObject gameObject = new GameObject(i.ToString());
                gameObject.transform.parent = mColliderParent;
                gameObject.layer = LayerMask.NameToLayer("BoardPoint");
                gameObject.transform.localPosition = BoardTools.PointToPosition(BoardTools.GetPointByKey(i));
                BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
                boxCollider.size = new Vector2(0.9f, 0.9f);
            }
        }

        protected void Update() {
            if (Input.GetMouseButtonDown(0)) {
                //射线检测
                Vector3 worldPos = mMainCamera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, mMainCamera.farClipPlane);
                if (null != hit.collider && !mIsChessMoving) {
                    sbyte pointKey = sbyte.Parse(hit.collider.name);
                    ClickPoint(pointKey);
                }
            }
        }


        public void SetChart(Chart chart) {
            mChart = chart;
            //更新棋子位置
            for (sbyte i = 0; i < AllChess.Count; i++) {
                Vector2Byte point = mChart.GetChessPoint(i);
                AllChess[i].SetDeath(null == point);
                if (null != point) {
                    AllChess[i].SetPosPoint(point);
                }
            }
        }

        public ChessBase GetChess(sbyte chessID) {
            if (AllChess.Count <= chessID || chessID < 0) {
                return null;
            }
            return AllChess[chessID];
        }

        /// <summary>
        /// 点击位置
        /// </summary>
        /// <param name="pointKey"></param>
        protected void ClickPoint(sbyte pointKey) {
            Vector2Byte point = BoardTools.GetPointByKey(pointKey);
            if (-1 == SelectedChessID) {
                //没有选中的棋
                sbyte chessID = -1;
                if (!mChart.GetChessByPoint(point, out chessID)) {
                    //没有选中棋，点的还是空白的地方
                    return;
                }
                bool isRedChess = BoardTools.IsRedChess(chessID);
                if (mChart.IsRedPlayChess != isRedChess) {
                    //不是这边下的
                    return;
                }
                //选中棋子
                SelectChess(chessID);
            } else{
                sbyte chessID = -1;
                if (!mChart.GetChessByPoint(point, out chessID)) {
                    //判断这个点能不能下
                    if (CheckCanMove(SelectedChessID, point)) {
                        //移动
                        MoveChess(SelectedChessID, point);
                    } else {
                        //不能移动
                        PlayCantMove(SelectedChessID, point);
                    }
                } else {
                    if (BoardTools.IsRedChess(SelectedChessID) == BoardTools.IsRedChess(chessID)) {
                        //之前选中的和当前棋是同一阵营的
                        SelectChess(chessID);
                    }else{
                        //判断能不能吃
                        if (CheckCanMove(SelectedChessID, point)) {
                            MoveChess(SelectedChessID, point);
                        } else {
                            //不能移动
                            PlayCantMove(SelectedChessID, point);
                        }
                    }
                }
            }
        }


        protected bool CheckCanMove(sbyte chessID, Vector2Byte point) {
            List<Vector2Byte> points = mChart.GetMovePoints(chessID);
            bool canMove = false;
            for (int i = 0; i < points.Count; i++) {
                if (point.IsEqules(points[i])) {
                    canMove = true;
                    break;
                }
            }
            if (false == canMove) {
                return false;
            }
            bool isRedChess = BoardTools.IsRedChess(chessID);
            Chart chart = Chart.Clone(mChart);
            chart.MoveChess(chessID, point);
            //判断是否被将军
            Vector2Byte shuaiPoint = chart.GetChessPoint((sbyte)(isRedChess ? 0 : 16));
            for (int i = 0; i < 16; i++) {
                sbyte tempID = (sbyte)(i + (isRedChess ? 16 : 0));
                List<Vector2Byte> tempPoints = chart.GetMovePoints(tempID);
                for (int k = 0; k < tempPoints.Count; k++) {
                    if (shuaiPoint.IsEqules(tempPoints[k])){
                        return false;
                    }
                }
            }
            return true;
        }

        protected void SelectChess(sbyte chessID) {
            RemoveMovePointEffects();
            if (-1 != SelectedChessID) {
                SelectedChess.GetComponent<Animator>().Play("UnSelected", 0, 0);
            }
            //选中棋子
            SelectedChessID = chessID;
            SelectedChess.GetComponent<Animator>().Play("Selected", 0, 0);
            //显示可以到达的位置
            List<Vector2Byte> points = mChart.GetMovePoints(SelectedChessID);
            for (int i = 0; i < points.Count; i++) {
                AddMovePointEffect(points[i]);
            }
        }

        protected void AddMovePointEffect(Vector2Byte point) {
            GameObject go = PrefabManager.Instance.LoadPrefab("Prefabs/CanMovedPoint");
            go.SetActive(true);
            go.transform.parent = mEffectParent;
            go.transform.position = BoardTools.PointToPosition(point);
            mAllMovePointEffect.Add(go);
        }

        protected void AddLastPointEffect(Vector2Byte point) {
            GameObject go = PrefabManager.Instance.LoadPrefab("Prefabs/LastPoint");
            go.SetActive(true);
            go.transform.parent = mEffectParent;
            go.transform.position = BoardTools.PointToPosition(point);
            mLastPointEffect = go;
        }

        protected void AddNewPointEffect(Vector2Byte point) {
            GameObject go = PrefabManager.Instance.LoadPrefab("Prefabs/NewPoint");
            go.SetActive(true);
            go.transform.parent = mEffectParent;
            go.transform.position = BoardTools.PointToPosition(point);
            mNewPointEffect = go;
        }

        protected void RemoveMovePointEffects() {
            while (0 < mAllMovePointEffect.Count) {
                PrefabManager.Instance.RemovePrefab(mAllMovePointEffect[0]);
                mAllMovePointEffect.RemoveAt(0);
            }
        }

        protected void RemoveEffects() {
            RemoveMovePointEffects();
            if (null != mNewPointEffect) {
                PrefabManager.Instance.RemovePrefab(mNewPointEffect);
                mNewPointEffect = null;
            }
            if (null != mLastPointEffect) {
                PrefabManager.Instance.RemovePrefab(mLastPointEffect);
                mLastPointEffect = null;
            }
        }

        protected void MoveChess(sbyte chessID, Vector2Byte point) {
            ChessBase chess = GetChess(chessID);
            Vector2Byte lastPoint = mChart.GetChessPoint(chessID);
            //动画
            chess.GetComponent<Animator>().Play("UnSelected");
            //删除特效
            RemoveEffects();
            mIsChessMoving = true;
            chess.transform.DOLocalMove(BoardTools.PointToPosition(point), 0.3f).OnComplete(()=> {
                mIsChessMoving = false;
                //添加
                AddLastPointEffect(lastPoint);
                AddNewPointEffect(point);
            });

            sbyte oldChessID = -1;
            if (mChart.GetChessByPoint(point, out oldChessID)) {
                //吃子
                GetChess(oldChessID).SetDeath(true);
            }
            //移动棋子
            mChart.MoveChess(chessID, point);
            SelectedChessID = -1;
        }

        protected void PlayCantMove(sbyte chessID, Vector2Byte point) {
            ChessBase chess = GetChess(chessID);
            chess.transform.DOShakePosition(0.5f, 0.01f);
        }
    }
}
