using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace BeesNuts
{

    public class Hive : MonoBehaviour
    {
        public void GetHit() {
            Swarm.Instance.AddSwarm();
        }


       

        private void OnMouseEnter() {
            Player.CurrentHover = gameObject;
        }

        private void OnMouseExit() {
            if (Player.CurrentHover == gameObject) Player.CurrentHover = null;
        }
    }
}