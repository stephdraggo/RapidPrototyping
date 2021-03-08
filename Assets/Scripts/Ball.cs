using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GyroGame
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Rigidbody ballBody;

        private ParticleSystem particles;
        
        void Start()
        {
            particles=GetComponent<ParticleSystem>();
        }

        private void OnTriggerEnter(Collider other)
        {
            ResetScene();
        }

        private void OnCollisionEnter(Collision collision)
        {
            particles.Play();
        }

        void Update()
        {
            ballBody.WakeUp();
        }

        public void ResetScene()
        {
            SceneManager.LoadScene(0);
        }
    }
}