using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;

namespace Marginagility {
    [DefaultExecutionOrder(10)]
    public class MarginagilityGameManager : MonoBehaviour {
        private MinigameManager.Ranking _ranking = new();

        void Update() {
            MarginagilityPawn[] pawns = FindObjectsByType<MarginagilityPawn>();
            bool[] hit = new bool[pawns.Length];
            int[] hits = new int[pawns.Length];

            for (int i = 0; i < pawns.Length; i++) {
                for (int j = 0; j < pawns.Length; j++) {
                    if (i == j) continue;
                    if (!pawns[i].GetRiding() || !pawns[j].GetLance()) continue;
                    if (pawns[i].GetDirection() == pawns[j].GetDirection()) continue;

                    if (pawns[i].GetDirection() == 1.0f &&
                        pawns[j].transform.position.x < pawns[i].transform.position.x) continue;
                    if (pawns[i].GetDirection() == -1.0f &&
                        pawns[j].transform.position.x > pawns[i].transform.position.x) continue;

                    if (Mathf.Abs(pawns[i].transform.position.x - pawns[j].transform.position.y) < 3) {
                        hit[i] = true;
                        hits[j]++;
                        Debug.Log("Player " + i + " hit by Player " + j);
                    }
                }
            }

            for (int i = 0; i < pawns.Length; i++) {
                if (hit[i]) {
                    pawns[i].Fall();
                    pawns[i].Hit(hits[i]);
                }
            }
        }

        public void EndMinigame() {
            List<int> scores = new List<int>();
            MarginagilityPawn[] pawns = FindObjectsByType<MarginagilityPawn>();
            foreach (MarginagilityPawn pawn in pawns) {
                scores.Add((int)pawn.GetScore());
            }

            _ranking.DetermineRankingFromScores(scores);
            MinigameManager.instance.EndMinigame(_ranking);
        }
    }
}