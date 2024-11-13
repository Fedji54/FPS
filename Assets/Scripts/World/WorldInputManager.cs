using UnityEngine;
using UnityEngine.InputSystem;

namespace WinterUniverse
{
    public class WorldInputManager : MonoBehaviour
    {
        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private bool _runInput;

        public Vector2 MoveInput => _moveInput;
        public Vector2 LookInput => _lookInput;
        public bool RunInput => _runInput;

        public void OnMove(InputValue value) => _moveInput = value.Get<Vector2>();
        public void OnLook(InputValue value) => _lookInput = value.Get<Vector2>();
        public void OnRun(InputValue value) => _runInput = value.isPressed;
        public void OnJump() => WorldManager.StaticInstance.PlayerManager.PawnLocomotion.TryPerformJump();
    }
}