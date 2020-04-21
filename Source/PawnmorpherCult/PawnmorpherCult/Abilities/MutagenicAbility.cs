// MutagenicAbility.cs created by Iron Wolf for PawnmorpherCult on 04/21/2020 9:20 AM
// last updated 04/21/2020  9:20 AM

using System.Collections.Generic;
using Pawnmorph;
using Pawnmorph.DebugUtils;
using Pawnmorph.TfSys;
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

        public override bool Activate(LocalTargetInfo target, LocalTargetInfo dest)
        {
            var mutagenicTracker = pawn.GetComp<MutagenicEntropyTracker>();
            if (mutagenicTracker == null) return false; 
            var mutagenicEntropyGain = def.GetMutagenicEntropyGain();
            if (mutagenicEntropyGain > float.Epsilon)
            {
                if (!mutagenicTracker.CanAcceptEntropyAmount(mutagenicEntropyGain))
                {
                    return false;
                }

                mutagenicTracker.EntropyLevel += mutagenicEntropyGain; 
            }
            //TODO AOE & target drawing 
            //TODO events 
            return base.Activate(target, dest); 
        }

        private MutagenicAbilityCommand[] _gizmo; 
        public override IEnumerable<Command> GetGizmos()
        {
            if (_gizmo == null)
            {
                //store a array of size 1 to avoid unnecessary memory allocations 
                _gizmo = new [] {new MutagenicAbilityCommand(this)};
            }

            return _gizmo; 
        }

        public override bool GizmoDisabled(out string reason)
        {
            if (base.GizmoDisabled(out reason)) return true;

            if (pawn.GetMutagenicAbilityLevel() < def.level)
            {
                reason = AbilityUtilities.NOT_HIGH_ENOUGH_LEVEL_LABEL.Translate(def.level);
                return true; 
            }

            var entropyTracker = pawn.GetComp<MutagenicEntropyTracker>();
            if (entropyTracker == null)
            {
                reason = $"{pawn.Name} does not have a mutagenic entropy tracker! this should not happen!";
                DebugLogUtils.Warning(reason);
                return true; 
            }

            if (!entropyTracker.CanAcceptEntropyAmount(def.GetMutagenicEntropyGain()))
            {
                reason = AbilityUtilities.TOO_MUCH_ENTROPY_LABEL.Translate(def.label);
                return true; 
            }

            return false; 
        }

        //TODO custom mutagenic ability Def 
        public MutagenicAbility(Pawn pawn, AbilityDef def) : base(pawn, def)
        {
        }

        public override bool CanCast
        {
            get
            {
                var canCast = base.CanCast && pawn.GetMutagenicAbilityLevel() >= def.level;
                if (canCast)
                {
                    canCast = pawn.GetComp<MutagenicEntropyTracker>()?.CanAcceptEntropyAmount(def.GetMutagenicEntropyGain())
                           ?? false;
                }

                return canCast; 
            }
        }
    }
}