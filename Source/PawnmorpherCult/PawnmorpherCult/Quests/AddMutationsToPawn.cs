// AddMutationsToPawn.cs created by Iron Wolf for PawnmorpherCult on 03/23/2020 8:17 PM
// last updated 03/23/2020  8:18 PM

using System.Collections.Generic;
using System.Linq;
using Pawnmorph;
using Pawnmorph.Hediffs;
using Pawnmorph.Utilities;
using RimWorld.QuestGen;
using Verse;

namespace PawnmorpherCult.Quests
{

    /// <summary>
    /// quest node for adding mutations to pawns 
    /// </summary>
    /// <seealso cref="RimWorld.QuestGen.QuestNode" />
    public class AddMutationsToPawn : QuestNode
    {
        public SlateRef<IEnumerable<MutationDef>> mutations;
        public SlateRef<AnimalClassBase> animalClass;
        public SlateRef<IEnumerable<MutationCategoryDef>> mutationCategories; 
        public SlateRef<IEnumerable<MorphCategoryDef>> morphCategory; 
        public SlateRef<bool> chimera; 
        public SlateRef<IntRange> addRange;
        public SlateRef<bool> allowRestricted; 
        public SlateRef<Pawn> pawn;  

        protected override void RunInt()
        {
            var slate = QuestGen.slate;
            var numMutations = addRange.GetValue(slate).RandomInRange;

            if (numMutations == 0) return;

            var mPawn = pawn.GetValue(slate); 
            
            if(mPawn == null) throw new QuestException($"required pawn ${nameof(pawn)}/\"{((ISlateRef) pawn).SlateRef}\" is missing!");


            var allMutations = GetAllAvailableMutations(slate).Distinct().ToList();
            int added = 0;
            while (added < numMutations)
            {
                if(allMutations.Count == 0) break;
                var rElement = allMutations.RandElement();
                allMutations.Remove(rElement);
                if (MutationUtilities.AddMutation(mPawn, rElement))
                    added++; 
            }

        }

        protected override bool TestRunInt(Slate slate)
        {
            var mPawn = pawn.GetValue(slate);
            if (mPawn == null) return false;
            return GetAllAvailableMutations(slate).Any(); 
        }

        
        IEnumerable<MutationDef> GetAllAvailableMutations(Slate slate)
        {
            //first go through the explicitly given mutations 
            foreach (MutationDef mutationDef in mutations.GetValue(slate).MakeSafe())
            {
                yield return mutationDef; 
            }

            //now mutation categories 
            foreach (var mutationCategory in mutationCategories.GetValue(slate).MakeSafe())
            {
                foreach (MutationDef mutation in mutationCategory.AllMutations)
                {
                    if(mutation.IsRestricted && !allowRestricted.GetValue(slate)) continue;
                    yield return mutation; 
                }
            }

            //now for morphs 
            List<MorphDef> allMorphs = new List<MorphDef>();
            allMorphs.AddRange(morphCategory.GetValue(slate).MakeSafe().SelectMany(m => m.AllMorphsInCategories.Where(mm => mm.AllAssociatedMutations.Any())));
            var aClass = animalClass.GetValue(slate);
            if (aClass != null)
            {
                allMorphs.AddRange(aClass.GetAllMorphsInClass().Where(m => m.AllAssociatedMutations.Any())); 
            }

            if (chimera.GetValue(slate))
            {
                //pick all mutations from morphs 
                foreach (MorphDef allMorph in allMorphs)
                {
                    foreach (MutationDef mutation in allMorph.AllAssociatedMutations)
                    {
                        if (mutation.IsRestricted && !allowRestricted.GetValue(slate))
                        {
                            continue;
                        }

                        yield return mutation; 
                    }
                }
            }
            else
            {
                //just get mutations from one morph 
                var morph = allMorphs.RandElement(); 
                if(morph == null) yield break;
                foreach (MutationDef mutation in morph.AllAssociatedMutations)
                {
                    if (mutation.IsRestricted && !allowRestricted.GetValue(slate))
                    {
                        continue;
                    }

                    yield return mutation; 
                }

            }
        }
    }
}