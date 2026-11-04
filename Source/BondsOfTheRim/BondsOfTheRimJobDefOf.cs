using RimWorld;
using Verse;

namespace BondsOfTheRim
{
    [DefOf]
    public static class BondsOfTheRimJobDefOf
    {
        public static JobDef BondsOfTheRim_InviteRelatedVisitor;
        public static JobDef BondsOfTheRim_ListenToInvitation;

        static BondsOfTheRimJobDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(BondsOfTheRimJobDefOf));
        }
    }
}
