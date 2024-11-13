using Lean.Pool;
using UnityEngine;

namespace WinterUniverse
{
    public class WeaponSlot : MonoBehaviour
    {
        private WeaponItemConfig _config;
        private PawnController _pawn;
        private GameObject _model;

        public PawnController Pawn => _pawn;
        public WeaponItemConfig Config => _config;

        public void Initialize(PawnController pawn)
        {
            _pawn = pawn;
            foreach (StatModifierCreator creator in _config.Modifiers)
            {
                _pawn.PawnStats.AddStatModifier(creator);
            }
            _model = LeanPool.Spawn(_config.Model, transform);
            _model.transform.SetLocalPositionAndRotation(_config.LocalPosition, _config.LocalRotation);
        }

        public void Equip(WeaponItemConfig weapon)
        {
            if (weapon == null)
            {
                return;
            }
            foreach (StatModifierCreator creator in _config.Modifiers)
            {
                _pawn.PawnStats.RemoveStatModifier(creator);
            }
            LeanPool.Despawn(_model);
            _config = weapon;
            foreach (StatModifierCreator creator in _config.Modifiers)
            {
                _pawn.PawnStats.AddStatModifier(creator);
            }
            _model = LeanPool.Spawn(_config.Model, transform);
            _model.transform.SetLocalPositionAndRotation(_config.LocalPosition, _config.LocalRotation);
        }
    }
}