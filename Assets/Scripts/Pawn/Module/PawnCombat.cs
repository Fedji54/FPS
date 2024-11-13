using System.Collections;
using UnityEngine;

namespace WinterUniverse
{
    public class PawnCombat : MonoBehaviour
    {
        private PawnController _pawn;

        [HideInInspector] public PawnController CurrentTarget;

        [HideInInspector] public float DistanceToTarget;
        [HideInInspector] public float AngleToTarget;

        public void Initialize(PawnController pawn)
        {
            _pawn = pawn;
        }

        public void HandleTargeting()
        {
            if (CurrentTarget != null && !_pawn.IsDead)
            {
                if (!CurrentTarget.IsDead)
                {
                    DistanceToTarget = Vector3.Distance(transform.position, CurrentTarget.transform.position);
                    AngleToTarget = ExtraTools.GetSignedAngleToDirection(transform.forward, CurrentTarget.transform.position - transform.position);
                }
                else
                {
                    SetTarget();
                }
            }
        }

        public void SetTarget(PawnController newTarget = null)
        {
            if (newTarget != null)
            {
                CurrentTarget = newTarget;
            }
            else
            {
                CurrentTarget = null;
                DistanceToTarget = float.MaxValue;
            }
        }

        public bool CurrentTargetInViewAngle()
        {
            return TargetInViewAngle(CurrentTarget);
        }

        public bool TargetInViewAngle(PawnController target)
        {
            return Vector3.Angle(_pawn.PawnAnimator.HeadPoint.forward, (target.PawnAnimator.BodyPoint.position - _pawn.PawnAnimator.HeadPoint.position).normalized) <= _pawn.PawnStats.ViewAngle.CurrentValue / 2f;// TODO
        }

        public bool CurrentTargetBlockedByObstacle()
        {
            return TargetBlockedByObstacle(CurrentTarget);
        }

        public bool TargetBlockedByObstacle(PawnController target)
        {
            return Physics.Linecast(_pawn.PawnAnimator.HeadPoint.position, target.PawnAnimator.BodyPoint.position, WorldManager.StaticInstance.LayerManager.ObstacleMask);
        }

        public bool CurrentTargetIsVisible()
        {
            return TargetIsVisible(CurrentTarget);
        }

        public bool TargetIsVisible(PawnController target)
        {
            return TargetInViewAngle(target) && !TargetBlockedByObstacle(target);
        }
    }
}