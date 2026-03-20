using _Project.Scripts.Configs.Base;
using UnityEngine;

namespace _Project.Scripts.Configs.Implementation.ScriptableObjects
{
    [CreateAssetMenu(menuName = "_Project/GameConfig", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject, IGameConfig
    {
        [field: SerializeField] public int MaxPlayerCountInSession { get; private set; }
    }
}
