using Achonor;
using Assets.Scripts;
using Assets.Scripts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


/// <summary>
/// ���������߷�
/// </summary>
public class SearchChart {
    public const int SByte_0 = 0;
    public const int SByte_1 = 1;
    public const int SByte_16 = 16;

    public const int MAX_VALUE = 100000;
    public const int MIN_VALUE = -100000;

    public class Step {
        public int chessID;
        public int point;

        public int searchDepth;
        /// <summary>
        /// ��������searchDepth�������ŷ���
        /// </summary>
        public int mostScore = MIN_VALUE;

        public void SetValue(int _chessID, int _point, int _searchDepth, int _mostScore) {
            this.chessID = _chessID;
            this.point = _point;
            this.searchDepth = _searchDepth;
            this.mostScore = _mostScore;
        }
    }

    private static Step Result = null;
    /// <summary>
    /// ��������״̬���,byte��ʾ�������
    /// </summary>
    private static Dictionary<ulong, int> mVisited = new Dictionary<ulong, int>();
    private static Dictionary<ulong, int> mVisitedScore = new Dictionary<ulong, int>();

    private static int runCount = 0;
    public static void Search(Chart chart, Action<Step> callback) {
        runCount = 0;
        Step result = null;
        Thread thread = new Thread(()=> {
            try {
                byte curDepth = 5;
                long startTime = Achonor.Function.GetLocaLTime();
                while (curDepth < 64) {
                    curDepth++;
                    mVisited.Clear();
                    result = SearchRoot(chart, curDepth);
                    if (10000 < (Achonor.Function.GetLocaLTime() - startTime)) {
                        break;
                    }
                }
                Debug.LogFormat("������ȣ�{0} ������������{1}", curDepth, runCount);
            } catch (Exception ex) {
                Debug.LogError(ex.ToString());
            }
        });
        thread.Start();
        thread.IsBackground = true;
        // �ȴ��������
        Achonor.Scheduler.CreateScheduler("WaitSearchFinished", 1.0f, 0, 0.5f, () => {
            if (!thread.IsAlive) {
                Debug.LogFormat("���ӣ�{0}�ߵ�{1}", result.chessID, result.point);
                Achonor.Function.CallCallback(callback, result);
                Achonor.Scheduler.Stop("WaitSearchFinished");
            }
        });
    }

    public static Step SearchRoot(Chart chart, int depth) {
        Step result = new Step();
        int bestScore = MIN_VALUE;
        //��ȡ�����ƶ�����
        List<MovePoint> movePoints = chart.GetAllMovePoints(chart.IsRedPlayChess);
        for (int i = 0; i < movePoints.Count; i++) {
            MovePoint move = movePoints[i];
            chart.MoveChess(move.ChessID, move.PointKey);
            int curScore;
            if (MIN_VALUE == bestScore) {
                curScore = -DfsSearch(chart, depth - 1, MIN_VALUE, MAX_VALUE, true);
            } else {
                //���Կղ��ü�
                curScore = -DfsSearch(chart, depth - 1, -bestScore - 1, -bestScore);
                if (bestScore < curScore) {
                    //��֤�ղ��ü�
                    curScore = -DfsSearch(chart, depth - 1, MIN_VALUE, -bestScore, true);
                }
            }
            chart.BackStep();
            if (bestScore < curScore) {
                bestScore = curScore;
                result.SetValue(movePoints[i].ChessID, movePoints[i].PointKey, depth, bestScore);
            }
        }
        return result;
    }


    public static int DfsSearch(Chart chart, int lastDepth, int alpha, int beta, bool NoNULL = false) {
        int result = MIN_VALUE;
        if (-1 == chart.GetChessPoint((chart.IsRedPlayChess ? 0 : 16))) {
            return result;
        }
        if (lastDepth <= 0) {
            runCount++;
            return chart.GetScore(chart.IsRedPlayChess);
        }
        //�ղ��ü�
        if ((!NoNULL) && chart.NullOkay()) {
            chart.NullMove();
            int value = -DfsSearch(chart, lastDepth - 3, -beta, 1 - beta, true);
            chart.BackNullMove();
            if (beta <= value) {
                return value;
            }
        }
        int resultMove;
        List<MovePoint> movePoints = chart.GetAllMovePoints(chart.IsRedPlayChess);
        for (int i = 0; i < movePoints.Count; i++) {
            MovePoint move = movePoints[i];
            chart.MoveChess(move.ChessID, move.PointKey);
            ulong newChartKey = chart.GetChartKey();
            int stepResult;
            if (mVisited.ContainsKey(newChartKey) && lastDepth - 1 <= mVisited[newChartKey]) {
                //�Ѿ�����������
                stepResult = -mVisitedScore[newChartKey];
            } else {
                if (MIN_VALUE == result) {
                    stepResult = -DfsSearch(chart, (lastDepth - 1), -beta, -alpha, true);
                } else {
                    stepResult = -DfsSearch(chart, (lastDepth - 1), -alpha - 1, -alpha);
                    if (alpha < stepResult && stepResult < beta) {
                        stepResult = -DfsSearch(chart, (lastDepth - 1), -beta, -alpha, true);
                    }
                }
            }
            chart.BackStep();
            if (result < stepResult) {
                resultMove = i;
                result = stepResult;
            }
            if (alpha < stepResult) {
                alpha = stepResult;
                if (beta <= alpha) {
                    //��֦
                    return result;
                }
            }
        }
        ulong chartKey = chart.GetChartKey();
        if (!mVisited.ContainsKey(chartKey) || mVisited[chartKey] < lastDepth) {
            mVisited.Update(chartKey, lastDepth);
            mVisitedScore.Update(chartKey, result);
        }
        return result;
    }
}