using UnityEngine;

namespace WinterUniverse
{
    public class WorldSaveLoadManager : MonoBehaviour
    {
        [SerializeField] private string _fileName = "SaveData";
        public PawnSaveData CurrentSaveData;

        public void SaveGame()
        {
            WorldManager.StaticInstance.PlayerManager.SaveData(ref CurrentSaveData);
            DataWriter.CreateSaveFile(CurrentSaveData, _fileName);
        }

        public void LoadGame()
        {
            if (DataWriter.FileExists(_fileName))
            {
                CurrentSaveData = DataWriter.LoadSavedFile(_fileName);
            }
            else
            {
                CurrentSaveData = new();
                WorldManager.StaticInstance.PlayerManager.CreateCharacter(CurrentSaveData);
                SaveGame();
            }
            WorldManager.StaticInstance.PlayerManager.LoadData(CurrentSaveData);
            WorldManager.StaticInstance.MapManager.LoadMap(CurrentSaveData.MapName);
        }

        public void DeleteGame()
        {
            DataWriter.DeleteSavedFile(_fileName);
        }
    }
}