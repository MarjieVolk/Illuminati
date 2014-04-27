using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Player
{
    public class VisibilityController : MonoBehaviour
    {
		public static VisibilityController instance { get; private set; }

        public delegate void VisibilityChangeHandler(Visibility visibility);

        public event VisibilityChangeHandler VisibilityChanged;

        public Visibility visibility { get; private set; }

        void OnGUI()
        {
            if (GUI.Button(new Rect(110, 10, 100, 90), "Toggle Visibility"))
            {
                ToggleVisibility();
            }
            GUI.TextArea(new Rect(110, 110, 100, 90), visibility.ToString());
        }

        void Start()
        {
			instance = this;
        }

        void Update()
        {

        }

        public void setVisibility(Visibility newVisibility)
        {
            if (newVisibility != visibility) ToggleVisibility();
        }

        private void ToggleVisibility()
        {
            if (Visibility.Public == visibility) visibility = Visibility.Private;
            else visibility = Visibility.Public;
            
            //notify everything that must be notified of the change
            if (null != VisibilityChanged) VisibilityChanged(visibility);
        }

        public enum Visibility
        {
            Public = 0, Private=1
        }
    }
}
