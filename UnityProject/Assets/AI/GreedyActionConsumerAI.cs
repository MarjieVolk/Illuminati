using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.AI
{
    public class GreedyActionConsumerAI : EnlightenedAI
    {
        //has a list of strategies, in priority order
        //it can schedule orders
        //it can trigger execute actions / end of turn
        public ActionConsumptionStrategy[] strategies;

        public override List<Action> scheduleActions(int numActionsToSchedule)
        {
            List<Action> ret = new List<Action>();
            foreach (ActionConsumptionStrategy strategy in strategies)
            {
                List<Action> actions = strategy.consumeActions(numActionsToSchedule);

                ret.AddRange(actions);
                numActionsToSchedule -= actions.Count;

                if (numActionsToSchedule <= 0) break;
            }

            return ret;
        }
    }
}
