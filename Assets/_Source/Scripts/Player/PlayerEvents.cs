using System;

namespace Varez.Player
{
    public struct PlayerEvents
    {
        public Action OnPlayerStanding;

        public Action<bool> OnPlayerWalking; //isWalking
        public Action<bool> OnPlayerSprinting; //isSprinting
    }
}