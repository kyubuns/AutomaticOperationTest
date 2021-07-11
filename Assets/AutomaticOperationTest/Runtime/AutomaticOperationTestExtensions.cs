using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AutomaticOperationTest
{
    public static class AutomaticOperationTestExtensions
    {
        public static bool CheckClickable(this Button button)
        {
            if (!button.isActiveAndEnabled) return false;
            if (!button.interactable) return false;
            if (EventSystem.current == null) return false;
            if (EventSystem.current.enabled == false) return false;

            var rect = button.GetComponent<RectTransform>();
            var center = rect.position;
            var canvas = button.GetComponentInParent<Canvas>();
            var pos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, center);

            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = pos
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Any() && results[0].gameObject.GetComponentInParent<Button>() == button;
        }

        public static string GetFullName(this GameObject target)
        {
            var fullName = target.name;
            var t = target.transform.parent;

            while (t != null)
            {
                fullName = $"{t.name}/{fullName}";
                t = t.parent;
            }
            return fullName;
        }
    }
}
