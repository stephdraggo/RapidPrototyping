using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BigBoi.PlayerController
{
    /// <summary>
    /// 4-directional movement.
    /// </summary>
    [SelectionBase]
    [AddComponentMenu("BigBoi/Player Controllers/2D Movement (Transform)/Basic 2D Movement")]
    public class Controller2DBasic : ControllerBase
    {
        #region Variables

        //-----------------Serialised Protected------------------
        [SerializeField]
        protected Dimension dimension;

        #endregion

        #region Unity Methods (Update)

        protected virtual void Update() {
            Move(Direction(), Speed());
        }

        #endregion
        
        #region Override Methods (Direction - Move - Speed)

        protected override Vector3 Direction() {
            Vector3 direction = Vector3.zero;

            //assign y or z to forward/backward depending on dimension 
            switch (dimension) {
                case Dimension.Horizontal:
                    if (Input.GetKey(KeyBinds["forward"].key)) {
                        direction.z++;
                    }

                    if (Input.GetKey(KeyBinds["backward"].key)) {
                        direction.z--;
                    }

                    break;

                case Dimension.Vertical:
                    if (Input.GetKey(KeyBinds["forward"].key)) {
                        direction.y++;
                    }

                    if (Input.GetKey(KeyBinds["backward"].key)) {
                        direction.y--;
                    }

                    break;
            }

            if (Input.GetKey(KeyBinds["left"].key)) {
                direction.x--;
            }

            if (Input.GetKey(KeyBinds["right"].key)) {
                direction.x++;
            }

            return direction.normalized;
        }

        protected override void Move(Vector3 direction, float speed) {
            transform.position += speed * Time.deltaTime * direction;
        }

        #endregion
    }

    public enum Dimension
    {
        Horizontal,
        Vertical,
    }
}