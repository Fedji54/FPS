using UnityEngine;
using UnityEngine.AI;

namespace WinterUniverse
{
    public class AIController : PawnController
    {
        private ALIFEMode _ALIFEMode;

        private NavMeshAgent _agent;
        private AIDetectionModule _aiDetectionModule;
        private bool _reachedDestination;

        public NavMeshAgent Agent => _agent;
        public AIDetectionModule AIDetectionModule => _aiDetectionModule;
        public bool ReachedDestination => _reachedDestination;
        public ALIFEMode ALIFEMode => _ALIFEMode;

        public override Vector2 GetMoveInput()
        {
            if (!_reachedDestination)
            {
                return new Vector2(Vector3.Dot(_agent.desiredVelocity, transform.right), Vector3.Dot(_agent.desiredVelocity, transform.forward)).normalized;
            }
            return Vector2.zero;
        }

        public override Vector2 GetLookInput()
        {
            //if (PawnCombat.CurrentTarget != null && PawnCombat.CurrentTargetIsVisible())
            //{
            //    return (PawnCombat.CurrentTarget.transform.position - transform.position).normalized;
            //}
            //else if (!_reachedDestination)
            //{
            //    return _agent.desiredVelocity;
            //}
            //else
            //{
            //}
            return Vector2.zero;
        }

        public override void CreateCharacter(PawnSaveData data)
        {
            _agent = GetComponentInChildren<NavMeshAgent>();
            _aiDetectionModule = GetComponent<AIDetectionModule>();
            _agent.updateRotation = false;
            _agent.height = _pawnAnimator.Height;
            _agent.radius = _pawnAnimator.Radius;
            base.CreateCharacter(data);
        }

        protected override void Update()
        {
            base.Update();
            if (_ALIFEMode == ALIFEMode.Simple)
            {
                // other stuff
                if (WorldManager.StaticInstance.PlayerManager != null && Vector3.Distance(transform.position, WorldManager.StaticInstance.PlayerManager.transform.position) < 250f)
                {
                    _ALIFEMode = ALIFEMode.Advanced;
                    // set values aka rendering/animation to advanced
                }
            }
            else if (_ALIFEMode == ALIFEMode.Advanced)
            {
                // other stuff
                if (WorldManager.StaticInstance.PlayerManager == null || Vector3.Distance(transform.position, WorldManager.StaticInstance.PlayerManager.transform.position) > 250f)
                {
                    _ALIFEMode = ALIFEMode.Simple;
                    // set values aka rendering/animation to simple
                }
            }
        }

        public void StopMovement()
        {
            _reachedDestination = true;
            _agent.ResetPath();
        }
    }
}