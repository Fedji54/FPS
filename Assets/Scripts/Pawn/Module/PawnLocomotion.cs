using UnityEngine;

namespace WinterUniverse
{
    [RequireComponent(typeof(CharacterController))]
    public class PawnLocomotion : MonoBehaviour
    {
        private CharacterController _cc;
        private PawnController _pawn;
        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private Vector3 _moveVelocity;
        private Vector3 _fallVelocity;
        private Vector3 _knockbackVelocity;
        private float _forwardVelocity;
        private float _rightVelocity;
        private float _lookAngle;
        private float _jumpTimer;
        private float _groundedTimer;
        private RaycastHit _groundHit;

        [SerializeField] private float _maxLookAngle = 85f;
        [SerializeField] private float _timeToJump = 0.25f;
        [SerializeField] private float _timeToFall = 0.25f;

        private bool CanJump => _jumpTimer > 0f && _groundedTimer > 0f && !_pawn.IsDead && !_pawn.IsPerfomingAction && _pawn.PawnStats.EnergyCurrent >= _pawn.PawnStats.JumpEnergyCost.CurrentValue;
        public Vector3 MoveVelocity => _moveVelocity;
        public CharacterController CC => _cc;

        public void Initialize(PawnController pawn)
        {
            _pawn = pawn;
            _cc = GetComponent<CharacterController>();
            _cc.height = _pawn.PawnAnimator.Height;
            _cc.radius = _pawn.PawnAnimator.Radius;
            _cc.center = _pawn.PawnAnimator.Height * 0.5f * Vector3.up;
        }

        public void HandleLocomotion()
        {
            if (_pawn.IsDead && _pawn.IsGrounded)
            {
                return;
            }
            _moveInput = _pawn.GetMoveInput();
            _lookInput = _pawn.GetLookInput();
            HandleGravity();
            HandleMovement();
            HandleRotation();
            HandleKnockback();
            if (_moveVelocity != Vector3.zero)
            {
                _forwardVelocity = Vector3.Dot(_moveVelocity, transform.forward);
                _rightVelocity = Vector3.Dot(_moveVelocity, transform.right);
                _pawn.IsMoving = true;
            }
            else
            {
                _forwardVelocity = 0f;
                _rightVelocity = 0f;
                _pawn.IsMoving = false;
            }
            _pawn.PawnAnimator.UpdateAnimatorMovement(_rightVelocity, _forwardVelocity, _pawn.PawnStats.MoveSpeed.CurrentValue);
        }

        private void HandleGravity()
        {
            if (CanJump)
            {
                _jumpTimer = 0f;
                _groundedTimer = 0f;
                ApplyJumpForce();
            }
            _pawn.IsGrounded = _fallVelocity.y <= 0.1f && Physics.SphereCast(transform.position + _cc.center, _cc.radius, Vector3.down, out _groundHit, _cc.center.y - (_cc.radius / 2f), WorldManager.StaticInstance.LayerManager.ObstacleMask);
            if (_pawn.IsGrounded)
            {
                _groundedTimer = _timeToFall;
                _fallVelocity.y = WorldManager.StaticInstance.DataManager.Gravity / 5f;
            }
            else
            {
                _groundedTimer -= Time.deltaTime;
                _fallVelocity.y += WorldManager.StaticInstance.DataManager.Gravity * Time.deltaTime;
            }
            _jumpTimer -= Time.deltaTime;
            _cc.Move(_fallVelocity * Time.deltaTime);
        }

        private void HandleMovement()
        {
            if (!_pawn.CanMove)
            {
                _moveVelocity = Vector3.zero;
                return;
            }
            if (_moveInput != Vector2.zero)
            {
                if (_pawn.IsRunning)
                {
                    _moveInput *= 2f;
                }
                _moveVelocity = Vector3.MoveTowards(_moveVelocity, (transform.right * _moveInput.x + transform.forward * _moveInput.y) * _pawn.PawnStats.MoveSpeed.CurrentValue, _pawn.PawnStats.Acceleration.CurrentValue * Time.deltaTime);
            }
            else
            {
                _moveVelocity = Vector3.MoveTowards(_moveVelocity, Vector3.zero, _pawn.PawnStats.Deceleration.CurrentValue * Time.deltaTime);
            }
            _cc.Move(_moveVelocity * Time.deltaTime);
        }

        private void HandleRotation()
        {
            if (!_pawn.CanRotate)
            {
                return;
            }
            if (_lookInput.x != 0f)
            {
                transform.Rotate(_lookInput.x * _pawn.PawnStats.RotateSpeed.CurrentValue * Time.deltaTime * Vector3.up);
            }
            if (_lookInput.y != 0f)
            {
                _lookAngle = Mathf.Clamp(_lookAngle - (_lookInput.y * _pawn.PawnStats.RotateSpeed.CurrentValue * Time.deltaTime), -_maxLookAngle, _maxLookAngle);
            }
            _pawn.PawnAnimator.HeadPoint.rotation = Quaternion.Euler(Vector3.right * _lookAngle);
        }

        private void HandleKnockback()
        {
            if (_knockbackVelocity != Vector3.zero)
            {
                _knockbackVelocity = Vector3.MoveTowards(_knockbackVelocity, Vector3.zero, Time.deltaTime);
                _cc.Move(_knockbackVelocity * Time.deltaTime);
            }
        }

        public bool HandleRunning()
        {
            if (_pawn.IsDead || _pawn.IsPerfomingAction || _pawn.PawnStats.EnergyCurrent <= _pawn.PawnStats.RunEnergyCost.CurrentValue || _moveInput.magnitude < 0.5f)
            {
                return false;
            }
            _pawn.PawnStats.ReduceCurrentEnergy(_pawn.PawnStats.RunEnergyCost.CurrentValue * Time.deltaTime);
            return true;
        }

        public void TryPerformJump()
        {
            _jumpTimer = _timeToJump;
        }

        private void ApplyJumpForce()
        {
            _fallVelocity.y = Mathf.Sqrt(_pawn.PawnStats.JumpForce.CurrentValue * -2f * WorldManager.StaticInstance.DataManager.Gravity);
            _pawn.PawnStats.ReduceCurrentEnergy(_pawn.PawnStats.JumpEnergyCost.CurrentValue);
        }

        public void Knockback(Vector3 direction, float force)
        {
            _knockbackVelocity += direction.normalized * force;
        }
    }
}