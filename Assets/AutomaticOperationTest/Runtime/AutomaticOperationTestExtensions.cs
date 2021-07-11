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
            var canvas = button.GetComponentInParent<Canvas>();
            var results = new List<RaycastResult>();
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current);

            var center = rect.position;
            var corners = new Vector3[4];
            rect.GetWorldCorners(corners);

            var minX = corners.Min(x => x.x);
            var minY = corners.Min(x => x.y);
            var minZ = corners.Max(x => x.z);
            var maxX = corners.Max(x => x.x);
            var maxY = corners.Max(x => x.y);
            var maxZ = corners.Max(x => x.z);
            var random = Enumerable.Range(0, 4).Select(_ => new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ)));

            foreach (var p in new[] { center }.Concat(corners).Concat(random))
            {
                eventDataCurrentPosition.position = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, p);
                EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
                if (results.Any() && results[0].gameObject.GetComponentInParent<Button>() == button) return true;
            }

            return false;
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
