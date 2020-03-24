// SignaledPart.cs created by Iron Wolf for PawnmorpherCult on 03/24/2020 4:59 PM
// last updated 03/24/2020  4:59 PM

using RimWorld;
using Verse;

namespace PawnmorpherCult.Quests
{
    /// <summary>
    /// abstract base class for quest parts that are signaled 
    /// </summary>
    /// <seealso cref="RimWorld.QuestPart" />
    public abstract class SignaledPart : QuestPart
    {
        public string inSignal;


        public override void Notify_QuestSignalReceived(Signal signal)
        {
            base.Notify_QuestSignalReceived(signal);
            if (signal.tag != inSignal) return;
            Run(signal); 
        }

        protected abstract void Run(in Signal signal); 

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref inSignal, nameof(inSignal));
        }
    }
}