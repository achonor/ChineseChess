using Assets.Scripts;
using Assets.Scripts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


/// <summary>
/// 搜索最优走法
/// </summary>
public class SearchChart {
    public const sbyte SByte_0 = (sbyte)0;
    public const sbyte SByte_1 = (sbyte)1;
    public const sbyte SByte_16 = (sbyte)16;

    public class Step {
        public sbyte chessID;
        public Vector2Byte point;

        public byte searchDepth;
        /// <summary>
        /// 往后搜索searchDepth步的最优分数
        /// </summary>
        public int mostScore = -100000;

        public void SetValue(sbyte _chessID, Vector2Byte _point, byte _searchDepth, int _mostScore) {
            this.chessID = _chessID;
            this.point = _point;
            this.searchDepth = _searchDepth;
            this.mostScore = _mostScore;
        }
    }
    /// <summary>
    /// 搜索过的状态标记,byte表示搜索深度
    /// </summary>
    private static Dictionary<string, byte> mVisited = new Dictionary<string, byte>();

    private static Dictionary<string, int> mVisitedScore = new Dictionary<string, int>();

    public static bool UpdateVisited(string chartKey, byte depth) {
        lock (mVisited) {
            if (mVisited.ContainsKey(chartKey) && depth <= mVisited[chartKey]) {
                return false;
            }
            Achonor.Function.Update(mVisited, chartKey, depth);
        }
        return true;
    }
    private static int runCount = 0;
    public static void Search(Chart chart, byte lastDepth, Action<Step> callback) {
        runCount = 0;
        mVisited.Clear();
        Step result = null;
        Thread thread = new Thread(()=> {
            try {
                result = DfsSearch(chart, lastDepth, true, int.MinValue, int.MaxValue);
            } catch (Exception ex) {
                Debug.LogError(ex.ToString());
            }
        });
        thread.Start();
        thread.IsBackground = true;
        // 等待搜索完成
        Achonor.Scheduler.CreateScheduler("WaitSearchFinished", 1.0f, 0, 0.1f, () => {
            if (!thread.IsAlive) {
                Debug.Log(runCount);
                Achonor.Function.CallCallback(callback, result);
                Achonor.Scheduler.Stop("WaitSearchFinished");
            }
        });
    }


    public static Step DfsSearch(Chart chart, byte lastDepth, bool isMax, int alpha, int beta) {
        Step result = new Step();
        result.searchDepth = lastDepth;
        if (null == chart.GetChessPoint((sbyte)(chart.IsRedPlayChess ? 0 : 16))) {
            return result;
        }

        List<MovePoint> movePoints = chart.GetAllMovePoints(chart.IsRedPlayChess);
        for (int k = 0; k < movePoints.Count; k++) {
            runCount++;
            MovePoint move = movePoints[k];
            Vector2Byte point = BoardTools.GetPointByKey(move.PointKey);
            chart.MoveChess(move.ChessID, point);
            string newChartKey = chart.GetChartKey();

            Step step;
            if (!UpdateVisited(newChartKey, lastDepth)) {
                //已经计算过更深的
                chart.BackStep();
                continue;
            } else {
                if (lastDepth <= 0) {
                    //直接计算分数，不能往下搜索了
                    step = new Step();
                    step.SetValue(move.ChessID, point, lastDepth, chart.GetScore(!chart.IsRedPlayChess));
                } else {
                    step = DfsSearch(chart, (byte)(lastDepth - 1), !isMax, alpha, beta);
                    step.mostScore *= -1;
                }
            }
            chart.BackStep();

            if (result.mostScore < step.mostScore) {
                result.SetValue(move.ChessID, point, lastDepth, step.mostScore);
            }
            if (isMax) {
                alpha = Math.Max(alpha, step.mostScore);
            } else {
                beta = Math.Min(beta, -step.mostScore);
            }
            if (beta <= alpha) {
                //剪枝
                return result;
            }
        }
        return result;
    }
}