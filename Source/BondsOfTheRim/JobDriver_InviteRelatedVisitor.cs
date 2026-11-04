using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace BondsOfTheRim
{
    public sealed class JobDriver_InviteRelatedVisitor : JobDriver
    {
        private Pawn Visitor => (Pawn)TargetA.Thing;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);

            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);

            yield return Toils_General.Do(delegate
            {
                if (!InvitationService.TryGetEligibleRelation(pawn, Visitor, out var relationDefName))
                {
                    return;
                }

                var scene = RelationshipScenePolicy.For(relationDefName);
                var messageKey = RelationshipScenePolicy.StartedMessageKey(scene);
                if (messageKey != null)
                {
                    Messages.Message(
                        messageKey.Translate(pawn.LabelShort, Visitor.LabelShort),
                        pawn,
                        MessageTypeDefOf.NeutralEvent);
                }

                var chitchat = DefDatabase<InteractionDef>.GetNamedSilentFail("Chitchat");
                if (chitchat != null)
                {
                    pawn.interactions.TryInteractWith(Visitor, chitchat);
                }
            });

            yield return Toils_General.Wait(job.count);

            yield return Toils_General.Do(delegate
            {
                InvitationService.ResolveInvitation(pawn, Visitor);
            });
        }
    }
}
