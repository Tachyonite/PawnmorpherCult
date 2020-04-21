// MutagenicAbilityGizmo.cs created by Iron Wolf for PawnmorpherCult on 04/21/2020 1:53 PM
// last updated 04/21/2020  1:53 PM

using RimWorld;

namespace PawnmorpherCult.Abilities
{
    public class MutagenicAbilityCommand : Command_Ability
    {
        public MutagenicAbilityCommand(MutagenicAbility ability) : base(ability)
        {
            Ability = ability; 
        }

        public MutagenicAbility Ability { get; }
    }
}