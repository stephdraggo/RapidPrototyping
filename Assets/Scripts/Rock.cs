using UnityEngine;

namespace Crashteroids
{
    public class Rock : MonoBehaviour
    {
        private float speed = 2;
        public float bottom { get; private set; }
        void Start()
        {
            transform.localScale *= Random.Range(0.2f, 2f);
            name = "Asteroid";
            speed = Random.Range(1, 5);
            bottom =  -FindObjectOfType<Spawner>().transform.position.y;
        }

        void Update()
        {
            if (transform.position.y <= bottom)
            {
                Destroy(gameObject);
            }
            transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime);
        }
    }
}