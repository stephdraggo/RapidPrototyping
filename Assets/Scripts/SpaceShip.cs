using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crashteroids
{
    public class SpaceShip : MonoBehaviour
    {
        #region Variables
        [SerializeField, Range(0.1f, 5f)]
        private float speed = 1;

        [SerializeField] private GameObject leftBound, rightBound;
        private float left => leftBound.transform.position.x;
        private float right => rightBound.transform.position.x;

        [SerializeField, Range(0.01f, 1f)] private float coolDown;
        [Min(0)] private float timer = 0;

        [SerializeField] private GameObject bulletPrefab, bulletParent;

        #endregion

        #region Start
        void Start()
        {
            
        }
        #endregion

        #region Collision
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Rock"))
            {
                Crash();
            }
        }
        #endregion

        #region Update
        void Update()
        {
            timer -= Time.deltaTime;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                if (transform.position.x > left)
                {
                    Move(left);
                }
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                if (transform.position.x < right)
                {
                    Move(right);
                }
            }
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.F))
            {
                Fire();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// move towards the gievn side of the map according to speed
        /// </summary>
        /// <param name="_dir">x value of target location</param>
        private void Move(float _dir)
        {
            transform.position += new Vector3(_dir, 0) * speed * Time.deltaTime;
        }
        private void Fire()
        {
            if (timer <= 0)
            {
                Vector3 position = new Vector3(transform.position.x, transform.position.y);
                Transform parent = bulletParent.transform;
                Instantiate(bulletPrefab, position, Quaternion.identity, parent);
                timer = coolDown;
            }
        }
        private void Crash()
        {
            Debug.Log("crashed");
        }
        #endregion
    }
}