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

        void Awake()
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

        public void ToggleVisibility()
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
