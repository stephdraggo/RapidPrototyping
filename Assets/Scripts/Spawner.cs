using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crashteroids
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField, Range(1, 5)] private float spawnTimerMax=3;
        private float spawnTimer=1;

        [SerializeField] private GameObject rockPrefab;

        [SerializeField] private GameObject leftBound, rightBound;
        private float left => leftBound.transform.position.x;
        private float right => rightBound.transform.position.x;


        void Start()
        {

        }

        
        void Update()
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0)
            {
                Vector3 position = new Vector3(Random.Range(left,right), transform.position.y);
                Transform parent = transform;
                Instantiate(rockPrefab, position, Quaternion.identity, parent);
                spawnTimer = Random.Range(1, spawnTimerMax);
            }
        }
    }
}