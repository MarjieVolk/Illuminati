using Assets.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Actions
{
    public class ActionGUI : MonoBehaviour
    {
        private Action Action;
        private GameObject listTag, mapTag;
        public GameObject button;
        public GameObject scheduledTag;

        private void Start()
        {
            Action = GetComponent<Action>();
            Action.OnStateUpdate += onScheduled;
        }

        private void onScheduled(Action action)
        {
            if (action.transform.parent.GetComponent<NodeData>().isScheduled)
            {
                if (TurnController.instance.CurrentPlayer.IsLocalHumanPlayer)
                {
                    GameObject tag = getMapScheduledTag();
                    tag.SetActive(true);
                    tag.transform.position = gameObject.transform.position + new Vector3(0.5f, 0.3f, -1);
                }
            }
        }

        public GameObject getListScheduledTag()
        {
            if (listTag == null)
            {
                initTags();
            }
            return listTag;
        }

        public GameObject getMapScheduledTag()
        {
            if (mapTag == null)
            {
                initTags();
            }
            return mapTag;
        }

        private void initTags()
        {
            mapTag = instantiateTag();
            listTag = instantiateTag();
            mapTag.GetComponent<ScheduledAction>().setSister(listTag.GetComponent<ScheduledAction>());
            listTag.GetComponent<ScheduledAction>().setDragable(true);
        }

        private GameObject instantiateTag()
        {
            GameObject tag = (GameObject)Instantiate(scheduledTag, new Vector3(0, 0, -10), Quaternion.identity);
            tag.GetComponent<ScheduledAction>().action = Action;
            tag.SetActive(false);
            return tag;
        }
    }
}
