using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crashteroids
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private GameObject highPoint;
        private float lifeSpan => highPoint.transform.position.y+1;

        [SerializeField, Range(1f, 10f)] private float speed = 1;

        void Start()
        {
            name = "Bullet";
            highPoint = FindObjectOfType<Spawner>().gameObject;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Rock"))
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }

        void Update()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime);

            if (transform.position.y > lifeSpan)
            {
                Destroy(gameObject);
            }
        }
    }
}