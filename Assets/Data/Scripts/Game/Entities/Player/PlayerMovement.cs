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
        [SerializeField] private GameObject _model;

        private PlayerThirdPersonAnimations _animations;
        private PlayerUIHandler _uIHandler;
        private Dictionary<string, KeyCode> _keys;

        private Rigidbody _rigidBody;
        private Camera _camera;
        private CapsuleCollider _capsuleCollider;

        private float _sensitivity;
        //Walking
        private float _acceleration;
        private Vector3 _axis;

        //Dashing
        private int _dash;
        private bool _isDashing;
        
        [SerializeField] private float dashRefreshTime = 1f;
        [SerializeField] private AudioClip dashClip;
        [SerializeField] private AudioClip dashRestoreClip;

        private bool _isCrouching;

        //Jump/DoubleJump
        
        private bool _isAbleToDoubleJump;

        private Vector3 _crouchCenter = new Vector3(0, 0.33f, 0);
        private Vector3 _modelCrouchingPos = new Vector3(0, -0.42f, 0);
        private Vector3 _modelStandingPos = new Vector3(0,-1f, 0);
        private float _percentCrouch;

        private void Start()
        {
            _camera = Camera.main;
            _rigidBody = GetComponent<Rigidbody>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _percentCrouch = 0;

            StartCoroutine(WalkSound());
        }
        public void SetConfig(PlayerThirdPersonAnimations animationReference, PlayerUIHandler uiHandler, Dictionary<string, KeyCode> keys, float sensivity)
        {
            _animations = animationReference;
            _keys = keys;
            _sensitivity = sensivity;
            _uIHandler = uiHandler;

            _dash = _maxDashes;
        }
        
        private void Update()
        {
            Look();
        }

        private void FixedUpdate()
        {
            Movement();
            JumpMovement();
            Dash();
            Crouching();
        }

        private void Look()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            var groundPlane = new Plane(Vector3.up, _model.transform.position);

            float rayDistance;
            if (groundPlane.Raycast(ray, out rayDistance))
            {
                var lookPoint = ray.GetPoint(rayDistance);
                var direction = (lookPoint - _model.transform.position).normalized;

                // Calcula el ángulo y aplica la sensibilidad
                var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                var currentAngle = _model.transform.eulerAngles.y;
                var angle = Mathf.LerpAngle(currentAngle, targetAngle, _sensitivity * Time.deltaTime);

                _model.transform.rotation = Quaternion.Euler(0, angle, 0);
            }
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

                Vector3 localInputDir = _model.transform.InverseTransformDirection(_axis.normalized);

                _animations.SetParameter(ParameterNamesHelper.XParameter, localInputDir.x * _acceleration);
                _animations.SetParameter(ParameterNamesHelper.YParameter, localInputDir.z * _acceleration);

                _axis = _axis.normalized * (CalculateMaxSpeed());
                _axis.y = _rigidBody.velocity.y;

                _rigidBody.velocity = _axis;
            }
        }

        private void Crouching()
        {
            if (Input.GetKeyDown(_keys["Duck"]))
            {
                _isCrouching = !_isCrouching;
                
                if(!_isCrouching)
                {
                    _isCrouching = CalculateIfCanStand();
                }
            }
            
            CalculatePercentCrouch();
            _animations.SetParameter(ParameterNamesHelper.IsCrouchingParameter, _isCrouching);
        }

        private Vector3 getDirection()
        {
            var vector = Vector3.zero;
            if (Input.GetKey(_keys["Up"]))
            {
                vector += Vector3.right;
            }
               
            if (Input.GetKey(_keys["Left"]))
            {
                vector += Vector3.forward;
            }
               
            if (Input.GetKey(_keys["Back"]))
            {
                vector += Vector3.left;
            }
               
            if (Input.GetKey(_keys["Right"]))
            {
                vector += Vector3.back;
            }

            return vector;
        }

        private void Dash()
        {
            if (Input.GetKeyDown(_keys["Dash"]) && !_isDashing && _dash > 0 && _maxDashes > 0 && !_isCrouching)
            {
                var direction = getDirection();
                if (direction != Vector3.zero)
                {
                    StartCoroutine(Dashing(direction.normalized));
                }
            }
        }

        private IEnumerator Dashing(Vector3 direction)
        {
            _isDashing = true;
            _animations.PlayAnimation(AnimationNamesHelper.DashAnimation);
            direction *= 50;
            _rigidBody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            _dash--;
            _rigidBody.velocity = direction;
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
            _capsuleCollider.enabled = false;
            enabled = false;
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

            _capsuleCollider.height = Mathf.Lerp(1.95f, 1.5f, _percentCrouch);
            _capsuleCollider.center = Vector3.Lerp(Vector3.zero, _crouchCenter, _percentCrouch);
            _model.transform.localPosition = Vector3.Lerp(_modelStandingPos, _modelCrouchingPos, _percentCrouch);
        }

        private float CalculateMaxSpeed()
        {
            return _maxSpeed * _acceleration * (_isCrouching ? 0.5f : 1);
        }
    }
}
