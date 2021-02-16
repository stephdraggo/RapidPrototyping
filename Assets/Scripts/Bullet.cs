using UnityEngine;

namespace Crashteroids
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private GameObject highPoint;
        private float lifeSpan;

        [SerializeField, Range(1f, 10f)] private float speed = 1;

        void Start()
        {
            name = "Bullet";
            highPoint = FindObjectOfType<Spawner>().gameObject;
            lifeSpan = highPoint.transform.position.y + 1;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Rock"))
            {
                GameManager.score++;
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }

        void Update()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime,-1f);

            if (transform.position.y > lifeSpan)
            {
                Destroy(gameObject);
            }
        }
    }
}