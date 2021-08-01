using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BigBoi.PlayerController
{
    public class GroundCheck : MonoBehaviour
    {
        //jump contains:
        //isGrounded
        //layer check for static walkable
        //reference to controller
        
        public bool IsGrounded { get; protected set; }
        protected Collider boxCollider;

        private void OnValidate() {
            boxCollider ??= GetComponent<Collider>();
            if (boxCollider && !boxCollider.isTrigger) {
                boxCollider.isTrigger = true;
            }
        }

       

        private void Start() {
            OnValidate();
        }

        // private void OnTriggerEnter(Collider other) {
        //     Debug.Log(other.gameObject.name);
        // }

        private void OnTriggerStay(Collider other) {
            if (IsInLayerMask(other.gameObject,StaticControllerInfo.Instance.walkable)) {
                IsGrounded = true;
            }
        }

        protected void OnTriggerExit(Collider other) {
            if (IsInLayerMask(other.gameObject,StaticControllerInfo.Instance.walkable)) {
                IsGrounded = false;
            }
        }
        
        /// <summary>
        /// Check if this object's layer is in the passed layermask
        /// </summary>
        public static bool IsInLayerMask(GameObject obj, LayerMask layerMask)
        {
            return ((layerMask.value & (1 << obj.layer)) > 0);
        }
    }
}