// Part_MutatePawn.cs created by Iron Wolf for PawnmorpherCult on 03/24/2020 4:55 PM
// last updated 03/24/2020  4:55 PM

using System.Collections.Generic;
using Pawnmorph;
using Pawnmorph.Hediffs;
using Pawnmorph.Utilities;
using RimWorld;
using UnityEngine;
using Verse;

namespace PawnmorpherCult.Quests
{
    public class Part_MutatePawn : SignaledPart
    {
        public List<MutationDef> mutations;
        public int countToAdd;
        public Pawn target;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref mutations, nameof(mutations), LookMode.Deep);
            Scribe_Values.Look(ref countToAdd, nameof(countToAdd));
            Scribe_References.Look(ref target, nameof(target));
            Scribe_Values.Look(ref inSignal, nameof(inSignal)); 
        }

        protected override void Run(in Signal signal)
        {
          
            Log.Message($"Running {nameof(Part_MutatePawn)}");
            int added = 0;
            if (target == null || target.Dead) return; 
            while (added < countToAdd)
            {
                if (mutations.Count == 0) break;
                var rElement = mutations.RandElement();
                mutations.Remove(rElement);
                if (MutationUtilities.AddMutation(target, rElement))
                    added++;
            }

            Log.Message($"{added} mutations added");
        }

        public override void ReplacePawnReferences(Pawn replace, Pawn with)
        {
            if (replace == target)
                target = with;
        }

        public override IEnumerable<Faction> InvolvedFactions
        {
            get
            {
                if(target?.Faction == null) yield break;
                yield return target.Faction;    
            }
        }
    }
}