using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FPSControllerLPFP
{
    /// Manages a first person character
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(AudioSource))]
    public class FpsControllerLPFP : MonoBehaviour
    {
#pragma warning disable 649

        private static FpsControllerLPFP instance;
        public static FpsControllerLPFP MyInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<FpsControllerLPFP>();
                }

                return instance;
            }
        }

        public Transform[] armsList;

        [SerializeField]
        private Transform arms;


        [SerializeField]
        private Vector3 armPosition;

        [SerializeField]
        private AudioClip walkingSound;

        [SerializeField]
        private AudioClip runningSound;

        [SerializeField]
        private float walkingSpeed = 5f;

        [SerializeField]
        private float runningSpeed = 9f;

        [SerializeField]
        private float movementSmoothness = 0.125f;

        [SerializeField]
        private float jumpForce = 35f;

        [SerializeField]
        private float mouseSensitivity = 7f;

        [SerializeField]
        private float rotationSmoothness = 0.05f;

        [SerializeField]
        private float minVerticalAngle = -90f;

        [SerializeField]
        private float maxVerticalAngle = 90f;

        [SerializeField]
        private FpsInput input;

        private int evade = 0;
        private bool evadeFlag = false;
#pragma warning restore 649

        private Rigidbody _rigidbody;
        private CapsuleCollider _collider;
        private AudioSource _audioSource;
        private SmoothRotation _rotationX;
        private SmoothRotation _rotationY;
        private SmoothVelocity _velocityX;
        private SmoothVelocity _velocityZ;
        private bool _isGrounded;
        public static bool isEvading;
        private float evadingTimer = 0;
        private float maxEvadingTimer = 0.4f;
        private readonly RaycastHit[] _groundCastResults = new RaycastHit[8];
        private readonly RaycastHit[] _wallCastResults = new RaycastHit[8];

        /// Initializes the FpsController on start.
        private void Start()
        {
            arms = armsList[0];
            arms = AssignCharactersCamera();
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _collider = GetComponent<CapsuleCollider>();
            _audioSource = GetComponent<AudioSource>();

            _audioSource.clip = walkingSound;
            _audioSource.loop = true;
            _rotationX = new SmoothRotation(RotationXRaw);
            _rotationY = new SmoothRotation(RotationYRaw);
            _velocityX = new SmoothVelocity();
            _velocityZ = new SmoothVelocity();
            Cursor.lockState = CursorLockMode.Locked;
            ValidateRotationRestriction();
        }

        private Transform AssignCharactersCamera()
        {
            var t = transform;
            arms.SetPositionAndRotation(t.position, t.rotation);
            return arms;
        }

        /// Clamps <see cref="minVerticalAngle"/> and <see cref="maxVerticalAngle"/> to valid values and
        /// ensures that <see cref="minVerticalAngle"/> is less than <see cref="maxVerticalAngle"/>.
        private void ValidateRotationRestriction()
        {
            minVerticalAngle = ClampRotationRestriction(minVerticalAngle, -90, 90);
            maxVerticalAngle = ClampRotationRestriction(maxVerticalAngle, -90, 90);
            if (maxVerticalAngle >= minVerticalAngle) return;
            Debug.LogWarning("maxVerticalAngle should be greater than minVerticalAngle.");
            var min = minVerticalAngle;
            minVerticalAngle = maxVerticalAngle;
            maxVerticalAngle = min;
        }

        private static float ClampRotationRestriction(float rotationRestriction, float min, float max)
        {
            if (rotationRestriction >= min && rotationRestriction <= max) return rotationRestriction;
            var message = string.Format("Rotation restrictions should be between {0} and {1} degrees.", min, max);
            Debug.LogWarning(message);
            return Mathf.Clamp(rotationRestriction, min, max);
        }

        /// Checks if the character is on the ground.
        private void OnCollisionStay()
        {
            var bounds = _collider.bounds;
            var extents = bounds.extents;
            var radius = extents.x - 0.01f;
            Physics.SphereCastNonAlloc(bounds.center, radius, Vector3.down,
                _groundCastResults, extents.y - radius * 0.5f, ~0, QueryTriggerInteraction.Ignore);
            if (!_groundCastResults.Any(hit => hit.collider != null && hit.collider != _collider)) return;
            for (var i = 0; i < _groundCastResults.Length; i++)
            {
                _groundCastResults[i] = new RaycastHit();
            }

            _isGrounded = true;
        }

        /// Processes the character movement and the camera rotation every fixed framerate frame.
        private void FixedUpdate()
        {
            bool canMove = true;
            if (GameObject.FindWithTag("Hunter") != null)
            {
                if (GameObject.FindWithTag("Hunter").activeInHierarchy != null)
                {
                    canMove = hunterControl.MyInstance.canMove;
                }
            }
            if (arms != armsList[JoelScript.weaponIndex])
            {
                arms = armsList[JoelScript.weaponIndex];
                arms = AssignCharactersCamera();
            }
            // FixedUpdate is used instead of Update because this code is dealing with physics and smoothing.
            RotateCameraAndCharacter();
            if (canMove)
            {
                MoveCharacter();
            }

            _isGrounded = false;
            if (SceneManager.GetActiveScene().name == "Mountain")
            {
                if (transform.position.z >= 60 && transform.position.z <= 940 && transform.position.x >= 50 && transform.position.x <= 960)
                {
                    if (isEvading && canMove)
                    {
                        var direction = new Vector3(input.Move, 0f, input.Strafe).normalized;
                        var worldDirection = transform.TransformDirection(direction);
                        var velocity = worldDirection * 45;
                        //Checks for collisions so that the character does not stuck when jumping against walls.
                        var intersectsWall = CheckCollisionsWithWalls(velocity);
                        if (intersectsWall)
                        {
                            _velocityX.Current = _velocityZ.Current = 0f;
                            return;
                        }

                        var smoothX = _velocityX.Update(velocity.x, movementSmoothness);
                        var smoothZ = _velocityZ.Update(velocity.z, movementSmoothness);
                        var rigidbodyVelocity = _rigidbody.velocity;
                        var force = new Vector3(smoothX - rigidbodyVelocity.x, 0f, smoothZ - rigidbodyVelocity.z + 2f);
                        this.gameObject.transform.Translate(force);
                        evadingTimer += Time.deltaTime;
                    }
                }

                if (maxEvadingTimer < evadingTimer)
                {
                    isEvading = false;
                    evadingTimer = 0;
                }
            }
            else if (SceneManager.GetActiveScene().name == "Forest")
            {
                if (transform.position.z >= 310 && transform.position.z <= 770 && transform.position.x >= 305 && transform.position.x <= 730)
                {
                    if (isEvading && canMove)
                    {
                        var direction = new Vector3(input.Move, 0f, input.Strafe).normalized;
                        var worldDirection = transform.TransformDirection(direction);
                        var velocity = worldDirection * 90f;
                        //Checks for collisions so that the character does not stuck when jumping against walls.
                        var intersectsWall = CheckCollisionsWithWalls(velocity);
                        if (intersectsWall)
                        {
                            _velocityX.Current = _velocityZ.Current = 0f;
                            return;
                        }

                        var smoothX = _velocityX.Update(velocity.x, movementSmoothness);
                        var smoothZ = _velocityZ.Update(velocity.z, movementSmoothness);
                        var rigidbodyVelocity = _rigidbody.velocity;
                        var force = new Vector3(smoothX - rigidbodyVelocity.x, 0f, smoothZ - rigidbodyVelocity.z + 2f);
                        this.gameObject.transform.Translate(force);
                        evadingTimer += Time.deltaTime;
                    }
                }

                if (maxEvadingTimer < evadingTimer)
                {
                    isEvading = false;
                    evadingTimer = 0;
                }
            }

            else
            {
                // do nothing
            }

        }

        /// Moves the camera to the character, processes jumping and plays sounds every frame.
        private void Update()
        {
            bool canMove = true;
            if (GameObject.FindWithTag("Hunter") != null)
            {
                if (GameObject.FindWithTag("Hunter").activeInHierarchy != null)
                {
                    canMove = hunterControl.MyInstance.canMove;
                }
            }

            if (arms != armsList[JoelScript.weaponIndex])
            {
                arms = armsList[JoelScript.weaponIndex];
                arms = AssignCharactersCamera();
            }
            arms.position = transform.position + transform.TransformVector(armPosition);
            Jump();
            PlayFootstepSounds();
            if (Input.GetKeyDown(KeyCode.LeftControl) && canMove)
            {

                isEvading = true;


            }

        }

        private void RotateCameraAndCharacter()
        {
            var rotationX = _rotationX.Update(RotationXRaw, rotationSmoothness);
            var rotationY = _rotationY.Update(RotationYRaw, rotationSmoothness);
            var clampedY = RestrictVerticalRotation(rotationY);
            _rotationY.Current = clampedY;
            var worldUp = arms.InverseTransformDirection(Vector3.up);
            var rotation = arms.rotation *
                           Quaternion.AngleAxis(rotationX, worldUp) *
                           Quaternion.AngleAxis(clampedY, Vector3.left);
            transform.eulerAngles = new Vector3(0f, rotation.eulerAngles.y, 0f);
            arms.rotation = rotation;
        }

        /// Returns the target rotation of the camera around the y axis with no smoothing.
        private float RotationXRaw
        {
            get { return input.RotateX * mouseSensitivity; }
        }

        /// Returns the target rotation of the camera around the x axis with no smoothing.
        private float RotationYRaw
        {
            get { return input.RotateY * mouseSensitivity; }
        }

        /// Clamps the rotation of the camera around the x axis
        /// between the <see cref="minVerticalAngle"/> and <see cref="maxVerticalAngle"/> values.
        private float RestrictVerticalRotation(float mouseY)
        {
            var currentAngle = NormalizeAngle(arms.eulerAngles.x);
            var minY = minVerticalAngle + currentAngle;
            var maxY = maxVerticalAngle + currentAngle;
            return Mathf.Clamp(mouseY, minY + 0.01f, maxY - 0.01f);
        }

        /// Normalize an angle between -180 and 180 degrees.
        /// <param name="angleDegrees">angle to normalize</param>
        /// <returns>normalized angle</returns>
        private static float NormalizeAngle(float angleDegrees)
        {
            while (angleDegrees > 180f)
            {
                angleDegrees -= 360f;
            }

            while (angleDegrees <= -180f)
            {
                angleDegrees += 360f;
            }

            return angleDegrees;
        }

        private void MoveCharacter()
        {
            var direction = new Vector3(input.Move, 0f, input.Strafe).normalized;
            var worldDirection = transform.TransformDirection(direction);
            var velocity = worldDirection * (input.Run ? runningSpeed : walkingSpeed);
            //Checks for collisions so that the character does not stuck when jumping against walls.
            var intersectsWall = CheckCollisionsWithWalls(velocity);
            if (intersectsWall)
            {
                _velocityX.Current = _velocityZ.Current = 0f;
                return;
            }

            var smoothX = _velocityX.Update(velocity.x, movementSmoothness);
            var smoothZ = _velocityZ.Update(velocity.z, movementSmoothness);
            var rigidbodyVelocity = _rigidbody.velocity;
            var force = new Vector3(smoothX - rigidbodyVelocity.x, 0f, smoothZ - rigidbodyVelocity.z);
            _rigidbody.AddForce(force, ForceMode.VelocityChange);
        }

        private bool CheckCollisionsWithWalls(Vector3 velocity)
        {
            if (_isGrounded) return false;
            var bounds = _collider.bounds;
            var radius = _collider.radius;
            var halfHeight = _collider.height * 0.5f - radius * 1.0f;
            var point1 = bounds.center;
            point1.y += halfHeight;
            var point2 = bounds.center;
            point2.y -= halfHeight;
            Physics.CapsuleCastNonAlloc(point1, point2, radius, velocity.normalized, _wallCastResults,
                radius * 0.04f, ~0, QueryTriggerInteraction.Ignore);
            var collides = _wallCastResults.Any(hit => hit.collider != null && hit.collider != _collider);
            if (!collides) return false;
            for (var i = 0; i < _wallCastResults.Length; i++)
            {
                _wallCastResults[i] = new RaycastHit();
            }

            return true;
        }

        private void Jump()
        {
            if (!_isGrounded || !input.Jump) return;
            _isGrounded = false;
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private void PlayFootstepSounds()
        {
            if (_isGrounded && _rigidbody.velocity.sqrMagnitude > 0.1f)
            {
                _audioSource.clip = input.Run ? runningSound : walkingSound;
                if (!_audioSource.isPlaying)
                {
                    _audioSource.Play();
                }
            }
            else
            {
                if (_audioSource.isPlaying)
                {
                    _audioSource.Pause();
                }
            }
        }

        /// A helper for assistance with smoothing the camera rotation.
        private class SmoothRotation
        {
            private float _current;
            private float _currentVelocity;

            public SmoothRotation(float startAngle)
            {
                _current = startAngle;
            }

            /// Returns the smoothed rotation.
            public float Update(float target, float smoothTime)
            {
                return _current = Mathf.SmoothDampAngle(_current, target, ref _currentVelocity, smoothTime);
            }

            public float Current
            {
                set { _current = value; }
            }
        }

        /// A helper for assistance with smoothing the movement.
        private class SmoothVelocity
        {
            private float _current;
            private float _currentVelocity;

            /// Returns the smoothed velocity.
            public float Update(float target, float smoothTime)
            {
                return _current = Mathf.SmoothDamp(_current, target, ref _currentVelocity, smoothTime);
            }

            public float Current
            {
                set { _current = value; }
            }
        }

        /// Input mappings
        [Serializable]
        private class FpsInput
        {
            [Tooltip("The name of the virtual axis mapped to rotate the camera around the y axis."),
             SerializeField]
            private string rotateX = "Mouse X";

            [Tooltip("The name of the virtual axis mapped to rotate the camera around the x axis."),
             SerializeField]
            private string rotateY = "Mouse Y";

            [Tooltip("The name of the virtual axis mapped to move the character back and forth."),
             SerializeField]
            private string move = "Horizontal";

            [Tooltip("The name of the virtual axis mapped to move the character left and right."),
             SerializeField]
            private string strafe = "Vertical";

            [Tooltip("The name of the virtual button mapped to run."),
             SerializeField]
            private string run = "Fire3";

            [Tooltip("The name of the virtual button mapped to jump."),
             SerializeField]
            private string jump = "Jump";

            /// Returns the value of the virtual axis mapped to rotate the camera around the y axis.
            public float RotateX
            {
                get { return Input.GetAxisRaw(rotateX); }
            }

            /// Returns the value of the virtual axis mapped to rotate the camera around the x axis.        
            public float RotateY
            {
                get { return Input.GetAxisRaw(rotateY); }
            }

            /// Returns the value of the virtual axis mapped to move the character back and forth.        
            public float Move
            {
                get { return Input.GetAxisRaw(move); }
            }

            /// Returns the value of the virtual axis mapped to move the character left and right.         
            public float Strafe
            {
                get { return Input.GetAxisRaw(strafe); }
            }

            /// Returns true while the virtual button mapped to run is held down.          
            public bool Run
            {
                get { return Input.GetButton(run); }
            }

            /// Returns true during the frame the user pressed down the virtual button mapped to jump.          
            public bool Jump
            {
                get { return Input.GetButtonDown(jump); }
            }
        }
    }
}