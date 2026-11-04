using RimWorld;
using Verse;
using Verse.AI;

namespace BondsOfTheRim
{
    public sealed class FloatMenuOptionProvider_InviteRelatedVisitor : FloatMenuOptionProvider
    {
        protected override bool Drafted => false;
        protected override bool Undrafted => true;
        protected override bool Multiselect => false;

        protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
        {
            var colonist = context.FirstSelectedPawn;
            if (!InvitationService.TryGetEligibleRelation(colonist, clickedPawn, out var relationDefName))
            {
                return null;
            }

            var scene = RelationshipScenePolicy.For(relationDefName);
            if (scene == RelationshipScene.None)
            {
                return null;
            }

            var component = Current.Game.GetComponent<BondsOfTheRimGameComponent>();
            if (component.IsOnCooldown(clickedPawn))
            {
                var remainingDays = component.RemainingCooldownDays(clickedPawn).ToString("F1");
                return new FloatMenuOption(
                    "BondsOfTheRim.InviteOnCooldown".Translate(clickedPawn.LabelShort, remainingDays),
                    null);
            }

            return new FloatMenuOption(
                "BondsOfTheRim.Invite".Translate(clickedPawn.LabelShort),
                delegate { QueueInvitation(colonist, clickedPawn, scene, relationDefName); });
        }

        private static void QueueInvitation(Pawn colonist, Pawn visitor, RelationshipScene scene, string relationDefName)
        {
            if (!InvitationService.TryGetEligibleRelation(colonist, visitor, out _))
            {
                InvitationService.Trace("Invitation was cancelled because eligibility changed before the conversation started.");
                return;
            }

            var listenerJob = JobMaker.MakeJob(BondsOfTheRimJobDefOf.BondsOfTheRim_ListenToInvitation, colonist);
            listenerJob.count = RelationshipScenePolicy.ListenerDurationTicks(scene);
            visitor.jobs.TryTakeOrderedJob(listenerJob);

            var speakerJob = JobMaker.MakeJob(BondsOfTheRimJobDefOf.BondsOfTheRim_InviteRelatedVisitor, visitor);
            speakerJob.count = RelationshipScenePolicy.DurationTicks(scene);
            colonist.jobs.TryTakeOrderedJob(speakerJob);
            InvitationService.Trace($"Paired conversation queued: {colonist.LabelShort} -> {visitor.LabelShort} through '{relationDefName}'.");
        }
    }
}
