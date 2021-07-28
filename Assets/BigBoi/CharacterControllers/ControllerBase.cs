using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;

namespace BigBoi.PlayerController
{
    [SelectionBase]
    public abstract class ControllerBase : MonoBehaviour
    {
        #region Variables

        //-----------------Public--------------------------------


        //-----------------Serialised Protected------------------
        [SerializeField, Min(0)]
        protected float baseMovementSpeed = 1;

        [SerializeField] //add jump, sprint, crouch for 3D
        protected KeyBind[] keyBinds = new KeyBind[0];

        //----------------Protected------------------------------
        protected Dictionary<string, KeyBind> KeyBinds => StaticControllerInfo.Instance.keyBinds;
        protected KeyBind[] DefaultKeys {
            get {
                KeyBind[] defaultKeysSet = {
                    new KeyBind("forward", KeyCode.W),
                    new KeyBind("backward", KeyCode.S),
                    new KeyBind("left", KeyCode.A),
                    new KeyBind("right", KeyCode.D),
                    new KeyBind("jump", KeyCode.Space),
                    new KeyBind("sprint", KeyCode.LeftShift),
                    new KeyBind("crouch", KeyCode.LeftControl),
                    new KeyBind("interact", KeyCode.E),
                };
                return defaultKeysSet;
            }
        }

        #endregion

        #region Unity Methods (OnValidate - Awake)

        /// <summary>
        /// Set default keybinds if there are no keybinds
        /// </summary>
        protected virtual void OnValidate() {
            if (keyBinds.Length <= 0) {
                keyBinds = DefaultKeys;
            }
        }

        /// <summary>
        /// Validate keybinds.
        /// Check for duplicate keys.
        /// Compile keybind dictionary.
        /// </summary>
        protected virtual void Awake() {
            OnValidate(); //validate one last time

            //validate keybinds
            foreach (KeyBind keyBind in keyBinds) {
                keyBind.OnValidateName();
            }

            //do this before setting dictionary
            CheckKeyDuplicates();

            //set dictionary
            keyBinds.CompileDictionary();
        }

        #endregion

        #region Setup Methods (CheckKeyDuplicates)

        /// <summary>
        /// !Important! Call this before setting keybind dictionary
        /// Stores used keys and name in temp lists and checks for duplicates of these.
        /// </summary>
        protected void CheckKeyDuplicates() {
            //temporary lists to keep track of used keys and names
            List<KeyCode> keysUsed = new List<KeyCode>();
            List<string> namesUsed = new List<string>();

            //go through keybinds and check if each is a double of another, otherwise add to temp lists for next keybind check
            foreach (KeyBind keyBind in keyBinds) {
                if (keysUsed.Contains(keyBind.key)) {
                    keyBind.OnDuplicateKeyFound();
                }
                else {
                    keysUsed.Add(keyBind.key);
                }

                if (namesUsed.Contains(keyBind.name)) {
                    keyBind.OnDuplicateNameFound();
                }
                else {
                    namesUsed.Add(keyBind.name);
                }
            }
        }

        #endregion

        #region Abstact Runtime Methods (Direction - Move)

        /// <summary>
        /// Logic for determining movement direction from player input.
        /// Remember to Normalise before returning value.
        /// </summary>
        /// <returns>vector3 direction</returns>
        protected abstract Vector3 Direction();

        /// <summary>
        /// Call in Update to move character.
        /// Use to combine direction with speed and any other movement factors.
        /// </summary>
        /// <param name="direction">current direction for movement</param>
        /// <param name="speed">current movement speed</param>
        protected abstract void Move(Vector3 direction, float speed);

        /// <summary>
        /// Calculate variable speed.
        /// </summary>
        /// <returns></returns>
        protected virtual float Speed() {
            return baseMovementSpeed;
        }

        #endregion
    }
}