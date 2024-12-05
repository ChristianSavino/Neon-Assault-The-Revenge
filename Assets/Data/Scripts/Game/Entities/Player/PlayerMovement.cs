using Keru.Scripts.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _maxSpeed;
        [SerializeField] private bool _canDoubleJump;
        [SerializeField] private int _maxDashes;

        private PlayerThirdPersonAnimations _animations;
        private PlayerUIHandler _uIHandler;
        private Dictionary<string, KeyCode> _keys;

        private Rigidbody _rigidBody;
        private Camera _camera;
        private CapsuleCollider _capsuleCollider;

        private float _yaw, _pitch, _sensitivity;
        //Walking
        private float _originalSpeed;
        private float _acceleration;
        private Vector2 _axis;
        private float _x = 0;
        private float _z = 0;

        //Dashing
        private int _dash;
        private bool _canDash;
        private bool _isDashing;
        
        [SerializeField] private float dashRefreshTime = 1f;
        [SerializeField] private AudioClip dashClip;
        [SerializeField] private AudioClip dashRestoreClip;

        private bool _isCrouching;

        //Jump/DoubleJump
        
        private bool _isAbleToDoubleJump;

        private Vector3 _legsCrouchPosition = new Vector3(0, -0.218f, 0);
        private Vector3 _legsStandingPosition = new Vector3(0, -0.662f, 0);
        private Vector3 _fullBodyStanding = new Vector3(0, -1, 0);
        private Vector3 _fullBodyCrouching = new Vector3(0, -0.396f, 0);
        private Vector3 _crouchCenter = new Vector3(0, 0.33f, 0);
        private float _percentCrouch;

        private void Start()
        {
            _camera = Camera.main;
            _rigidBody = GetComponent<Rigidbody>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _percentCrouch = 0;

            Cursor.lockState = CursorLockMode.Locked;
            StartCoroutine(WalkSound());
        }
        public void SetConfig(PlayerThirdPersonAnimations animationReference, PlayerUIHandler uiHandler, Dictionary<string, KeyCode> keys, float sensivity)
        {
            _animations = animationReference;
            _keys = keys;
            _sensitivity = sensivity;
            _uIHandler = uiHandler;
        }
        
        private void Update()
        {
            Look();
        }

        private void FixedUpdate()
        {
            Movement();
            JumpMovement();
            //Dash();
            //Crouching();
        }

        private void Look()
        {
            _pitch -= Input.GetAxisRaw("Mouse Y") * (_sensitivity * 100) * Time.deltaTime;
            _pitch = Mathf.Clamp(_pitch, -89f, 90f);
            _yaw += Input.GetAxisRaw("Mouse X") * (_sensitivity * 100) * Time.deltaTime;
            _camera.transform.localRotation = Quaternion.Euler(_pitch, 0, 0);
            transform.localRotation = Quaternion.Euler(0, _yaw, 0);
        }

        private void JumpMovement()
        {
            if (Input.GetKeyDown(_keys["Jump"]) && _isAbleToDoubleJump)
            {
                Jump();
                _isAbleToDoubleJump = false;
            }
            if (Input.GetKey(_keys["Jump"]) && GroundCheck())
            {
                Jump();
                _isAbleToDoubleJump = _canDoubleJump;
            }
        }

        private void Jump()
        {
            _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, 5.0f, _rigidBody.velocity.z);
            _animations.PlayAnimation(AnimationNamesHelper.JumpAnimation);
        }

        private void Movement()
        {
            if (!_isDashing)
            {
                GroundCheck();
                var accelerate = false;
                
                if (Input.GetKey(_keys["Up"]) || Input.GetKey(_keys["Left"]) || Input.GetKey(_keys["Back"]) || Input.GetKey(_keys["Right"]))
                {
                    _axis = getDirection();
                    accelerate = true;

                }

                if (accelerate)
                {
                    _acceleration += (Time.deltaTime * 2);
                }
                else
                {
                    _acceleration -= (Time.deltaTime * 4);
                }
                
                _acceleration = Mathf.Clamp(_acceleration, 0, 1);

                _animations.SetParameter(ParameterNamesHelper.XParameter, _axis.y * _acceleration);
                _animations.SetParameter(ParameterNamesHelper.YParameter, _axis.x * _acceleration);

                _axis = new Vector2(_z, _x) * (_maxSpeed * _acceleration);

                var forward = new Vector3(-_camera.transform.right.z, 0, _camera.transform.right.x);
                var wishDirection = (forward * _axis.x + _camera.transform.right * _axis.y + Vector3.up * _rigidBody.velocity.y);
                _rigidBody.velocity = wishDirection;
            }
        }

        private void Crouching()
        {
            if (Input.GetKey(_keys["Duck"]))
            {
                _isCrouching = true;
            }
            else
            {
                _isCrouching = CalculateIfCanStand();
            }
            
            CalculatePercentCrouch();
            _animations.SetParameter(ParameterNamesHelper.IsCrouchingParameter, _isCrouching);
        }

        private Vector2 getDirection()
        {
            _x = 0;
            _z = 0;
            if (Input.GetKey(_keys["Up"]))
            {
                _z = 1;
            }
               
            if (Input.GetKey(_keys["Left"]))
            {
                _x = -1;
            }
               
            if (Input.GetKey(_keys["Back"]))
            {
                _z = -1;
            }
               
            if (Input.GetKey(_keys["Right"]))
            {
                _x = 1;
            }
            
            return new Vector2(_z, _x);
        }

        private void Dash()
        {
            if (Input.GetKeyDown(_keys["Dash"]) && !_isDashing && _dash > 0 && _maxDashes > 0 && !_isCrouching)
            {
                if (getDirection() != Vector2.zero)
                {
                    StartCoroutine(Dashing());
                }
            }
        }

        private IEnumerator Dashing()
        {
            _isDashing = true;
            _axis = getDirection() * 50;
            var forward = new Vector3(-_camera.transform.right.z, 0, _camera.transform.right.x);
            var wishDirection = (forward * _axis.x + _camera.transform.right * _axis.y);
            _rigidBody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            _dash--;
            _rigidBody.velocity = wishDirection;
            //UI Code

            yield return new WaitForSeconds(0.1f);

            _rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            _isDashing = false;
            StartCoroutine(RestoreDashPoint());
        }

        private IEnumerator RestoreDashPoint()
        {
            yield return new WaitForSeconds(dashRefreshTime);
            _dash++;
            //UI Code
        }

        private IEnumerator WalkSound()
        {
            yield return new WaitForSeconds(0.5f);
            
            if (_rigidBody.velocity != Vector3.zero && !_isCrouching && GroundCheck())
            {
                //Director Code
            }

            StartCoroutine(WalkSound());
        }

        public float CheckIsCrouching()
        {
            return _isCrouching ? 0.5f : 1;
        }

        public void Die()
        {
            _rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        }

        private bool GroundCheck()
        {
            bool isOnGround = Physics.Raycast(_rigidBody.transform.position, Vector3.down, 1 + 0.010f);
            _animations.SetParameter(ParameterNamesHelper.OnLandParameter, isOnGround);

            return isOnGround;
        }

        private bool CalculateIfCanStand()
        {
            if (_capsuleCollider.height < 2)
            {
                return Physics.Raycast(_capsuleCollider.bounds.max, Vector3.up, 2f - _capsuleCollider.height);
            }

            return false;
        }

        private void CalculatePercentCrouch()
        {
            var direction = _isCrouching ? 1 : -1;

            _percentCrouch += direction * Time.deltaTime * 4f;
            _percentCrouch = Mathf.Clamp01(_percentCrouch);

            //legs.transform.localPosition = Vector3.Lerp(legsStandingPosition, legsCrouchPosition, percentCrouch);
            //torso.transform.localPosition = Vector3.Lerp(fullBodyStanding, fullBodyCrouching, percentCrouch);
            _capsuleCollider.height = Mathf.Lerp(2, 1.5f, _percentCrouch);
            _capsuleCollider.center = Vector3.Lerp(Vector3.zero, _crouchCenter, _percentCrouch);
            _maxSpeed = Mathf.Lerp(_originalSpeed, _originalSpeed / 2, _percentCrouch);
        }
    }
}
