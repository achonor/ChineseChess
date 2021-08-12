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

    public class Step {
        public int chessID;
        public int point;

        public byte searchDepth;
        /// <summary>
        /// ��������searchDepth�������ŷ���
        /// </summary>
        public int mostScore = -100000;

        public void SetValue(int _chessID, int _point, byte _searchDepth, int _mostScore) {
            this.chessID = _chessID;
            this.point = _point;
            this.searchDepth = _searchDepth;
            this.mostScore = _mostScore;
        }
    }
    /// <summary>
    /// ��������״̬���,byte��ʾ�������
    /// </summary>
    private static Dictionary<ulong, byte> mVisited = new Dictionary<ulong, byte>();

    private static Dictionary<string, int> mVisitedScore = new Dictionary<string, int>();

    public static bool UpdateVisited(ulong chartKey, byte depth) {
        if (mVisited.ContainsKey(chartKey) && depth <= mVisited[chartKey]) {
            return false;
        }
        return true;
    }
    private static int runCount = 0;
    public static void Search(Chart chart, Action<Step> callback) {
        runCount = 0;
        mVisited.Clear();
        Step result = null;
        Thread thread = new Thread(()=> {
            try {
                byte curDepth = 3;
                long startTime = Achonor.Function.GetLocaLTime();
                while (curDepth <= 64) {
                    result = DfsSearch(chart, curDepth, true, int.MinValue, int.MaxValue);
                    long newTime = Achonor.Function.GetLocaLTime();
                    if (15000 < (newTime - startTime)) {
                        break;
                    }
                    curDepth++;
                }
                Debug.LogFormat("������ȣ�{0} ������������{1}", (curDepth + 1), runCount);
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


    public static Step DfsSearch(Chart chart, byte lastDepth, bool isMax, int alpha, int beta) {
        Step result = new Step();
        result.searchDepth = lastDepth;
        if (-1 == chart.GetChessPoint((chart.IsRedPlayChess ? 0 : 16))) {
            return result;
        }
        //�ղ��ü�



        List<MovePoint> movePoints = chart.GetAllMovePoints(chart.IsRedPlayChess);
        for (int k = 0; k < movePoints.Count; k++) {
            MovePoint move = movePoints[k];
            chart.MoveChess(move.ChessID, move.PointKey);
            ulong newChartKey = chart.GetChartKey();
            Step step;
            if (!UpdateVisited(newChartKey, lastDepth)) {
                //�Ѿ�����������
                chart.BackStep();
                continue;
            } else {
                if (lastDepth <= 0) {
                    runCount++;
                    //ֱ�Ӽ����������������������
                    step = new Step();
                    step.SetValue(move.ChessID, move.PointKey, lastDepth, chart.GetScore(!chart.IsRedPlayChess));
                } else {
                    step = DfsSearch(chart, (byte)(lastDepth - 1), !isMax, alpha, beta);
                    step.mostScore *= -1;
                }
            }
            chart.BackStep();

            if (result.mostScore < step.mostScore) {
                result.SetValue(move.ChessID, move.PointKey, lastDepth, step.mostScore);
            }
            if (isMax) {
                alpha = Math.Max(alpha, step.mostScore);
            } else {
                beta = Math.Min(beta, -step.mostScore);
            }
            if (beta <= alpha) {
                //��֦
                return result;
            }
        }
        return result;
    }
}