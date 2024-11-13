using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Winter Universe/Item/Weapon/New Item")]
    public class WeaponItemConfig : ItemConfig
    {
        [Header("Weapon Information")]
        [SerializeField] private AnimatorOverrideController _controller;
        [SerializeField] private WeaponTypeConfig _weaponType;
        [Header("Local Transform Values")]
        [SerializeField] private Vector3 _localPosition;
        [SerializeField] private Quaternion _localRotation;
        [Header("Attack")]
        [SerializeField] private List<AudioClip> _attackClips = new();
        [SerializeField] private List<DamageType> _damageTypes = new();
        [SerializeField] private List<EffectCreator> _ownerEffects = new();
        [SerializeField] private List<EffectCreator> _targetEffects = new();
        [Header("Modifiers")]
        [SerializeField] private List<StatModifierCreator> _modifiers = new();

        public List<StatModifierCreator> Modifiers => _modifiers;
        public AnimatorOverrideController Controller => _controller;
        public WeaponTypeConfig WeaponType => _weaponType;
        public Vector3 LocalPosition => _localPosition;
        public Quaternion LocalRotation => _localRotation;
        public List<AudioClip> AttackClips => _attackClips;
        public List<DamageType> DamageTypes => _damageTypes;
        public List<EffectCreator> OwnerEffects => _ownerEffects;
        public List<EffectCreator> TargetEffects => _targetEffects;

        private void OnValidate()
        {
            _itemType = ItemType.Weapon;
        }

        public override void Use(PawnController character, bool fromInventory = true)// TODO проверять CanUse перед этим методом
        {
            character.PawnEquipment.EquipWeapon(this, fromInventory);
        }
    }
}