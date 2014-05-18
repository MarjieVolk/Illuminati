using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.AI
{
    public abstract class ActionConsumptionStrategy : GameLogicComponent
    {
        public abstract List<Action> consumeActions(int numActionsRemaining);
    }
}
