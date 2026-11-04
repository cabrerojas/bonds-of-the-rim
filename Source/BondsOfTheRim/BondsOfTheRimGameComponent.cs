using System.Collections.Generic;
using Verse;

namespace BondsOfTheRim
{
    public sealed class BondsOfTheRimGameComponent : GameComponent
    {
        private Dictionary<Pawn, int> nextInvitationTicks = new Dictionary<Pawn, int>();

        public BondsOfTheRimGameComponent(Game game)
        {
        }

        public bool IsOnCooldown(Pawn pawn)
        {
            return nextInvitationTicks.TryGetValue(pawn, out var nextTick)
                && !InvitationCooldownPolicy.IsAvailable(Find.TickManager.TicksGame, nextTick);
        }

        public float RemainingCooldownDays(Pawn pawn)
        {
            return nextInvitationTicks.TryGetValue(pawn, out var nextTick)
                ? InvitationCooldownPolicy.RemainingDays(Find.TickManager.TicksGame, nextTick)
                : 0f;
        }

        public void RecordAttempt(Pawn pawn)
        {
            nextInvitationTicks[pawn] = Find.TickManager.TicksGame + InvitationCooldownPolicy.CooldownTicks;
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref nextInvitationTicks, "nextInvitationTicks", LookMode.Reference, LookMode.Value);

            if (Scribe.mode == LoadSaveMode.PostLoadInit && nextInvitationTicks == null)
            {
                nextInvitationTicks = new Dictionary<Pawn, int>();
            }
        }
    }
}
