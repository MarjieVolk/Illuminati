using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.AI
{
    public abstract class EnlightenedAI : MonoBehaviour
    {
        public abstract List<Action> scheduleActions(int numActionsToSchedule);
    }
}
