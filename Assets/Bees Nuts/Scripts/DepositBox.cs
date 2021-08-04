using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeesNuts
{
    public class DepositBox : MonoBehaviour
    {
        public static DepositBox Instance;
        public int score;
        
        

        public void ScoreChange(int addScore) {
            score += addScore;
        }

        private void Awake() {
            Instance = this;
        }

        private void OnMouseEnter() {
            Player.CurrentHover = gameObject;
        }

        private void OnMouseExit() {
            if (Player.CurrentHover == gameObject) Player.CurrentHover = null;
        }
    }
}