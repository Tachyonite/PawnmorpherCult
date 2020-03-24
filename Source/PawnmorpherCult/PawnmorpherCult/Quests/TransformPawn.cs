// TransformPawn.cs created by Iron Wolf for PawnmorpherCult on 03/23/2020 8:49 PM
// last updated 03/23/2020  8:49 PM

using System.Collections.Generic;
using System.Linq;
using Pawnmorph;
using Pawnmorph.TfSys;
using Pawnmorph.Utilities;
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
        public SlateRef<IEnumerable<PawnKindDef>> animals;
        public SlateRef<string> traderTag;
        public SlateRef<Pawn> target;
        public SlateRef<string> storeTfAs;
        public SlateRef<bool> allAnimals = false; 
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
            var storeAs = storeTfAs.GetValue(slate); 
            if(!string.IsNullOrEmpty(storeAs))
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
            if (allAnimals.GetValue(slate))
            {
                foreach (PawnKindDef animal in DefDatabase<PawnKindDef>.AllDefs.Where(k => k.RaceProps.Animal))
                {
                    yield return animal; 
                }
                yield break;
            }

            foreach (PawnKindDef pawnKindDef in animals.GetValue(slate).MakeSafe())
            {
                yield return pawnKindDef;
            }

            var tTag = traderTag.GetValue(slate); 
            if(string.IsNullOrEmpty(tTag)) yield break;
            foreach (PawnKindDef animal in DefDatabase<PawnKindDef>.AllDefs.Where(k => k.RaceProps.Animal))
            {
                if (animal.race.tradeTags?.Contains(tTag) == true) yield return animal; 
            }

        }
    }
}