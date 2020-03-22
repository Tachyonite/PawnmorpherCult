// AddMutation.cs created by Iron Wolf for PawnmorpherCult on 03/21/2020 9:06 PM
// last updated 03/21/2020  9:06 PM

using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using Pawnmorph;
using Pawnmorph.Hediffs;
using Pawnmorph.Utilities;
using PawnmorpherCult.Hediffs;
using Verse;

namespace PawnmorpherCult.Abilities
{
    /// <summary>
    ///     ability comp that adds mutations
    /// </summary>
    /// <seealso cref="AbilityCompBase{AddMutationProperties}" />
    public class AddMutationComp : AbilityCompBase<AddMutationProperties>
    {
        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            if (Properties.applyToSelf)
                return base.Valid(target, throwMessages) && MutagenDefOf.defaultMutagen.CanInfect(parent.pawn);

            Pawn p = target.Pawn;
            if (p == null) return false;
            return base.Valid(target, throwMessages) && MutagenDefOf.defaultMutagen.CanInfect(p);
        }


        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn addPawn = Properties.applyToSelf ? parent.pawn : target.Pawn;
            if (addPawn == null) return;

            List<RemoveMutationDurationComp.MutationEntry>
                removedMutations = new List<RemoveMutationDurationComp.MutationEntry>();
            var mutations = addPawn.health.hediffSet.hediffs.OfType<Hediff_AddedMutation>().ToList();
            //store any mutations that will be removed by this action 

            foreach (MutationDef mutationDef in Properties.mutations.MakeSafe())
            {
                StoreRemovedMutations(mutationDef, mutations, removedMutations); 
            }
            bool addedList = false;


            foreach (MutationDef mutationDef in Properties.mutations.MakeSafe())
            {
                var res = MutationUtilities.AddMutation(target.Pawn, mutationDef);
               
                foreach (Hediff_AddedMutation hediff in res)
                {
                    var rmComp = hediff.TryGetComp<RemoveMutationDurationComp>();
                    if (rmComp == null) continue;
                    rmComp.ticksToDisappear = GetDurationSeconds(hediff.pawn).SecondsToTicks();
                    if (!addedList) //only add the list to one mutation 
                    {
                        rmComp.addMutations = removedMutations;
                        addedList = true;
                    }
                }
            }
        }

        private void StoreRemovedMutations(MutationDef mDef, List<Hediff_AddedMutation> mutations,
                                           List<RemoveMutationDurationComp.MutationEntry> outList)
        {
            if (mDef.RemoveComp == null) return;

            foreach (Hediff_AddedMutation mutation in mutations)
            {
                var mRmComp = mutation.TryGetComp<RemoveFromPartComp>();
                if (mRmComp == null) continue;
                var otherMDef = mutation.def as MutationDef;
                if (otherMDef == null)
                    Log.Warning($"encountered mutation {mutation.def.defName} that does not use a MutationDef!");
                if (mRmComp.Layer == mDef.RemoveComp.layer && mDef.parts.MakeSafe().Contains(mutation.Part?.def))
                {
                    outList.Add(new RemoveMutationDurationComp.MutationEntry
                    {
                        mDef = otherMDef,
                        severity = mutation.Severity,
                        record = mutation.Part
                    });
                }
            }
        }
    }

    /// <summary>
    ///     ability comp properties for adding mutations
    /// </summary>
    /// <seealso cref="AbilityCompPropertiesBase{AddMutationComp}" />
    public class AddMutationProperties : AbilityCompPropertiesBase<AddMutationComp>
    {
        public List<MutationDef> mutations = new List<MutationDef>();
        public bool applyToSelf;
    }
}