using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ActiveItemData", menuName = "Custom/ActiveItemData", order = 51)]
[System.Serializable]
public class ActiveItemSO : ScriptableObject
{
    [SerializeField] public List<ActiveChar> activeItemData = new List<ActiveChar>();
}
[System.Serializable]
public class ActiveChar
{
    public float damage = 0;
    public float frequency = 0;
    public float rotationSpeed = 0;
    public float slowEffect = 0;
    public float flySpeed = 0;
}
