// AbilityCompBase.cs created by Iron Wolf for PawnmorpherCult on 03/21/2020 8:50 PM
// last updated 03/21/2020  8:50 PM

using System;
using RimWorld;
using Verse;

namespace PawnmorpherCult.Abilities
{
    /// <summary>
    /// useful base class for custom ability comps 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="RimWorld.AbilityComp" />
    public abstract class AbilityCompBase<T> : CompAbilityEffect_WithDuration where T: AbilityCompProperties
    {

        public T Properties
        {
            get
            {
                try
                {
                    return (T) props; 
                }
                catch (InvalidCastException e)
                {
                    throw new InvalidCastException($"could not convert {props.GetType()} to {typeof(T).Name} in {parent.def}!",e);
                }
            }
        }

    }

    /// <summary>
    /// useful base class for ability comp properties 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Verse.AbilityCompProperties" />
    public abstract class AbilityCompPropertiesBase<T> : CompProperties_AbilityEffectWithDuration where T : AbilityComp
    {
        public AbilityCompPropertiesBase()
        {
            compClass = typeof(T); 
        }
    }


}