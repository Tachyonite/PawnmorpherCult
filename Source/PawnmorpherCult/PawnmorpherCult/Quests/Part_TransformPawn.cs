// Part_TransformPawn.cs created by Iron Wolf for PawnmorpherCult on 03/24/2020 4:32 PM
// last updated 03/24/2020  4:33 PM

using System.Collections.Generic;
using System.Linq;
using Pawnmorph;
using Pawnmorph.TfSys;
using PawnmorpherCult.Utilities;
using RimWorld;
using Verse;

namespace PawnmorpherCult.Quests
{
    public class Part_TransformPawn : SignaledPart
    {

        public PawnKindDef pawnKind;
        public float minSapience = 0;
        public float maxSapience = 1;
        public Pawn target;
        private bool _canRun = true;


        public override IEnumerable<Faction> InvolvedFactions
        {
            get
            {
                if (target?.Faction == null) yield break;
                yield return target.Faction;
            }
        }

        public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
        {
            base.Notify_PawnKilled(pawn, dinfo);
            if (pawn == target) _canRun = false;
        }

        public override void ReplacePawnReferences(Pawn replace, Pawn with)
        {
            if (replace == target)
                target = with; 
        }

        protected override void Run(in Signal signal)
        {
           
            if (!_canRun) return;
            if (target == null)
                throw new QuestException($"required quest parameter {nameof(target)} is not set in node {nameof(TransformPawn)}");


            var tfRequest = new TransformationRequest(pawnKind, target)
            {
                maxSeverity = maxSapience,
                minSeverity = minSapience,
                noLetter = true
            };


            TransformedPawn tfDPawn = MutagenDefOf.defaultMutagen.MutagenCached.Transform(tfRequest);
            if (tfDPawn == null)
            {
                Log.Error($"could not turn {target.Label} into {pawnKind.LabelCap}!");
                return;
            }

            Find.World.GetComponent<PawnmorphGameComp>().AddTransformedPawn(tfDPawn);

            quest.ReplaceReferences(target, tfDPawn.TransformedPawns.First()); 
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref pawnKind, nameof(pawnKind));
            Scribe_Values.Look(ref maxSapience, nameof(maxSapience), 1);
            Scribe_Values.Look(ref minSapience, nameof(minSapience));
            Scribe_References.Look(ref target, nameof(target));
        }
    }
}