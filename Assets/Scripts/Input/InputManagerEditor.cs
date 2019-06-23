using UnityEditor;
using UnityEngine;

namespace ControllerInputCreator
{
#if UNITY_EDITOR
    [CustomEditor(typeof(InputCreator))]
    public class InputCreatorManager : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Update"))
            {
                (target as InputCreator).Init();
            }
        }
    }
#endif
}