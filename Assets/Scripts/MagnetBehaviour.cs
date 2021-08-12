using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attraction
{
    public class MagnetBehaviour : MonoBehaviour
    {
        
        [SerializeField]
        private float leftActivePos, rightActivePos;

        [SerializeField]
        private float lerpSpeed = 1;

        [SerializeField]
        private KeyCode activateLeft, activateRight;

        [SerializeField]
        private GameObject ball;


        void Update() {
            Vector3 currentPos = transform.position;
            float xPos = 0;
            if (Input.GetKey(activateLeft) ^ Input.GetKey(activateRight)) {
                xPos = Input.GetKey(activateRight) ? rightActivePos : leftActivePos;
            }
            currentPos = Vector3.MoveTowards(currentPos, new Vector3(xPos, currentPos.y, currentPos.z), lerpSpeed * Time.deltaTime);
            transform.position = new Vector3(currentPos.x, ball.transform.position.y, currentPos.z);
            
            
        }

        public float PullEffect() {
            float effect = 0;

            effect = -transform.position.x*lerpSpeed;
            
            return effect;
        }
    }
}