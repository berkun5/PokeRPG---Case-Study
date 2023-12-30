using System.Collections;
using System.Collections.Generic;
using GameConfig.Enum;
using GameServices;
using GameServices.ServiceLocator;
using Logger;
using Managers;
using UnityEngine;

namespace Test
{
    public class TestCharacterSelectionDataStates : MonoBehaviour
    {
        public List<CharacterId> testSelectedCharacters;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(3);
            testSelectedCharacters = new List<CharacterId>(GameServiceLocator.GetService<PersistentServiceProvider>()
                .GetManager<SettingsManager>().RemoteData.CharacterData.GetSelectedCharacters());

            if (testSelectedCharacters.Count > 3)
            {
                DevLog.LogError("TEST:: Selected characters are more than 3!");
            }
            else
            {
                DevLog.Log("TEST:: Selected characters are less or equal to 3.");
            }
        }
    }
}
