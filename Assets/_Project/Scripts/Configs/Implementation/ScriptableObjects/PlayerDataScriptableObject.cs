using _Project.Scripts.Configs.Base;
using UnityEngine;

namespace _Project.Scripts.Configs.Implementation.ScriptableObjects
{
    [CreateAssetMenu(menuName = "_Project/PlayerData", fileName = "PlayerData")]
    public class  PlayerDataScriptableObject : ScriptableObject, IPlayerData
    {
        [field: SerializeField] public int Health { get; private set; } = 100;
        [field: SerializeField] public int Damage { get; private set; } = 20;
        [field: SerializeField] public float AttackCooldown { get; private set; } = 0.5f;
        [field: SerializeField] public float MovementSpeed { get; private set; } = 20f;
        [field: SerializeField] public float AttackDistance { get; private set; } = 10f;
    }
}