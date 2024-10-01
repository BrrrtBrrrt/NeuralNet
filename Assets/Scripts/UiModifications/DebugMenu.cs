using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.UiModifications
{
#if UNITY_EDITOR
    /// <summary>
    /// Provides a custom debug menu in the Unity Editor for debugging purposes.
    /// </summary>
    public static class DebugMenu
    {
        /// <summary>
        /// Prints the global position of the currently selected GameObject in the Unity Editor.
        /// </summary>
        /// <remarks>
        /// This method is accessible via the Unity Editor menu under "Debug/Print Global Position".
        /// If no GameObject is selected, no output will be produced.
        /// </remarks>
        [MenuItem("Debug/Print Global Position")]
        public static void PrintGlobalPosition()
        {
            if (Selection.activeGameObject != null)
            {
                // Log the name and global position of the selected GameObject
                Debug.Log(Selection.activeGameObject.name + " is at " + Selection.activeGameObject.transform.position);
            }
        }
    }
#endif
}
