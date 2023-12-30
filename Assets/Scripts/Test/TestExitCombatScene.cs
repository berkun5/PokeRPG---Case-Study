using System;
using GameConfig.Enum;
using GameServices;
using GameServices.ServiceLocator;
using Logger;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public class TestExitCombatScene : MonoBehaviour
    {
        [SerializeField] private Button testCombatSceneButton;
        
        private void Start()
        {
            DevLog.Log("Starting: TestExitCombatScene");
            testCombatSceneButton.onClick.AddListener(()=> GameServiceLocator.GetService<PersistentServiceProvider>()
                .GetManager<LevelManager>().LoadScene(GameState.CharacterSelection));
        }

    }
}
