using UnityEngine;
using UnityEngine.UI;

namespace Varez.UI
{
    [RequireComponent(typeof(Image))]
    public class UISchemeChangeHandler : MonoBehaviour
    {
        public InputActionEnum uiKey;

        private GameManager _manager;
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            _manager = GameManager.Instance;
            _image.sprite = _manager.gameUIScriptable.uIList[0].icons.actionSprites[(int)uiKey].sprite;
            _manager.UIEvents.OnSchemeChange += ChangeSprite;
        }

        private void ChangeSprite(string schemeChangeTo)
        {
            _image.sprite = schemeChangeTo switch
            {
                "PC" => CheckIfNull(_manager.gameUIScriptable.uIList[0].icons.actionSprites[(int)uiKey].sprite),
                "Gamepad" => CheckIfNull(_manager.gameUIScriptable.uIList[1].icons.actionSprites[(int)uiKey].sprite),
                _ => null
            };
        }

        private Sprite CheckIfNull(Sprite spriteChange)
        {
            if (spriteChange) return spriteChange;
            
            Debug.Log($"There is no icon for {uiKey.ToString()} action in current Scheme");
            return null;

        }
        
        
    }
}
