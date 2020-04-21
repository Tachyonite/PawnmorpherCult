// MutagicEntropyTracker.cs created by Iron Wolf for PawnmorpherCult on 04/21/2020 7:53 AM
// last updated 04/21/2020  7:53 AM

using System.Collections.Generic;
using Pawnmorph;
using UnityEngine;
using Verse;

namespace PawnmorpherCult.AbilitySys
{
    /// <summary>
    ///     comp for tracking the amount of 'mutagenic' entropy on a pawn
    /// </summary>
    /// <seealso cref="Verse.ThingComp" />
    public class MutagenicEntropyTracker : ThingComp
    {
        //TODO events 


        //TODO make these stats? 
        public const float MAX_ENTROPY = 100;
        public const float MIN_ENTROPY = 0;
        private const float EPSILON = 0.01f;
        private const float OVERFLOW_PERCENTAGE = 0.1f;
        public const float MAX_OVERFLOW = MAX_ENTROPY * (1 + OVERFLOW_PERCENTAGE);
        private float _entropyLevel = 0;

        private bool _allowOverflow;

        public float MaxEntropy => AllowOverflow ? MAX_OVERFLOW : MAX_ENTROPY;

        public float EntropyRelativeValue => EntropyLevel / MaxEntropy;

        public float RecoveryRatePerSecond => 0.1f; //TODO 

        public float RecoveryRate => RecoveryRatePerSecond; //TODO 

        public bool AllowOverflow
        {
            get => _allowOverflow;
            set
            {
                _allowOverflow = value;
                if (value)
                    EnableOverflow();
                else
                    DisableOverflow();
            }
        }

        /// <summary>
        ///     Gets or sets the entropy level of the attached pawn.
        /// </summary>
        /// <value>
        ///     The entropy level.
        /// </value>
        public float EntropyLevel
        {
            get => _entropyLevel;
            set => _entropyLevel = Mathf.Clamp(value, MIN_ENTROPY, MAX_OVERFLOW);
        }

        public Pawn Parent => (Pawn) parent;

        /// <summary>
        ///     Determines whether the attached pawn can accept the amount of mutagenic entropy
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns>
        ///     <c>true</c> if the attached pawn can accept the amount of mutagenic entropy otherwise, <c>false</c>.
        /// </returns>
        public bool CanAcceptEntropyAmount(float amount)
        {
            float mx = _allowOverflow ? MAX_OVERFLOW : MAX_ENTROPY;
            return EntropyLevel + amount < mx;
        }

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            if (!(parent is Pawn))
                Log.Error($"{nameof(MutagenicEntropyTracker)} is attached to a non Pawn {parent.ThingID}/{parent.Label}! this comp only works on Pawns");
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref _entropyLevel, nameof(EntropyLevel));
            Scribe_Values.Look(ref _allowOverflow, nameof(AllowOverflow));
        }

        private Gizmo _gizmo;

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (Enabled)
            {
                _gizmo = _gizmo ?? new MutagenicEntropyGizmo(this);
                yield return _gizmo; 
            }
        }

        private bool Enabled => Parent.IsHumanlike(); //TODO 

        private void DisableOverflow()
        {
        }

        private void EnableOverflow()
        {
        }
    }
}