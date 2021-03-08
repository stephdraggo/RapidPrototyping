using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GyroGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Rigidbody bodyy;

        
        void Start()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            ResetScene();
        }

        void Update()
        {
            bodyy.WakeUp();
        }

        public void ResetScene()
        {
            SceneManager.LoadScene(0);
        }
    }
}