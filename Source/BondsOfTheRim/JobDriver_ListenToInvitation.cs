using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace BondsOfTheRim
{
    public sealed class JobDriver_ListenToInvitation : JobDriver
    {
        private Pawn Speaker => (Pawn)TargetA.Thing;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);

            yield return Toils_General.Do(delegate
            {
                pawn.pather.StopDead();
            });

            var listen = Toils_General.Wait(job.count);
            listen.tickAction = delegate
            {
                pawn.rotationTracker.FaceCell(Speaker.Position);
            };
            yield return listen;
        }
    }
}
