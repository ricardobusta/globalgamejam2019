namespace Game.Scripts
{
    using System;

    [Serializable]
    public class Cooldown
    {
        public float max;
        private float current=0;

        public bool Trigger()
        {
            if (current <= 0)
            {
                current = max;
                return true;
            }

            return false;
        }

        public void Update(float dt)
        {
            if (current > 0)
            {
                current -= dt;
            }
        }
    }
}