using UnityEngine;

namespace WinterUniverse
{
    public class PlayerController : PawnController
    {
        public override Vector2 GetMoveInput()
        {
            return WorldManager.StaticInstance.InputManager.MoveInput;
        }

        public override Vector2 GetLookInput()
        {
            return WorldManager.StaticInstance.InputManager.LookInput;
        }

        public void ToggleCursorVisible(bool enabled)
        {
            if (enabled)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }

        public void SaveData(ref PawnSaveData data)
        {
            data.CharacterName = _characterName;
            data.Faction = _faction.DisplayName;
            data.Health = _pawnStats.HealthCurrent;
            data.Energy = _pawnStats.EnergyCurrent;
            data.InventoryStacks.Clear();
            foreach (ItemStack stack in _pawnInventory.Stacks)
            {
                if (data.InventoryStacks.ContainsKey(stack.Item.DisplayName))
                {
                    data.InventoryStacks[stack.Item.DisplayName] += stack.Amount;
                }
                else
                {
                    data.InventoryStacks.Add(stack.Item.DisplayName, stack.Amount);
                }
            }
            data.Weapon = _pawnEquipment.WeaponSlot.Config.DisplayName;
            // save armors
            data.Transform.SetPositionAndRotation(transform.position, transform.eulerAngles);
        }

        public void LoadData(PawnSaveData data)
        {
            CreateCharacter(data);
            _pawnStats.HealthCurrent = data.Health;
            _pawnStats.EnergyCurrent = data.Energy;
            transform.SetPositionAndRotation(data.Transform.GetPosition(), data.Transform.GetRotation());
        }
    }
}