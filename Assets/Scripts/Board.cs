using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
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

        [SerializeField]
        private Toggle mAIToggleDown;
        [SerializeField]
        private Toggle mAIToggleUp;

        [SerializeField]
        private Text mDownScore;
        [SerializeField]
        private Text mUpScore;

        [SerializeField]
        private Toggle mFlipBoardToggle;

        [SerializeField]
        private Button mClearButton;

        private Chart mChart;

        private bool mIsChessMoving = false;

        private List<GameObject> mAllMovePointEffect = new List<GameObject>();
        private GameObject mNewPointEffect = null;
        private GameObject mLastPointEffect = null;


        public bool IsRedOpenAI {
            get {
                return (mFlipBoardToggle.isOn) ? mAIToggleUp.isOn : mAIToggleDown.isOn;
            }
        }

        public bool IsBlockOpenAI {
            get {
                return (mFlipBoardToggle.isOn) ? mAIToggleDown.isOn : mAIToggleUp.isOn;
            }
        }

        /// <summary>
        /// 当前回合数
        /// </summary>
        private int CurRoundCount = 0;

        private int SelectedChessID = -1;
        private ChessBase SelectedChess {
            get {
                return GetChess(SelectedChessID);
            }
        }

        /// <summary>
        /// 黑方预测分数
        /// </summary>
        private List<int> LastBlockExpectScore = new List<int>();

        protected override void Awake() {
            base.Awake();
            BoardTools.InitData();
            mFlipBoardToggle.onValueChanged.AddListener((param) => {
                Vector3 angles = mMainCamera.transform.localEulerAngles;
                if (!param) {
                    angles.z = 0;
                } else {
                    angles.z = 180;
                }
                mMainCamera.transform.localEulerAngles = angles;
                for (int i = 0; i < AllChess.Count; i++) {
                    AllChess[i].SetFilpChess(param);
                }
                UpdateScoreText();
            });
            mClearButton.onClick.AddListener(() => {
                SetChart(new Chart());
                RemoveEffects();
            });
        }

        protected void Start() {
            SetChart(new Chart());
            //生成Collider
            for (int i = 0; i < 90; i++) {
                int point = BoardTools.GetPointByPosition((i / 10) + 3, (i % 10) + 3);
                GameObject gameObject = new GameObject(point.ToString());
                gameObject.transform.parent = mColliderParent;
                gameObject.layer = LayerMask.NameToLayer("BoardPoint");
                gameObject.transform.localPosition = (Vector2)BoardTools.PointToPosition(point);
                BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
                boxCollider.size = new Vector2(0.9f, 0.9f);
            }
        }

        private int LastPointKey = -1;
        protected void Update() {
            if (Input.GetMouseButtonDown(0)) {
                //射线检测
                Vector3 worldPos = mMainCamera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, mMainCamera.farClipPlane);
                if (null != hit.collider && !mIsChessMoving) {
                    int pointKey = int.Parse(hit.collider.name);
                    ClickPoint(pointKey);
                    if (-1 != mChart.GetChessByPoint(pointKey)) {
                        Debug.Log("棋子评分：" + mChart.GetChessScore(mChart.GetChessByPoint(pointKey)));
                    }
                    if (0 <= LastPointKey) {
                        //Debug.Log("中间棋子数量：" + mChart.GetLineChessCount(pointKey, LastPointKey));
                        LastPointKey = -1;
                    } else {
                        LastPointKey = pointKey;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Backspace)) {
                mChart.BackStep();
                mChart.BackStep();
                RemoveEffects();
                CurRoundCount--;
                if (0 < LastBlockExpectScore.Count) {
                    LastBlockExpectScore.RemoveAt(LastBlockExpectScore.Count - 1);
                }
                SetChart(mChart);
            }
        }


        public void SetChart(Chart chart) {
            mChart = chart;
            //更新棋子位置
            for (int i = 0; i < AllChess.Count; i++) {
                int point = mChart.GetChessPoint(i);
                AllChess[i].SetDeath(-1 == point);
                if (-1 != point) {
                    AllChess[i].SetPosPoint(point);
                }
            }
            UpdateScoreText();
        }

        /// <summary>
        /// 更新分数
        /// </summary>
        public void UpdateScoreText() {
            if (mFlipBoardToggle.isOn) {
                mDownScore.text = mChart.BlockScore.ToString();
                mUpScore.text = mChart.RedScore.ToString();
            } else {
                mDownScore.text = mChart.RedScore.ToString();
                mUpScore.text = mChart.BlockScore.ToString();
            }
        }

        public ChessBase GetChess(int chessID) {
            if (AllChess.Count <= chessID || chessID < 0) {
                return null;
            }
            return AllChess[chessID];
        }

        /// <summary>
        /// 点击位置
        /// </summary>
        /// <param name="pointKey"></param>
        protected void ClickPoint(int pointKey) {
            if (-1 == SelectedChessID) {
                //没有选中的棋
                int chessID = -1;
                if (!mChart.GetChessByPoint(pointKey, out chessID)) {
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
                AudioManager.Instance.PlayNewSound("select");
            } else{
                int chessID = -1;
                if (!mChart.GetChessByPoint(pointKey, out chessID)) {
                    //判断这个点能不能下
                    if (CheckCanMove(SelectedChessID, pointKey)) {
                        //移动
                        MoveChess(SelectedChessID, pointKey);
                    } else {
                        //不能移动
                        PlayCantMove(SelectedChessID, pointKey);
                    }
                } else {
                    if (BoardTools.IsRedChess(SelectedChessID) == BoardTools.IsRedChess(chessID)) {
                        //之前选中的和当前棋是同一阵营的
                        SelectChess(chessID);
                    }else{
                        //判断能不能吃
                        if (CheckCanMove(SelectedChessID, pointKey)) {
                            MoveChess(SelectedChessID, pointKey);
                        } else {
                            //不能移动
                            PlayCantMove(SelectedChessID, pointKey);
                        }
                    }
                }
            }
        }


        protected bool CheckCanMove(int chessID, int point) {
            List<int> points = mChart.GetMovePoints(chessID);
            bool canMove = false;
            for (int i = 0; i < points.Count; i++) {
                if (point == points[i]) {
                    canMove = true;
                    break;
                }
            }
            if (false == canMove) {
                return false;
            }
            bool isRedChess = BoardTools.IsRedChess(chessID);
            mChart.MoveChess(chessID, point);
            //判断是否被将军
            if (mChart.IsJiangJun(!isRedChess)) {
                //对面帅的位置
                int enemyShuaiPoint = mChart.GetShuaiPoint(!isRedChess);
                if ((point >> 4) != (enemyShuaiPoint >> 4) || (point & 0xF) != (enemyShuaiPoint & 0xF)) {
                    mChart.BackStep();
                    return false;
                }
            }
            mChart.BackStep();
            return true;
        }

        protected void SelectChess(int chessID) {
            RemoveMovePointEffects();
            if (-1 != SelectedChessID) {
                SelectedChess.GetComponent<Animator>().Play("UnSelected", 0, 0);
            }
            //选中棋子
            SelectedChessID = chessID;
            SelectedChess.GetComponent<Animator>().Play("Selected", 0, 0);
            //显示可以到达的位置
            List<int> points = mChart.GetMovePoints(SelectedChessID);
            for (int i = 0; i < points.Count; i++) {
                AddMovePointEffect(points[i]);
            }
        }



        protected void AddMovePointEffect(int point) {
            GameObject go = PrefabManager.Instance.LoadPrefab("Prefabs/CanMovedPoint");
            go.SetActive(true);
            go.transform.parent = mEffectParent;
            go.transform.position = (Vector2)BoardTools.PointToPosition(point);
            mAllMovePointEffect.Add(go);
        }

        protected void AddLastPointEffect(int point) {
            GameObject go = PrefabManager.Instance.LoadPrefab("Prefabs/LastPoint");
            go.SetActive(true);
            go.transform.parent = mEffectParent;
            go.transform.position = (Vector2)BoardTools.PointToPosition(point);
            mLastPointEffect = go;
        }

        protected void AddNewPointEffect(int point) {
            GameObject go = PrefabManager.Instance.LoadPrefab("Prefabs/NewPoint");
            go.SetActive(true);
            go.transform.parent = mEffectParent;
            go.transform.position = (Vector2)BoardTools.PointToPosition(point);
            mNewPointEffect = go;
        }

        protected void AddJiangJunEffect() {
            GameObject go = PrefabManager.Instance.LoadPrefab("Prefabs/JiangJun");
            go.SetActive(true);
            go.transform.parent = mEffectParent;
            go.transform.position = Vector3.zero;
            Achonor.Scheduler.AddDelay(0.3f, () => {
                PrefabManager.Instance.RemovePrefab(go);
            });
            AudioManager.Instance.PlayNewSound("Woman_jiangjun");
        }

        protected void AddJueShaEffect() {
            GameObject go = PrefabManager.Instance.LoadPrefab("Prefabs/JueSha");
            go.SetActive(true);
            go.transform.parent = mEffectParent;
            go.transform.position = Vector3.zero;
            Achonor.Scheduler.AddDelay(0.8f, () => {
                PrefabManager.Instance.RemovePrefab(go);
            });
            AudioManager.Instance.PlayNewSound("gamewin");
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

        protected void MoveChess(int chessID, int point) {
            ChessBase chess = GetChess(chessID);
            int lastPoint = mChart.GetChessPoint(chessID);
            //动画
            chess.GetComponent<Animator>().Play("UnSelected");
            //删除特效
            RemoveEffects();
            mIsChessMoving = true;
            chess.transform.DOLocalMove((Vector2)BoardTools.PointToPosition(point), 0.3f).OnComplete(()=> {
                //添加
                AddLastPointEffect(lastPoint);
                AddNewPointEffect(point);
                //音效
                AudioManager.Instance.PlayNewSound("go");

                if ((mChart.IsRedPlayChess && IsRedOpenAI) | ((!mChart.IsRedPlayChess) && IsBlockOpenAI)) {
                    //人机下棋
                    SearchChart.Search(mChart, (step) => {
                        mIsChessMoving = false;
                        LastBlockExpectScore.Add(step.mostScore);
                        MoveChess(step.chessID, step.point);
                    });
                } else {
                    mIsChessMoving = false;
                }
            });

            int oldChessID = -1;
            if (mChart.GetChessByPoint(point, out oldChessID)) {
                //吃子
                GetChess(oldChessID).SetDeath(true);
            }
            //移动棋子
            if (mChart.MoveChess(chessID, point)) {
                AudioManager.Instance.PlayNewSound("eat");
            }
            SelectedChessID = -1;

            if (mChart.IsRedPlayChess) {
                CurRoundCount++;
            }

            //判断是否将军
            bool isRedChess = BoardTools.IsRedChess(chessID);
            if (mChart.IsJiangJun(isRedChess)) {
                //将军, 判断是否绝杀
                if (mChart.IsJueSha(isRedChess)) {
                    AddJueShaEffect();
                } else {
                    AddJiangJunEffect();
                }
            }
            //更新分数
            UpdateScoreText();
        }

        protected void PlayCantMove(int chessID, int point) {
            ChessBase chess = GetChess(chessID);
            chess.transform.DOShakePosition(0.5f, 0.02f);
            AudioManager.Instance.PlayNewSound("goerror");
        }
    }
}



/*
            System.Random random = new System.Random();
            HashSet<ulong> hasValue = new HashSet<ulong>();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            for (int k = 0; k < 7; k++) {
                stringBuilder.Append("{");
                for (int l = 0; l < 90; l++) {
                    ulong number = ((((ulong)random.Next()) << 31) ^ (((ulong)random.Next()) << 21) ^ (((ulong)random.Next()) << 11));
                    if (hasValue.Contains(number)) {
                        Debug.LogError("中彩票了");
                    }
                    hasValue.Add(number);
                    if (89 == l) {
                        stringBuilder.Append(number);
                    } else {
                        stringBuilder.Append(number.ToString() + ",");
                    }
                }
                if (6 == k) {
                    stringBuilder.Append("}");
                } else {
                    stringBuilder.Append("},");
                }
            }
            stringBuilder.Append("}");
            Debug.Log(stringBuilder);

            stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            for (int k = 0; k < 7; k++) {
                stringBuilder.Append("{");
                for (int l = 0; l < 90; l++) {
                    ulong number = ((((ulong)random.Next()) << 31) ^ (((ulong)random.Next()) << 21) ^ (((ulong)random.Next()) << 11));
                    if (hasValue.Contains(number)) {
                        Debug.LogError("中彩票了");
                    }
                    hasValue.Add(number);
                    if (89 == l) {
                        stringBuilder.Append(number);
                    } else {
                        stringBuilder.Append(number.ToString() + ",");
                    }
                }
                if (6 == k) {
                    stringBuilder.Append("}");
                } else {
                    stringBuilder.Append("},");
                }
            }
            stringBuilder.Append("}");
            Debug.Log(stringBuilder);



*/