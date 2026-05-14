using _Project.Scripts.GoogleSheets;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnitStats : MonoBehaviour
{
    public string UnitId { get; private set; }
    public int Health { get; private set; }
    public float MoveSpeed { get; private set; }

    // Применяем данные из UnitData
    public void ApplyData(UnitData data)
    {
        UnitId = data.id;
        Health = data.health;
        MoveSpeed = data.moveSpeed;

        // Пример: применить к Rigidbody или другим компонентам
        // var rb = GetComponent<Rigidbody>();
        // rb.mass = Mathf.Clamp(MoveSpeed / 5f, 0.5f, 5f);

        // Лог для отладки
        Debug.Log($"Applied data to {name}: {data}");
    }
}