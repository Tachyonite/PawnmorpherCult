// RemoveMutationDuration.cs created by Iron Wolf for PawnmorpherCult on 03/22/2020 9:21 AM
// last updated 03/22/2020  9:21 AM

using System.Collections.Generic;
using JetBrains.Annotations;
using Pawnmorph;
using Pawnmorph.Hediffs;
using Pawnmorph.Utilities;
using RimWorld;
using Verse;

namespace PawnmorpherCult.Hediffs
{
    public class RemoveMutationDurationComp : HediffComp_Disappears
    {
        [CanBeNull]
        public List<MutationEntry> addMutations;

        
        public override void CompExposeData()
        {
            base.CompExposeData();

            Scribe_Deep.Look(ref addMutations, nameof(addMutations));

        }

        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();

            foreach (MutationEntry mutationEntry in addMutations.MakeSafe())
            {
                if(mutationEntry.mDef == null) continue;

                var res = MutationUtilities.AddMutation(Pawn, mutationEntry.mDef, mutationEntry.record);  
                if(!res) continue;
                foreach (Hediff_AddedMutation mutation in res)
                {
                    mutation.Severity = mutationEntry.severity; 
                }
            }

            addMutations?.Clear();

        }


        public class MutationEntry : IExposable
        {
            public MutationDef mDef;
            public float severity;
            public BodyPartRecord record;
            public void ExposeData()
            {
                Scribe_Values.Look(ref severity, nameof(severity));
                Scribe_BodyParts.Look(ref record, nameof(record));
                Scribe_Defs.Look(ref mDef, nameof(mDef));
            }
        }
    }

    public class RemoveMutationDurationProperties : HediffCompProperties_Disappears
    {
        public RemoveMutationDurationProperties()
        {
            compClass = typeof(RemoveMutationDurationComp);
        } 

    }
}