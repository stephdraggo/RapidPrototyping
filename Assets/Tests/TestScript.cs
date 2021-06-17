using System.Collections;
using System.Collections.Generic;
using Crashteroids;
using NUnit.Framework;
using UnityEngine;
using UAssert = UnityEngine.Assertions.Assert;
using UnityEngine.TestTools;


public class TestScript
{
    private GameObject gamePrefab;
    private Rock rock;

    [UnityTest]
    public IEnumerator AsteroidMovesDown()
    {
        SpaceShip.NoDeath = true;
        Spawner.DoSpawn = false;
        gamePrefab ??= MonoBehaviour.Instantiate(Resources.Load<GameObject>("game"));

        rock ??= MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Asteroid"), gamePrefab.transform)
            .GetComponent<Rock>();
        float initialYPosition = rock.transform.position.y;
        yield return new WaitForSeconds(1f);

        Assert.Less(rock.transform.position.y, initialYPosition);

        yield return null;
    }

    [UnityTest]
    public IEnumerator BulletMovesUp()
    {
        SpaceShip.NoDeath = true;
        Spawner.DoSpawn = false;
        gamePrefab ??= MonoBehaviour.Instantiate(Resources.Load<GameObject>("game"));

        Bullet bullet = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"), gamePrefab.transform)
            .GetComponent<Bullet>();
        float initialYPosition = bullet.transform.position.y;
        yield return new WaitForSeconds(1f);

        Assert.Greater(bullet.transform.position.y, initialYPosition);

        GameObject.Destroy(rock.gameObject);
        yield return null;
    }

    [UnityTest]
    public IEnumerator DestroyAsteroidOffscreen()
    {
        SpaceShip.NoDeath = true;
        Spawner.DoSpawn = false;
        gamePrefab ??= MonoBehaviour.Instantiate(Resources.Load<GameObject>("game"));

        rock = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Asteroid"), gamePrefab.transform)
            .GetComponent<Rock>();
        rock.gameObject.transform.position = new Vector3(0, -100, 0);
        yield return new WaitForSeconds(1f);

        UAssert.IsNull(rock);
        if(rock) GameObject.Destroy(rock.gameObject);
        
        yield return null;
    }

    [UnityTest]
    public IEnumerator DestroyBulletOffscreen()
    {
        SpaceShip.NoDeath = true;
        Spawner.DoSpawn = false;
        gamePrefab ??= MonoBehaviour.Instantiate(Resources.Load<GameObject>("game"));

        Bullet bullet = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"), gamePrefab.transform)
            .GetComponent<Bullet>();
        bullet.gameObject.transform.position = new Vector3(0, 100, 0);
        yield return new WaitForSeconds(1f);

        UAssert.IsNull(bullet);
        
        yield return null;
    }

    [UnityTest]
    public IEnumerator DieOnCollision()
    {
        SpaceShip.NoDeath = false;
        Spawner.DoSpawn = false;
        gamePrefab ??= MonoBehaviour.Instantiate(Resources.Load<GameObject>("game"));
        
        rock = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Asteroid"), gamePrefab.transform)
            .GetComponent<Rock>();
        rock.gameObject.transform.position =  Vector3.zero;

        //yield return new WaitForSeconds(1);
        for (int i = 0; i < 1000; i++) { yield return null; }

        Vector3 collisionPos = rock.gameObject.transform.position;

        for (int i = 0; i < 10; i++) { yield return null; }
        
        Assert.AreEqual(collisionPos,rock.gameObject.transform.position);

        yield return null;
    }
}