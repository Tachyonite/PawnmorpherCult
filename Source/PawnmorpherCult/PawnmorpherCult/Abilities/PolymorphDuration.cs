// PolymorphDuration.cs created by Iron Wolf for PawnmorpherCult on 03/22/2020 10:28 PM
// last updated 03/22/2020  10:28 PM

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Pawnmorph;
using Pawnmorph.TfSys;
using Pawnmorph.Utilities;
using Verse;

namespace PawnmorpherCult.Abilities
{
    public class PolymorphDurationComp : AbilityCompBase<PolymorphDurationProperties>
    {
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);

            var transformationRequest = new TransformationRequest(Properties.TargetAnimals.RandElement(), target.Pawn);

            var tfdPawn = MutagenDefOf.defaultMutagen.MutagenCached.Transform(transformationRequest);
            var animal = tfdPawn?.TransformedPawns.First(); 

            if (tfdPawn != null)
            {
                Find.World.GetComponent<PawnmorphGameComp>().AddTransformedPawn(tfdPawn); 
                Hediff hediff = HediffMaker.MakeHediff(Properties.trackerHediff, animal);
                var rmComp = hediff.TryGetComp<HediffComp_Disappears>();

                rmComp.ticksToDisappear = GetDurationSeconds(target.Pawn).SecondsToTicks();
                animal.health.AddHediff(hediff); 
            }
            else
            {
                Log.Warning($"could not transform target {target.Pawn.Label}");
            }
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            if (target.Pawn == null || !MutagenDefOf.defaultMutagen.CanTransform(target.Pawn))
                return false;
            return base.Valid(target, throwMessages);
        }
    }

    public class PolymorphDurationProperties : AbilityCompPropertiesBase<PolymorphDurationComp>
    {
        public List<PawnKindDef> targetAnimals;
        public HediffDef trackerHediff;

        private List<PawnKindDef> _targetsCached;

        [NotNull]
        public List<PawnKindDef> TargetAnimals
        {
            get
            {
                if (_targetsCached == null)
                {
                    if (targetAnimals != null)
                        _targetsCached = targetAnimals;
                    else
                        _targetsCached = DefDatabase<PawnKindDef>.AllDefsListForReading.Where(d => d.RaceProps.Animal).ToList();
                }

                return _targetsCached;
            }
        }
    }
}