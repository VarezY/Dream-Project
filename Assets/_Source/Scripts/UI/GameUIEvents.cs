using System;

namespace Varez.UI
{
    public struct GameUIEvents
    {
        // Scheme, spriteKey
        public Action<string> OnSchemeChange;
        
        // In-Game Events
        public Action<bool> OnAbleInteract;
        public Action OnNotificationStart;
        public Action OnNotificationEnd;
        
        // Dialogue
        public Action<string, string> OnPlayDialogue;
    }
}