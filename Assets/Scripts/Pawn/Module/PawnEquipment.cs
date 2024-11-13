using System;
using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public class PawnEquipment : MonoBehaviour
    {
        public Action OnEquipmentChanged;

        private PawnController _pawn;

        [SerializeField] private WeaponSlot _weaponSlot;
        [SerializeField] private List<ArmorSlot> _armorSlots = new();

        public WeaponSlot WeaponSlot => _weaponSlot;

        public void Initialize(PawnController pawn)
        {
            _pawn = pawn;
            _weaponSlot.Initialize(_pawn);
            foreach (ArmorSlot slot in _armorSlots)
            {
                slot.Initialize(_pawn);
            }
        }

        public void EquipWeapon(WeaponItemConfig weapon, bool removeNewFromInventory = true, bool addOldToInventory = true)
        {
            if (weapon == null || _pawn.IsDead)
            {
                return;
            }
            if (removeNewFromInventory)
            {
                _pawn.PawnInventory.RemoveItem(weapon);
            }
            if (addOldToInventory)
            {
                _pawn.PawnInventory.AddItem(_weaponSlot.Config);
            }
            _weaponSlot.Equip(weapon);
            _pawn.PawnAnimator.PlayActionAnimation($"Swap Weapon", true);
            OnEquipmentChanged?.Invoke();
        }

        public void EquipArmor(ArmorItemConfig armor, bool removeFromInventory = true, bool addOldToInventory = true)
        {
            if (armor == null || _pawn.IsDead)
            {
                return;
            }
            foreach (ArmorSlot slot in _armorSlots)
            {
                if (slot.Type == armor.ArmorType)
                {
                    if (removeFromInventory)
                    {
                        _pawn.PawnInventory.RemoveItem(armor);
                    }
                    if (addOldToInventory)
                    {
                        _pawn.PawnInventory.AddItem(slot.Data);
                    }
                    slot.Equip(armor);
                    OnEquipmentChanged?.Invoke();
                    break;
                }
            }
        }

        public void EquipArmor(ArmorItemConfig armor, ArmorSlot slot, bool removeFromInventory = true, bool addOldToInventory = true)// for drag and drop
        {
            if (armor == null || slot == null || armor.ArmorType != slot.Type || _pawn.IsDead)
            {
                return;
            }
            if (removeFromInventory)
            {
                _pawn.PawnInventory.RemoveItem(armor);
            }
            if (addOldToInventory)
            {
                _pawn.PawnInventory.AddItem(slot.Data);
            }
            slot.Equip(armor);
            OnEquipmentChanged?.Invoke();
        }

        public void ForceUpdateMeshes()
        {
            foreach (ArmorSlot slot in _armorSlots)
            {
                slot.ForceUpdateMeshes();
            }
        }

        public void EquipBestItems()
        {
            if (_pawn.PawnInventory.GetBestWeapon(out WeaponItemConfig weapon) && weapon.Price > _weaponSlot.Config.Price)
            {
                EquipWeapon(weapon);
            }
            foreach (ArmorSlot slot in _armorSlots)
            {
                if (_pawn.PawnInventory.GetBestArmor(slot.Type, out ArmorItemConfig armor) && armor.Price > slot.Data.Price)
                {
                    EquipArmor(armor, slot);
                }
            }
        }
    }
}