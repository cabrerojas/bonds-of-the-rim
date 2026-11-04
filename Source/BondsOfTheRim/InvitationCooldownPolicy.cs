namespace BondsOfTheRim
{
    public static class InvitationCooldownPolicy
    {
        public const int TicksPerDay = 60000;
        public const int CooldownDays = 15;
        public const int CooldownTicks = TicksPerDay * CooldownDays;

        public static bool IsAvailable(int currentTick, int nextAvailableTick)
        {
            return currentTick >= nextAvailableTick;
        }

        public static float RemainingDays(int currentTick, int nextAvailableTick)
        {
            var remainingTicks = nextAvailableTick - currentTick;
            if (remainingTicks <= 0)
            {
                return 0f;
            }

            return remainingTicks / (float)TicksPerDay;
        }
    }
}
