// QuestUtilities.cs created by Iron Wolf for PawnmorpherCult on 03/24/2020 6:09 PM
// last updated 03/24/2020  6:09 PM

using System;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace PawnmorpherCult.Utilities
{
    public static class QuestUtilities
    {
        public static void ReplaceReferences([NotNull] this Quest quest,  Pawn original, Pawn replacement)
        {
            if (quest == null) throw new ArgumentNullException(nameof(quest));
            foreach (QuestPart questPart in quest.PartsListForReading)
            {
                questPart.ReplacePawnReferences(original, replacement);

                //lent colonist doesn't implement ReplacePawns correctly so we need to fix that 
                if (questPart is QuestPart_LendColonistsToFaction lq)
                {
                    if (lq.LentColonistsListForReading != null)
                    {
                        for (int i = 0; i < lq.LentColonistsListForReading.Count; i++)
                        {
                            var p = lq.LentColonistsListForReading[i];
                            if (p == original)
                                lq.LentColonistsListForReading[i] = replacement; 
                        }
                    }

                }

            }
        }
    }
}