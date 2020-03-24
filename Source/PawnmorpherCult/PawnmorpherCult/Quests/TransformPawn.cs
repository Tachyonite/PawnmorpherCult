// TransformPawn.cs created by Iron Wolf for PawnmorpherCult on 03/23/2020 8:49 PM
// last updated 03/23/2020  8:49 PM

using System.Collections.Generic;
using System.Linq;
using Pawnmorph;
using Pawnmorph.TfSys;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace PawnmorpherCult.Quests
{
    /// <summary>
    /// Quest Node that will transform a pawn into a former human
    /// </summary>
    /// <seealso cref="RimWorld.QuestGen.QuestNode" />
    public class TransformPawn : QuestNode
    {
        public SlateRef<FloatRange> sapienceRange = new FloatRange(0, 1); 
        public SlateRef<IEnumerable<PawnKindDef>> animalKinds;
        public SlateRef<Pawn> target;
        public SlateRef<string> storeTfAs;
        protected override void RunInt()
        {
            var slate = QuestGen.slate;
            var pawn = target.GetValue(slate);
            var range = sapienceRange.GetValue(slate); 
            if(pawn == null) throw new QuestException($"required quest parameter {nameof(target)} is not set in node {nameof(TransformPawn)}");
            var tfTarget = GetTransformations(slate).RandomElement();
            var tfRequest = new TransformationRequest(tfTarget, pawn)
            { 
                maxSeverity = range.max,
                minSeverity = range.min
            };


            var tfDPawn = MutagenDefOf.defaultMutagen.MutagenCached.Transform(tfRequest);
            if (tfDPawn == null)
            {
                Log.Error($"could not turn {pawn.Label} into {tfTarget.LabelCap}!");
                return; 
            }

            Find.World.GetComponent<PawnmorphGameComp>().AddTransformedPawn(tfDPawn);

            slate.Set(storeTfAs.GetValue(slate), tfDPawn.TransformedPawns.First());
        }

        protected override bool TestRunInt(Slate slate)
        {
            var pawn = target.GetValue(slate);
            if (pawn == null || !MutagenDefOf.defaultMutagen.CanTransform(pawn)) return false;

            return GetTransformations(slate).Any(); 
        }

        public IEnumerable<PawnKindDef> GetTransformations(Slate slate)
        {
            IEnumerable<PawnKindDef> kinds = animalKinds.GetValue(slate);
            if (kinds == null) kinds = DefDatabase<PawnKindDef>.AllDefs.Where(k => k.RaceProps.Animal);

            return kinds; 
        }
    }
}