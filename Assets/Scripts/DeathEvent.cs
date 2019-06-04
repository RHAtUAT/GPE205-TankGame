using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class DeathEvent
    {
        public class DeathEventArgs : EventArgs
        {
            public GameObject pawn { get; set; }
        }


        public event EventHandler<DeathEventArgs> Died;

        protected virtual void OnDeath(GameObject gameObject)
        {
            if (Died != null)
                Died(this, new DeathEventArgs() { pawn = gameObject });
        }
    }
}
