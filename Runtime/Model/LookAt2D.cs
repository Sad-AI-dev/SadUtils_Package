using UnityEngine;

namespace SadUtils
{
    public static class LookAt2D
    {
        /// <summary>
        /// Calculates the rotation resulting in the positive x of the <paramref name="origin"/>
        /// to look towards the <paramref name="position"/>.<br/>
        /// This calculation ignores the z coordinate.
        /// </summary>
        /// <param name="origin">Transform which looks at <paramref name="position"/>.</param>
        /// <param name="position">World space position to look at.</param>
        /// <returns>Quaternion that results in the <paramref name="origin"/> looking to <paramref name="position"/>.</returns>
        public static Quaternion GetLookAtRotation(Transform origin, Vector3 position)
        {
            Vector2 dir = position - origin.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }

        /// <summary>
        /// Calculates the rotation resulting in the positive x of the <paramref name="origin"/>
        /// to look towards the <paramref name="target"/>.<br/>
        /// This calculation ignores the z coordinate.
        /// </summary>
        /// <param name="origin">Transform which looks at <paramref name="target"/>.</param>
        /// <param name="target">Transform to look at.</param>
        /// <returns>Quaternion that results in the <paramref name="origin"/> looking to <paramref name="target"/>.</returns>
        public static Quaternion GetLookAtRotation(Transform origin, Transform target)
        {
            return GetLookAtRotation(origin, target.position);
        }

        /// <summary>
        /// Calculates the rotation resulting in the positive x of the <paramref name="origin"/>
        /// to look towards the <paramref name="target"/> UI element.<br/>
        /// This calculation ignores the z coordinate.
        /// </summary>
        /// <param name="origin">Transform which looks at <paramref name="target"/>.</param>
        /// <param name="target">UI element to look at.</param>
        /// <param name="camera">Camera to determine mouse position through.<br/>Null be default.</param>
        /// <returns>Quaternion that results in the <paramref name="origin"/> looking to the <paramref name="target"/>.</returns>
        public static Quaternion GetLookAtRotation(Transform origin, RectTransform target, Camera camera = null)
        {
            // Do not use compound assignment for Unity objects!
            if (ReferenceEquals(camera, null))
                camera = Camera.main;

            return GetLookAtRotation(origin, camera.ScreenToWorldPoint(target.position));
        }

        /// <summary>
        /// Calculates the rotation resulting in the positive x of the <paramref name="origin"/>
        /// to look towards the mouse.<br/>
        /// This calculation ignores the z coordinate.
        /// </summary>
        /// <param name="origin">Transform which looks at the mouse.</param>
        /// <param name="camera">Camera to determine mouse position through.<br/>Null be default.</param>
        /// <returns>Quaternion that results in the <paramref name="origin"/> looking to the mouse</returns>
        public static Quaternion GetLookAtMouseRotation(Transform origin, Camera camera = null)
        {
            // Do not use compound assignment for Unity objects!
            if (ReferenceEquals(camera, null))
                camera = Camera.main;

            IMouseInputProvider mouseInputProvider = InputProviderFactory.GetMouseInputProvider();
            Vector3 mousePosition = mouseInputProvider.GetMousePosition();

            return GetLookAtRotation(origin, camera.ScreenToWorldPoint(mousePosition));
        }
    }
}
