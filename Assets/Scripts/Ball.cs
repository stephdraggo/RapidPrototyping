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
        private ParticleSystem.MainModule module;


        void Start()
        {
            particles=GetComponent<ParticleSystem>();
            module = particles.main;
        }

        private void OnTriggerEnter(Collider other)
        {
            ResetScene();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("wall"))
            {
                module.loop = true;
                particles.Play();
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("wall"))
            {
                module.loop = false;
            }
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