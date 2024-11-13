using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WinterUniverse
{
    public class AIDetectionModule : MonoBehaviour
    {
        private AIController _ai;
        private List<PawnController> _visibleEnemies = new();
        private List<PawnController> _visibleNeutrals = new();
        private List<PawnController> _visibleAllies = new();

        private void Awake()
        {
            _ai = GetComponent<AIController>();
        }

        public void FindTargetInViewRange()
        {
            if (_ai.PawnCombat.CurrentTarget != null)
            {
                return;
            }
            _visibleEnemies.Clear();
            _visibleNeutrals.Clear();
            _visibleAllies.Clear();
            Collider[] colliders = Physics.OverlapSphere(_ai.PawnAnimator.HeadPoint.position, _ai.PawnStats.ViewDistance.CurrentValue, WorldManager.StaticInstance.LayerManager.PawnMask);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out PawnController character) && character != _ai && !character.IsDead)
                {
                    if (Vector3.Distance(transform.position, character.transform.position) <= _ai.PawnStats.HearRadius.CurrentValue || _ai.PawnCombat.TargetIsVisible(character))
                    {
                        switch (_ai.Faction.GetState(character.Faction))
                        {
                            case RelationshipState.Enemy:
                                _visibleEnemies.Add(character);
                                break;
                            case RelationshipState.Neutral:
                                _visibleNeutrals.Add(character);
                                break;
                            case RelationshipState.Ally:
                                _visibleAllies.Add(character);
                                break;
                        }
                    }
                }
            }
            if (_visibleEnemies.Count > 0)
            {
                _ai.PawnCombat.SetTarget(GetClosestEnemy());
            }
        }

        public PawnController GetClosestEnemy()
        {
            return _visibleEnemies.OrderBy(target => Vector3.Distance(target.transform.position, transform.position)).FirstOrDefault();
        }
    }
}