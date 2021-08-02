using System.Collections;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    protected TriggerObject type;

    [SerializeField]
    private Vector2 wheelBounds;

    private void Start() {
        if (wheelBounds.magnitude > 0)
            StartCoroutine(MoveWheel());
    }

    private void OnTriggerEnter(Collider other) {
        Player player = other.gameObject.GetComponentInParent<Player>();
        if (player != null) {
            switch (type) {
                case TriggerObject.Damage:
                    player.TakeDamage();
                    break;

                case TriggerObject.Coin:
                    player.CollectCoin();
                    DestroyCoin();
                    break;

                case TriggerObject.End:
                    player.Win();
                    break;
            }
        }
    }

    void DestroyCoin() {
        Destroy(gameObject);
    }

    IEnumerator MoveWheel() {
        //logic for moving wheel
        yield return null;
    }
}

public enum TriggerObject
{
    Damage,
    Coin,
    End,
}