using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GyroGame
{

    public class GyroMovement : MonoBehaviour
    {
        [SerializeField]
        private float maxTurn = 45f, minTurn = -45f;

        void Start()
        {
            Input.gyro.enabled = true;
        }

        void Update()
        {

        }

        private void FixedUpdate()
        {
            
            gameObject.transform.rotation = new Quaternion(Mathf.Clamp(Input.gyro.attitude.x, minTurn, maxTurn), 0, Mathf.Clamp(Input.gyro.attitude.y, minTurn, maxTurn), gameObject.transform.rotation.w);
        }
    }
}