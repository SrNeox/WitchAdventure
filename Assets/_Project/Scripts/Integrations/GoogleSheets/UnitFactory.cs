using _Project.Scripts.GoogleSheets;
using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    [SerializeField] private UnitDatabase _database;
    [SerializeField] private GameObject _unitPrefab; // префаб с компонентом UnitStats

    public GameObject Spawn(string id, Vector3 position)
    {
        if (_database == null)
        {
            Debug.LogError("UnitDatabase not assigned.");
            return null;
        }

        if (!_database.TryGetUnit(id, out var data))
        {
            Debug.LogError($"Unit with id '{id}' not found.");
            return null;
        }

        var go = Instantiate(_unitPrefab, position, Quaternion.identity);
        var stats = go.GetComponent<UnitStats>();
        if (stats != null)
        {
            stats.ApplyData(data);
        }
        else
        {
            Debug.LogWarning("Unit prefab has no UnitStats component.");
        }

        return go;
    }
}