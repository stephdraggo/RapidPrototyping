using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BigBoi.PlayerController
{
    /// <summary>
    /// 4-directional movement.
    /// Sprint and crouch speeds.
    /// Sprint is toggleable.
    /// </summary>
    [SelectionBase]
    [AddComponentMenu("BigBoi/Player Controllers/2D Movement (Transform)/Complex 2D Movement")]
    public class Controller2DComplex : Controller2DBasic
    {
        #region Variables

        //-----------------Serialised Protected------------------
        [SerializeField, Min(0)]
        protected float sprintSpeedMultiplier = 2, crouchSpeedMultiplier = 0.5f;

        [SerializeField]
        protected bool enableVariableSpeeds = true, toggleSprint = false;

        //----------------Protected------------------------------
        protected bool canSprint = true, sprintToggledOn = false;

        #endregion

       

        #region Public Methods (SetSprintAvailable - SwapSprintAvailable)

        /// <summary>
        /// Manually set whether player can sprint or not.
        /// </summary>
        public void SetSprintAvailable(bool enable) {
            canSprint = enable;
        }

        /// <summary>
        /// Switch from previous value whether player can sprint or not.
        /// </summary>
        public void SwapSprintAvailable() {
            canSprint = !canSprint;
        }

        #endregion


        #region Override Methods (Speed)

        /// <summary>
        /// Inherits from
        /// <see cref="ControllerBase.Speed">base.Speed</see>.
        /// Returns base speed if variable speed is not enabled.
        /// Checks if sprint is toggleable.
        /// Applies crouch or sprint modifier to base speed if applicable.
        /// Holding both keys cancel them out.
        /// </summary>
        /// <returns></returns>
        protected override float Speed() {
            if (!enableVariableSpeeds) return base.Speed();

            //reduce repeatedly fetching values
            KeyCode sprintKeyCode = KeyBinds["sprint"].key;

            if (toggleSprint && Input.GetKeyDown(sprintKeyCode)) {
                sprintToggledOn = !sprintToggledOn;
            }

            float speed = baseMovementSpeed;

            //get input as bools
            bool crouchCheck = Input.GetKey(KeyBinds["crouch"].key);
            bool sprintCheck = Input.GetKey(sprintKeyCode) || (toggleSprint && sprintToggledOn);

            if (sprintCheck && crouchCheck) {
                return speed;
            }

            if (sprintCheck && canSprint) {
                speed *= sprintSpeedMultiplier;
            }

            if (crouchCheck) {
                speed *= crouchSpeedMultiplier;
            }

            return speed;
        }

        #endregion
    }
}