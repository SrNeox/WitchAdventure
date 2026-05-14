using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GoogleSheets
{
    public class UnitDatabase : MonoBehaviour
    {
        [SerializeField] private SpreadSheetsContainerAsset _sheetsContainer;
        
        private Dictionary<string, UnitData> _byId;

        private void Awake()
        {
            BuildIndex();
        }

        private void BuildIndex()
        {
            _byId = new Dictionary<string, UnitData>();
            
            if (_sheetsContainer == null || _sheetsContainer.Content == null)
            {
                Debug.LogWarning("SpreadSheetsContainer not assigned or empty.");
                return;
            }

            foreach (var unitData in _sheetsContainer.Content.Units)
            {
                if (string.IsNullOrWhiteSpace(unitData.id)) 
                    continue;
                
                if (_byId.ContainsKey(unitData.id))
                {
                    Debug.LogWarning($"Duplicate unit id: {unitData.id}");
                    continue;
                }
                
                _byId.Add(unitData.id, unitData);
            }
        }

        public bool TryGetUnit(string id, out UnitData data)
        {
            if (_byId == null) 
                BuildIndex();
            
            data = null;
            
            return _byId != null && _byId.TryGetValue(id, out data);
        }
    }
}
