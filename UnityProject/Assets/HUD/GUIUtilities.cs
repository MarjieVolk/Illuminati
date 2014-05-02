using System;
using UnityEngine;

namespace Assets.HUD {
	public class GUIUtilities {

        public static Rect getRect(float width, float height) {
            return getRect(width, height, 0, 0);
        }

        public static Rect getRect(float width, float height, float xOffset, float yOffset) {
            return new Rect((Screen.width / 2.0f) - (width / 2.0f) + xOffset, (Screen.height / 2.0f) - (height / 2.0f) + yOffset, width, height);
        }
	}
}
