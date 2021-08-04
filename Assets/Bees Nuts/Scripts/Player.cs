using System.Collections;
using System.Collections.Generic;
using BigBoi.PlayerController;
using UnityEngine;

namespace BeesNuts
{
    public class Player : MonoBehaviour
    {
        #region Variables

        public static GameObject CurrentHover;

        public static Player Instance;

        public int honeyComb { get; private set; }
        public float health;


        private ControllerBase controller;
        private SpriteRenderer renderer;

        #endregion

        private void Awake() {
            controller = GetComponent<ControllerBase>();
            Instance = this;
            renderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Update() {
            if (Input.GetKeyDown(controller.KeyBinds["left"].key)) {
                renderer.flipX = false;
            }
            else if (Input.GetKeyDown(controller.KeyBinds["right"].key)) {
                renderer.flipX = true;
            }

            if (CurrentHover) {
                if (Input.GetKeyDown(controller.KeyBinds["interact"].key)) {
                    if (CurrentHover.TryGetComponent(out Hive hive)) {
                        GetHoney(hive);
                    }
                    else if (CurrentHover.TryGetComponent(out DepositBox box)) {
                        DepositHoney();
                    }
                }
            }
        }

        public void GetHurt(int swarmSize) {
            health -= swarmSize;
            if (health <= 0) {
                health = 0;
                SceneFlowManager.Instance.Lose();
            }
        }

        private void GetHoney(Hive hive) {
            honeyComb++;
            hive.GetHit();
        }

        private void DepositHoney() {
            DepositBox.Instance.ScoreChange(honeyComb);

            honeyComb = 0;
        }
    }
}