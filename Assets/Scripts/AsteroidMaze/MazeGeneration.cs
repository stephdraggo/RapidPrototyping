using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidMaze
{
    public class MazeGeneration : MonoBehaviour
    {
        #region Variables
        float mazeWidth, mazeHeight;

        [SerializeField] private GameObject blockerBlock, backgroundBlock,player;
        [SerializeField] private GameObject[] backgroundRows;

        private List<GameObject> backgroundBlocks;
        #endregion

        #region Start
        void Start()
        {
            backgroundBlocks = new List<GameObject>();
            FillBackgroundRows();
            SpawnBlocks();
        }
        #endregion

        #region Update
        void Update()
        {

        }
        #endregion

        #region Methods
        private void FillBackgroundRows()
        {
            foreach (GameObject _row in backgroundRows)
            {
                for (int i = 0; i < 18; i++)
                {
                    GameObject newBG = Instantiate(backgroundBlock, _row.transform);
                    newBG.name = "space " + i.ToString();
                    backgroundBlocks.Add(newBG);
                }
            }
        }

        private void SpawnBlocks()
        {
            foreach (GameObject _sky in backgroundBlocks)
            {
                if (Vector2.SqrMagnitude(_sky.transform.position - player.transform.position) < 2)
                {
                    break;
                }


                Vector3 pos=_sky.transform.position;
                /*
                int corner = Random.Range(0, 3);

                switch (corner)
                {
                    case 0:
                        pos += new Vector3(.5f, .5f);
                        break;
                    case 1:
                        pos += new Vector3(.5f, -.5f);
                        break;
                    case 2:
                        pos += new Vector3(-.5f, .5f);
                        break;
                    case 3:
                        pos += new Vector3(-.5f, -.5f);
                        break;
                    default:
                        break;
                }*/

                GameObject newBlock = Instantiate(blockerBlock, pos, Quaternion.identity, _sky.transform);

            }
        }
        #endregion
    }
}