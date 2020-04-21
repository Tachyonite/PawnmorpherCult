// AbilityUtilities.cs created by Iron Wolf for PawnmorpherCult on 04/21/2020 9:54 AM
// last updated 04/21/2020  9:54 AM

using System;
using JetBrains.Annotations;
using Pawnmorph;
using RimWorld;
using Verse;

namespace PawnmorpherCult.Utilities
{
    public static class AbilityUtilities
    {
        /// <summary>
        /// Gets the mutagenic entropy gain from this ability def.
        /// </summary>
        /// <param name="def">The definition.</param>
        /// <returns></returns>
        public static float GetMutagenicEntropyGain([NotNull] this AbilityDef def)
        {
            if (def == null) throw new ArgumentNullException(nameof(def));
            return def.statBases?.GetStatValueFromList(PCStatDefOf.Ability_MutagenicEntropyGain, 0) ?? 0;
        }


        /// <summary>
        /// Gets the level of mutagenic ability this pawn is capable of casting.
        /// </summary>
        /// <param name="pawn">The pawn.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">pawn</exception>
        public static int GetMutagenicAbilityLevel([NotNull] this Pawn pawn)
        {
            if (pawn == null) throw new ArgumentNullException(nameof(pawn));
            var idx = pawn.GetAspectTracker()?.GetAspect(PCAspectDefOf.EtherealAmplification)?.StageIndex;
            
            //if they don't have the aspect return 0
            //if they do return stage index + 1 so the first stage counts as level 1 
            return idx + 1 ?? 0; 
        }
    }
}