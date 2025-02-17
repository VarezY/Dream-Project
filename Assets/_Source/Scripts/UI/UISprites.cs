using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Varez.UI
{
    [CreateAssetMenu(fileName = "UISprites", menuName = "Scriptable/UISprites", order = 0)]
    public class UISprites : ScriptableObject
    {
        public GameSprite[] actionSprites;
        
        private void OnValidate()
        {
            // Ensure we have an entry for each enum value
            List<InputActionEnum> enumValues = System.Enum.GetValues(typeof(InputActionEnum))
                                  .Cast<InputActionEnum>()
                                  .ToList();

            // If array is null or empty, initialize it
            if (actionSprites == null || actionSprites.Length == 0)
            {
                actionSprites = new GameSprite[enumValues.Count];
                for (int i = 0; i < enumValues.Count; i++)
                {
                    actionSprites[i] = new GameSprite { actionEnumName = enumValues[i] };
                }
                return;
            }

            // Create a list to store existing values
            List<GameSprite> updatedSchemes = new List<GameSprite>();

            // First, add all existing valid entries
            foreach (var scheme in actionSprites)
            {
                if (enumValues.Contains(scheme.actionEnumName) && 
                    !updatedSchemes.Any(x => x.actionEnumName == scheme.actionEnumName))
                {
                    updatedSchemes.Add(scheme);
                }
            }

            // Then add missing enum values
            foreach (InputActionEnum enumValue in enumValues.Where(enumValue => !updatedSchemes.Any(x => x.actionEnumName == enumValue)))
            {
                updatedSchemes.Add(new GameSprite { actionEnumName = enumValue });
            }

            // Sort by enum order
            actionSprites = updatedSchemes
                .OrderBy(x => x.actionEnumName)
                .ToArray();
        }

        // Utility method to get GameList by difficulty
        public GameSprite GetGameList(InputActionEnum difficulty)
        {
            return actionSprites.FirstOrDefault(x => x.actionEnumName == difficulty);
        }

        // Utility method to get sprite by difficulty
        public Sprite GetSprite(InputActionEnum difficulty)
        {
            return GetGameList(difficulty)?.sprite;
        }

        // Get all schemes
        public GameSprite[] GetAllSchemes()
        {
            return actionSprites;
        }
    }

    [Serializable]
    public class GameSprite
    {
        [FormerlySerializedAs("actionName")] public InputActionEnum actionEnumName;
        public Sprite sprite;
    }
}