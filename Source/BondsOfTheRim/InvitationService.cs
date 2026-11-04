using RimWorld;
using Verse;

namespace BondsOfTheRim
{
    internal static class InvitationService
    {
        internal static bool TryGetEligibleRelation(Pawn colonist, Pawn visitor, out string relationDefName)
        {
            relationDefName = null;

            if (colonist == null || visitor == null
                || !colonist.IsColonistPlayerControlled
                || !visitor.Spawned
                || visitor.Dead
                || !visitor.RaceProps.Humanlike
                || visitor.Faction == null
                || visitor.Faction == Faction.OfPlayer
                || visitor.IsPrisonerOfColony
                || visitor.HostileTo(Faction.OfPlayer))
            {
                return false;
            }

            foreach (var relation in colonist.relations.DirectRelations)
            {
                if (relation.otherPawn != visitor || relation.def == null)
                {
                    continue;
                }

                var candidateDefName = relation.def.defName;
                if (!InvitationPolicy.IsEligibleRelationship(candidateDefName))
                {
                    continue;
                }

                if (candidateDefName == "Friend"
                    && visitor.relations.OpinionOf(colonist) < InvitationPolicy.FriendOpinionThreshold)
                {
                    continue;
                }

                relationDefName = candidateDefName;
                return true;
            }

            return false;
        }

        internal static void ResolveInvitation(Pawn colonist, Pawn visitor)
        {
            if (!TryGetEligibleRelation(colonist, visitor, out var relationDefName))
            {
                Trace("Invitation was cancelled because eligibility changed before resolution.");
                return;
            }

            var component = Current.Game.GetComponent<BondsOfTheRimGameComponent>();
            if (component.IsOnCooldown(visitor))
            {
                Trace("Invitation was cancelled because the visitor is on cooldown.");
                return;
            }

            component.RecordAttempt(visitor);

            var socialSkill = colonist.skills.GetSkill(SkillDefOf.Social).Level;
            var visitorOpinion = visitor.relations.OpinionOf(colonist);
            var chance = InvitationPolicy.CalculateAcceptanceChance(relationDefName, socialSkill, visitorOpinion);
            Trace($"Resolving invitation with chance {chance:P0}; Social={socialSkill}, opinion={visitorOpinion}.");

            if (Rand.Chance(chance))
            {
                visitor.SetFaction(Faction.OfPlayer, colonist);
                Trace("Invitation accepted; visitor joined the player faction.");
                Messages.Message(
                    "BondsOfTheRim.InvitationAccepted".Translate(visitor.LabelShort),
                    visitor,
                    MessageTypeDefOf.PositiveEvent);
                return;
            }

            Trace("Invitation declined by probability roll.");
            Messages.Message(
                "BondsOfTheRim.InvitationDeclined".Translate(visitor.LabelShort),
                visitor,
                MessageTypeDefOf.NeutralEvent);
        }

        internal static void Trace(string message)
        {
            if (Prefs.DevMode)
            {
                Log.Message("[BondsOfTheRim] " + message);
            }
        }
    }
}
