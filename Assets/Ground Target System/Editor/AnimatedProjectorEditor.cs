using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(AnimatedProjector))]
public class AnimatedProjectorEditor : Editor
{
    // The default location for the prefab texture assets.
    // If the location changes these paths must be updated!
    private const string circleAsset = "Assets/Ground Target System/Textures/circle_1d.png";
    private const string falloffAsset = "Assets/Ground Target System/Textures/Falloff.psd";

    private ReorderableList colorList;

    private float lastPulseMinValue = 0;
    private float lastPulseMaxValue = 0;

    public void OnEnable()
    {
        colorList = new ReorderableList(serializedObject,
                        serializedObject.FindProperty("colors"),
                        true, true, true, true);

        colorList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = colorList.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;

                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, EditorGUIUtility.currentViewWidth - 60, EditorGUIUtility.singleLineHeight),
                    element, GUIContent.none);
            };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        AnimatedProjector projector = (AnimatedProjector)target;
        if (projector != null)
        {
            EditorGUILayout.Separator();

            SerializedProperty texture = serializedObject.FindProperty("texture");
            if (texture != null)
            {
                EditorGUILayout.PropertyField(texture, new GUIContent("Default Texture"));

                if (projector.DefaultTexture != projector.texture)
                    projector.DefaultTexture = projector.texture;

            }

            projector.color = EditorGUILayout.ColorField("Default Color", projector.color);

            if (projector.DefaultColor != projector.color)
                projector.DefaultColor = projector.color;

            if (!projector.pulse)
                projector.DefaultSize = EditorGUILayout.Slider("Default Size", projector.DefaultSize, 0.1f, 2.0f);

            projector.rotation = EditorGUILayout.Slider("Rotation", projector.rotation, 0, 259);
            projector.Refresh();

            EditorGUILayout.Separator();
            GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
            EditorGUILayout.Separator();

            projector.rotate = EditorGUILayout.Toggle("Auto Rotate", projector.rotate);
            if (projector.rotate)
            {
                EditorGUI.indentLevel++;
                projector.rotationSpeed = EditorGUILayout.Slider("Speed", projector.rotationSpeed, -1, 1);
                EditorGUI.indentLevel--;

                EditorGUILayout.Separator();
                GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
                EditorGUILayout.Separator();
            }

            projector.pulse = EditorGUILayout.Toggle("Pulse", projector.pulse);
            if (projector.pulse)
            {
                EditorGUI.indentLevel++;
                projector.pulseLoop = EditorGUILayout.Toggle("Loop", projector.pulseLoop);
                projector.pulseSpeed = EditorGUILayout.Slider("Speed", projector.pulseSpeed, 0.0f, 1.0f);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.MinMaxSlider(new GUIContent("Range"), ref projector.pulseMin, ref projector.pulseMax, 0, 1);

                if (lastPulseMinValue != projector.pulseMin)
                {
                    projector.DefaultSize = projector.pulseMin;
                }
                else if (lastPulseMaxValue != projector.pulseMax)
                {
                    projector.DefaultSize = projector.pulseMax;
                }

                lastPulseMinValue = projector.pulseMin;
                lastPulseMaxValue = projector.pulseMax;

                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel--;

                EditorGUILayout.Separator();
                GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
                EditorGUILayout.Separator();
            }

            projector.colorBlend = EditorGUILayout.Toggle("Color Blend", projector.colorBlend);
            if (projector.colorBlend)
            {
                EditorGUI.indentLevel++;
                projector.colorSpeed = EditorGUILayout.Slider("Speed", projector.colorSpeed, 0.0f, 1.0f);

                EditorGUILayout.Separator();

                colorList.DoLayoutList();

                EditorGUI.indentLevel--;

                EditorGUILayout.Separator();
                GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
                EditorGUILayout.Separator();
            }


            if (GUI.changed)
                EditorUtility.SetDirty(projector);
        }

        serializedObject.ApplyModifiedProperties();
    }

    [MenuItem("GameObject/Game Native/Ground Targets/Create", false, 20)]
    public static void CreateGroundTarget()
    {
        string fullpath = EditorUtility.SaveFilePanelInProject("Save Ground Target", "GroundTarget.prefab", "prefab", "Please provide a filename for the Ground Target:");
        string[] parts = fullpath.Split('/');

        if (!string.IsNullOrEmpty(fullpath) && parts.Length > 0)
        {
            string filename = parts[parts.Length - 1];
            string basename = filename.Remove(filename.LastIndexOf('.'));
            string path = fullpath.Remove(fullpath.LastIndexOf(filename));

            string mat1_path = string.Format("{0}{1}_1.mat", path, basename);
            string mat2_path = string.Format("{0}{1}_2.mat", path, basename);
            string mat3_path = string.Format("{0}{1}_3.mat", path, basename);
            string mat4_path = string.Format("{0}{1}_4.mat", path, basename);

            // Destroy Existing
            AssetDatabase.DeleteAsset(mat1_path);
            AssetDatabase.DeleteAsset(mat2_path);
            AssetDatabase.DeleteAsset(mat3_path);
            AssetDatabase.DeleteAsset(mat4_path);

            AssetDatabase.DeleteAsset(fullpath);
            AssetDatabase.Refresh();

            // Create Materials
            Material mat1 = new Material(Shader.Find("Projector/LightCustom"));
            mat1.SetColor("_Color", Color.yellow);
            mat1.SetTexture("_ShadowTex", AssetDatabase.LoadAssetAtPath<Texture>(circleAsset));
            mat1.SetTexture("_FalloffTex", AssetDatabase.LoadAssetAtPath<Texture>(falloffAsset));

            AssetDatabase.CreateAsset(mat1, mat1_path);

            Material mat2 = new Material(Shader.Find("Projector/LightCustom"));
            mat2.SetColor("_Color", Color.green);
            mat2.SetTexture("_ShadowTex", AssetDatabase.LoadAssetAtPath<Texture>(circleAsset));
            mat2.SetTexture("_FalloffTex", AssetDatabase.LoadAssetAtPath<Texture>(falloffAsset));

            AssetDatabase.CreateAsset(mat2, mat2_path);

            Material mat3 = new Material(Shader.Find("Projector/LightCustom"));
            mat3.SetColor("_Color", Color.blue);
            mat3.SetTexture("_ShadowTex", AssetDatabase.LoadAssetAtPath<Texture>(circleAsset));
            mat3.SetTexture("_FalloffTex", AssetDatabase.LoadAssetAtPath<Texture>(falloffAsset));

            AssetDatabase.CreateAsset(mat3, mat3_path);

            Material mat4 = new Material(Shader.Find("Projector/LightCustom"));
            mat4.SetColor("_Color", Color.red);
            mat4.SetTexture("_ShadowTex", AssetDatabase.LoadAssetAtPath<Texture>(circleAsset));
            mat4.SetTexture("_FalloffTex", AssetDatabase.LoadAssetAtPath<Texture>(falloffAsset));

            AssetDatabase.CreateAsset(mat4, mat4_path);

            // Build the ground target prefab.
            GameObject groundTarget = new GameObject();
            groundTarget.name = basename;

            // Target 1
            GameObject target1 = new GameObject("Projector_1");
            target1.transform.SetParent(groundTarget.transform);
            target1.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));

            AnimatedProjector projector1 = target1.AddComponent<AnimatedProjector>();
            projector1.Initialize();
            projector1.SetMaterial(mat1);
            projector1.texture = projector1.DefaultTexture;
            projector1.color = projector1.DefaultColor;
            projector1.DefaultSize = 0.345f;

            // Target 2
            GameObject target2 = new GameObject("Projector_2");
            target2.transform.SetParent(groundTarget.transform);
            target2.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));

            AnimatedProjector projector2 = target2.AddComponent<AnimatedProjector>();
            projector2.Initialize();
            projector2.SetMaterial(mat2);
            projector2.texture = projector2.DefaultTexture;
            projector2.color = projector2.DefaultColor;
            projector2.DefaultSize = 0.415f;

            // Target 3
            GameObject target3 = new GameObject("Projector_3");
            target3.transform.SetParent(groundTarget.transform);
            target3.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));

            AnimatedProjector projector3 = target3.AddComponent<AnimatedProjector>();
            projector3.Initialize();
            projector3.SetMaterial(mat3);
            projector3.texture = projector3.DefaultTexture;
            projector3.color = projector3.DefaultColor;
            projector3.DefaultSize = 0.5f;

            // Target 4
            GameObject target4 = new GameObject("Projector_4");
            target4.transform.SetParent(groundTarget.transform);
            target4.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));

            AnimatedProjector projector4 = target4.AddComponent<AnimatedProjector>();
            projector4.Initialize();
            projector4.SetMaterial(mat4);
            projector4.texture = projector4.DefaultTexture;
            projector4.color = projector4.DefaultColor;
            projector4.DefaultSize = 0.6f;


            PrefabUtility.CreatePrefab(fullpath, groundTarget);
            DestroyImmediate(groundTarget);

            // Refresh the assets
            AssetDatabase.Refresh(ImportAssetOptions.Default);
        }
    }
}