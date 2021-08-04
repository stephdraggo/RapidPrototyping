using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeesNuts
{
    [RequireComponent(typeof(SphereCollider))]
    public class Swarm : MonoBehaviour
    {
        public static Swarm Instance;
        
        [SerializeField]
        private int speed = 5;
        
        private int size = 1;
        private float yPos;
        private int useSpeed;
        private ParticleSystem particles;

        void Awake() {
            yPos = transform.position.y;
            Instance = this;
            useSpeed = speed;
            particles = GetComponent<ParticleSystem>();
        }

        void Update() {
            Move();
        }

        public void AddSwarm() {
            if (size == 0) {
                useSpeed = speed;
            }
            size ++;
            transform.localScale = new Vector3(size, size, size);
        }

        private void Move() {
            Vector3 temp = Vector3.MoveTowards(transform.position, Player.Instance.transform.position,
                useSpeed * Time.deltaTime);
            transform.position = new Vector3(temp.x, yPos, temp.z);
        }

        private void StingPlayer() {
            Player.Instance.GetHurt(size);
            size--;
            transform.localScale = new Vector3(size, size, size);
            if (size <= 0) {
                useSpeed = 0;
            }
        }


        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.TryGetComponent(out Player player)) {
                StingPlayer();
            }
        }

        private void OnMouseEnter() {
            Player.CurrentHover = gameObject;
        }

        private void OnMouseExit() {
            if (Player.CurrentHover == gameObject) Player.CurrentHover = null;
        }
    }
}