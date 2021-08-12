using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attraction
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallMove : MonoBehaviour
    {
        [SerializeField]
        private float top, bottom,fifty,negTens,fives;
        
        private Rigidbody rb;
        private MagnetBehaviour magnets;

        // Start is called before the first frame update
        void Start() {
            rb = GetComponent<Rigidbody>();
            magnets = FindObjectOfType<MagnetBehaviour>();
        }

        // Update is called once per frame
        void Update() {
            transform.position +=  new Vector3(magnets.PullEffect(), 0, 0) * Time.deltaTime;

            if (transform.position.y <= bottom) {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity=Vector3.zero;
                    transform.position = new Vector3(0, top, -1);

                    float xPos = transform.position.x;
                    if (xPos > -fifty && xPos < fifty) {
                        SceneFlowManager.Instance.points += 50;
                    }else if (xPos > -negTens && xPos < negTens) {
                        SceneFlowManager.Instance.points -= 10;
                    }else if (xPos > -fives && xPos < fives) {
                        SceneFlowManager.Instance.points += 5;
                    }
                    else {
                        SceneFlowManager.Instance.points -= 50;
                    }
                    
                
                    SceneFlowManager.Instance.CheckEnd();
                
            }
        }
    }
}