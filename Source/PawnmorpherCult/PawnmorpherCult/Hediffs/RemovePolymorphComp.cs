// RemovePolymorphComp.cs created by Iron Wolf for PawnmorpherCult on 03/22/2020 10:35 PM
// last updated 03/22/2020  10:35 PM

using Pawnmorph;
using Pawnmorph.TfSys;
using Verse;

namespace PawnmorpherCult.Hediffs
{
    public class RemovePolymorphComp : HediffComp_Disappears
    {

        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            if (Pawn.Dead) return; 
            if(Find.World.GetComponent<PawnmorphGameComp>().GetPawnStatus(parent.pawn) == TransformedStatus.Transformed)
                MutagenDefOf.defaultMutagen.MutagenCached.TryRevert(parent.pawn); 
            else 
                Log.Warning($"{parent.def.defName} on {parent.pawn.Name} but that pawn is not a former human!");
        }

     
    }

    public class RemovePolymorphCompProperties : HediffCompProperties_Disappears
    {
        public RemovePolymorphCompProperties()
        {
            compClass = typeof(RemovePolymorphComp); 
        }
    }
}