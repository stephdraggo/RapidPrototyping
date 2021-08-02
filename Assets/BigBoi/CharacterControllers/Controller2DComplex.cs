using System.Collections;
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
        protected float sprintSpeedMultiplier = 2,
            crouchSpeedMultiplier = 0.5f,
            jumpForceMultiplier = 1,
            jumpMaxTime = 1,
            fallSpeedIncreaseMultiplier = 1;

        [SerializeField]
        protected bool enableVariableSpeeds = true, toggleSprint = false;

        [SerializeField, Tooltip("Use jump mechanic to move upwards instead of directional movement.")]
        protected bool jumpOverrideUp = false;

        [SerializeField]
        protected Collider groundCollider;

        //----------------Protected------------------------------
        protected bool canSprint = true, sprintToggledOn = false, jumping = false;

        protected bool falling => !(jumping || groundCheck.IsGrounded);

        protected float upDownMovement;

        protected GroundCheck groundCheck;

        protected Player player;

        #endregion


        private void Start() {
            //jump component sends a message to this class when it collides with the ground
            //check if there are two jump components -> log warning

            player = GetComponent<Player>();

            if (groundCollider) {
                groundCheck = groundCollider.TryGetComponent(out GroundCheck ground)
                    ? ground
                    : groundCollider.gameObject.AddComponent<GroundCheck>();
            }

            //StartCoroutine(Jump());
        }

        protected override void Update() {
            base.Update();

            //Debug.Log("falling: "+falling);

            if (falling && !jumping) {
                StartCoroutine(Fall());
            }
        }


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
        /// Checks if jumping is allowed and being called.
        /// Starts jump IEnumerator if needed.
        /// </summary>
        protected virtual void JumpCheck() {
            if (!jumpOverrideUp) return;
            if (jumping) return;
            if (falling) return;

            if (Input.GetKey(KeyBinds["jump"].key)) {
                jumping = true;
                StartCoroutine(Jump());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual IEnumerator Jump() {
            float timer = jumpMaxTime;

            while (timer > 0) {
                float timeAsLerp = 1 - timer / jumpMaxTime;
                float speedMultiplier = Mathf.Lerp(jumpForceMultiplier, 0, timeAsLerp) * player.SpeedMod;

                upDownMovement = speedMultiplier;

                yield return null;
                timer -= Time.deltaTime;
                //if (groundCheck.IsGrounded) break; //stop jumping when on ground
                if (Input.GetKeyUp(KeyBinds["jump"].key)) break; //stop jumping if no jump input
            }

            upDownMovement = 0;

            StartCoroutine(Fall());

            jumping = false;
        }

        protected virtual IEnumerator Fall() {
            float timer = 0;

            yield return null;
            while (falling) {
                upDownMovement = -(fallSpeedIncreaseMultiplier * timer) / player.SpeedMod; //gravity, modified by time
                upDownMovement *= Time.deltaTime;

                timer += Time.deltaTime;
                yield return null;
            }

            upDownMovement = 0;
        }

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
            if (!enableVariableSpeeds) return base.Speed() * player.SpeedMod;

            //reduce repeatedly fetching values
            KeyCode sprintKeyCode = KeyBinds["sprint"].key;

            if (toggleSprint && Input.GetKeyDown(sprintKeyCode)) {
                sprintToggledOn = !sprintToggledOn;
            }

            float speed = baseMovementSpeed;

            //get input as bools
            bool crouchCheck = Input.GetKey(KeyBinds["crouch"].key);
            bool sprintCheck = Input.GetKey(sprintKeyCode) || (toggleSprint && sprintToggledOn);

            if (sprintCheck) {
                if (crouchCheck) {
                    return speed;
                }

                if (canSprint) {
                    speed *= sprintSpeedMultiplier;
                }
            }

            if (crouchCheck) {
                speed *= crouchSpeedMultiplier;
            }

            return speed;
        }

        protected override Vector3 Direction() {
            Vector3 direction = base.Direction();

            if (!jumpOverrideUp) return direction;

            JumpCheck();

            //replace upward directional movement is jump should override
            switch (dimension) {
                case Dimension.Horizontal:
                    direction.z = upDownMovement;
                    break;

                case Dimension.Vertical:
                    direction.y = upDownMovement;
                    break;
            }

            return direction;
        }

        #endregion
    }
}