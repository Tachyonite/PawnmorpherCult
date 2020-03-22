// StockGenerator_BuyFormerHuman.cs created by Iron Wolf for PawnmorpherCult on 03/22/2020 4:34 PM
// last updated 03/22/2020  4:34 PM

using System.Collections.Generic;
using System.Linq;
using Pawnmorph;
using RimWorld;
using Verse;

namespace PawnmorpherCult.Traders
{
    //TODO figure out a way to restrict it to former humans only 

    public class StockGenerator_BuyFormerHuman : StockGenerator
    {
        public SapienceLevel minSapienceLevel;
        public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
        {
            return Enumerable.Empty<Thing>();
        }

        public override bool HandlesThingDef(ThingDef thingDef)
        {
            return thingDef.race?.Animal == true;
        }


        
    }


    public class StockGenerator_BuyChaomorph : StockGenerator
    {
        public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
        {
            return Enumerable.Empty<Thing>();
        }

        public override bool HandlesThingDef(ThingDef thingDef)
        {
            return thingDef?.tradeTags?.Contains("Chaomorph") == true; 
        }
    }
    
}