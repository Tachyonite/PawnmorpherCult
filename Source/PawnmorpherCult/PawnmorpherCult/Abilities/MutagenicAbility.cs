// MutagenicAbility.cs created by Iron Wolf for PawnmorpherCult on 04/21/2020 9:20 AM
// last updated 04/21/2020  9:20 AM

using PawnmorpherCult.Utilities;
using RimWorld;
using Verse;

namespace PawnmorpherCult.Abilities
{
    /// <summary>
    /// base class for all mutagenic abilities 
    /// </summary>
    /// <seealso cref="RimWorld.Ability" />
    public class MutagenicAbility : Ability 
    {
        public MutagenicAbility(Pawn pawn) : base(pawn)
        {
        }

        //TODO custom mutagenic ability Def 
        public MutagenicAbility(Pawn pawn, AbilityDef def) : base(pawn, def)
        {
        }

        public override bool CanCast
        {
            get { return base.CanCast && pawn.GetMutagenicAbilityLevel() >= def.level;  }
        }
    }
}