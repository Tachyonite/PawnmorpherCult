// PCStatDefOf.cs created by Iron Wolf for PawnmorpherCult on 04/21/2020 8:25 AM
// last updated 04/21/2020  8:25 AM

using RimWorld;

namespace PawnmorpherCult
{
    public static class PCStatDefOf
    {
        /// <summary>
        /// stat used to determine how much mutagenic entropy gain an ability has 
        /// </summary>
        public static StatDef Ability_MutagenicEntropyGain; 

        static PCStatDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(StatDef)); 
        }   
    }
}