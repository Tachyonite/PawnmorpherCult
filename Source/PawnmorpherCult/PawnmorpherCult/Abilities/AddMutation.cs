// AddMutation.cs created by Iron Wolf for PawnmorpherCult on 03/21/2020 9:06 PM
// last updated 03/21/2020  9:06 PM

using System.Collections.Generic;
using Pawnmorph;
using Pawnmorph.Hediffs;
using Pawnmorph.Utilities;
using RimWorld;
using Verse;

namespace PawnmorpherCult.Abilities
{
    /// <summary>
    /// ability comp that adds mutations 
    /// </summary>
    /// <seealso cref="AbilityCompBase{AddMutationProperties}" />
    public class AddMutationComp : AbilityCompBase<AddMutationProperties>
    {
        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            if (Properties.applyToSelf)
            {
                return base.Valid(target, throwMessages) && MutagenDefOf.defaultMutagen.CanInfect(parent.pawn); 
            }

            var p = target.Pawn;
            if (p == null) return false;
            return base.Valid(target, throwMessages) && MutagenDefOf.defaultMutagen.CanInfect(p); 
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn addPawn = Properties.applyToSelf ? parent.pawn : target.Pawn;

            if (addPawn == null) return; 
            foreach (MutationDef mutationDef in Properties.mutations.MakeSafe())
            {
                MutationUtilities.AddMutation(target.Pawn, mutationDef); 
            }

        }

       
    }
    /// <summary>
    /// ability comp properties for adding mutations 
    /// </summary>
    /// <seealso cref="AbilityCompPropertiesBase{AddMutationComp}" />
    public class AddMutationProperties : AbilityCompPropertiesBase<AddMutationComp>
    {
        public List<MutationDef> mutations = new List<MutationDef>();
        public bool applyToSelf;

    }
}