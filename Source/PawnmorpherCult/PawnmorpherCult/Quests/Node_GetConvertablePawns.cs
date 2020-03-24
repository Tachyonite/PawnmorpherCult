// Node_GetConvertablePawns.cs created by Iron Wolf for PawnmorpherCult on 03/23/2020 6:03 PM
// last updated 03/23/2020  6:03 PM

using System;
using System.Collections.Generic;
using System.Linq;
using Pawnmorph;
using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace PawnmorpherCult.Quests
{

    /// <summary>
    /// quest node for getting a pawn that can be converted by the cult 
    /// </summary>
    /// <seealso cref="RimWorld.QuestGen.QuestNode" />
    public class GetConvertablePawn : QuestNode
    {
        public SlateRef<string> storeAs; 
        public SlateRef<bool> mustBeColonist;
        public SlateRef<bool> mustBePrisoner;

        public SlateRef<bool> canBeHybrid = false;
        public SlateRef<bool> canBeFormerHuman = false; 

        protected override void RunInt()
        {
            var slate = QuestGen.slate;
            var pawn = PawnsFinder.AllMaps_FreeColonistsAndPrisoners.Where(p => GoodPawn(p, slate))
                                       .RandomElementByWeight(GetWeight);

            if(pawn.Faction != null && !pawn.Faction.def.hidden)
            {
                QuestPart_InvolvedFactions factionPart = new QuestPart_InvolvedFactions()
                {
                    factions = new List<Faction> {pawn.Faction}
                };
                QuestGen.quest.AddPart(factionPart);
            }

            slate.Set(storeAs.GetValue(slate), pawn); 

        }

        float GetWeight(Pawn pawn)
        {
            var mutTracker = pawn.GetMutationTracker();
            if (mutTracker == null)
            {
                return  0; 
            }

            const float epsilon = 0.01f; 
            float weight;
            //the less mutation they have the more the cult wants them 
            if (Math.Abs(mutTracker.TotalInfluence) < epsilon)
            {
                weight = 1;
            }
            else weight = 1 / mutTracker.TotalInfluence;

            return weight;
        }


        protected override bool TestRunInt(Slate slate)
        {
            if (mustBeColonist.GetValue(slate) && mustBePrisoner.GetValue(slate))
            {
                return false;
            }
            return PawnsFinder.AllMaps_FreeColonistsAndPrisoners.Any(p => GoodPawn(p, slate));
        }

      

        bool GoodPawn(Pawn pawn, Slate slate)
        {
            if (!pawn.IsColonist && mustBeColonist.GetValue(slate)) return false;
            if (!pawn.IsPrisoner && mustBePrisoner.GetValue(slate)) return false;
            if (pawn.IsFormerHuman() && !canBeFormerHuman.GetValue(slate)) return false;
            if (pawn.IsHybridRace() && !canBeHybrid.GetValue(slate)) return false;
            if (pawn.GetMutationTracker(false) == null) return false; 
            return true; 
        }
    }
}