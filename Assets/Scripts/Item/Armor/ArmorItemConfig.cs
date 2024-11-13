using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Armor Item", menuName = "Winter Universe/Item/Armor/New Item")]
    public class ArmorItemConfig : ItemConfig
    {
        [Header("Armor Information")]
        [SerializeField] private ArmorTypeConfig _armorType;
        [Header("Modifiers")]
        [SerializeField] private List<StatModifierCreator> _modifiers = new();

        public ArmorTypeConfig ArmorType => _armorType;
        public List<StatModifierCreator> Modifiers => _modifiers;

        private void OnValidate()
        {
            _itemType = ItemType.Armor;
        }

        public override void Use(PawnController character, bool fromInventory = true)
        {
            character.PawnEquipment.EquipArmor(this, fromInventory);
        }
    }
}