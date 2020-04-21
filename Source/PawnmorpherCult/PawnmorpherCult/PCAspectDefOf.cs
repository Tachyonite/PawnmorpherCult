// PC_AspectDef.cs created by Iron Wolf for PawnmorpherCult on 04/21/2020 11:24 AM
// last updated 04/21/2020  11:24 AM

using Pawnmorph;
using RimWorld;

namespace PawnmorpherCult
{
    [DefOf]
    public static class PCAspectDefOf
    {
        /// <summary>
        /// aspect used to track what kind of mutagenic abilities the pawn has access to 
        /// </summary>
        public static AspectDef EtherealAmplification; 

        static PCAspectDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AspectDef));
        }
    }
}