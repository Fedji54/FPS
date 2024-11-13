using UnityEngine;

namespace WinterUniverse
{
    public class WorldLayerManager : MonoBehaviour
    {
        [SerializeField] private LayerMask _pawnMask;
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private LayerMask _interactableMask;

        public LayerMask PawnMask => _pawnMask;
        public LayerMask ObstacleMask => _obstacleMask;
        public LayerMask InteractableMask => _interactableMask;
    }
}