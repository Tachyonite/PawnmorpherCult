// QuestException.cs created by Iron Wolf for PawnmorpherCult on 03/23/2020 8:45 PM
// last updated 03/23/2020  8:45 PM

namespace PawnmorpherCult.Quests
{
    /// <summary>
    /// exception thrown when a quest node encounters an unrecoverable error 
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class QuestException : System.Exception
    {
        public QuestException(string message) : base(message)
        {
        }

        public QuestException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}