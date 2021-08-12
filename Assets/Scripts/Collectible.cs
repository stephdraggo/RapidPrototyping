using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attraction
{
    public class Collectible : MonoBehaviour
    {
        [SerializeField]
        private bool good;

        private void OnTriggerEnter(Collider other) {
            if (good) {
                SceneFlowManager.Instance.points+=5;
            }
            else {
                SceneFlowManager.Instance.points-=5;
            }
            
            SceneFlowManager.Instance.UpdateFill();
        }
    }
}