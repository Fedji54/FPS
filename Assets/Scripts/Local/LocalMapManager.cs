using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public class LocalMapManager : MonoBehaviour
    {
        [SerializeField] private List<MapEntryPoint> _mapEntryPoints = new();

        public Transform GetEntryPoint(MapConfig oldMap)
        {
            if (oldMap == null)
            {
                return transform;
            }
            foreach (MapEntryPoint mep in _mapEntryPoints)
            {
                if (mep.Map == oldMap)
                {
                    return mep.EntryPoint;
                }
            }
            Debug.LogError($"Not finded entry from {oldMap.DisplayName}!");
            return transform;
        }
    }
}