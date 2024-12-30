#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using TMPro;
using Core3lb;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CoreHelperWindow : EditorWindow
{
    //Vars
    private Transform targetTransform;
    private BaseActivator baseActivator;
    private GameObject selectionReplacement;
    private Material selectedMat;
    private Material secondaryMat;
    private MonoScript selectedMonoScript;
    private Vector2 scrollPos;

    private bool useLocal = false;
    float buttonTallHeight = 20f; // You can change this value to increase or decrease button height
    // Adds a menu item to open the window
    [MenuItem("GroKit-Core/Helper Window")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CoreHelperWindow), false, "GroKit Helper");
    }

    public Transform followerTransform
    {
        get
        {
            if (Selection.activeGameObject)
            {
                return Selection.activeGameObject.transform;
            }
            else
            {
                return null;
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("GroKit Helper", EditorStyleExtensions.ColorBanner(EditorStyleExtensions.InfoBlue,Color.white));
        Docs();
        Creation();
        TransformTools();
        TransformMatch();
        SetActive();
        OffOnStart();
        Activators();
        Utility();

    }


    void TransformMatch()
    {
        //########################################################################## Transform Match
        GUILayout.Label("Transform Match", EditorStyleExtensions.FancyHeader());
        // Fields to set the target and follower transforms
        targetTransform = EditorGUILayout.ObjectField("Target Transform", targetTransform, typeof(Transform), true) as Transform;

        useLocal = EditorGUILayout.Toggle("useLocal", useLocal);
        bool hasValidTransforms = targetTransform != null && followerTransform != null;

        // Specify the desired button height
        // Buttons for Move, Parent, Rotate, and Scale in the same row with custom height
        if (GUILayout.Button("Set Target to Selection", GUILayout.Height(buttonTallHeight)))
        {
            if (Selection.activeGameObject)
            {
                targetTransform = Selection.activeGameObject.transform;
            }
        }
        EditorGUI.BeginDisabledGroup(!hasValidTransforms);
        GUILayout.BeginHorizontal(); // Start a horizontal group
        if (GUILayout.Button("Move", GUILayout.Height(buttonTallHeight)))
        {
            MoveToPosition();
        }
        if (GUILayout.Button("Parent", GUILayout.Height(buttonTallHeight)))
        {
            ParentToTarget();
        }
        if (GUILayout.Button("Rotate", GUILayout.Height(buttonTallHeight)))
        {
            RotateToTarget();
        }
        if (GUILayout.Button("Scale", GUILayout.Height(buttonTallHeight)))
        {
            ScaleTo();
        }

        GUILayout.EndHorizontal(); // End the horizontal group
        EditorGUI.EndDisabledGroup();
    }

    void TransformTools()
    {
        GUILayout.Label("Transform Tools", EditorStyleExtensions.FancyHeader());
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("RandomY", GUILayout.Height(buttonTallHeight)))
        {
            RandomY();
        }
        if (GUILayout.Button("Reset", GUILayout.Height(buttonTallHeight)))
        {
            ResetObject();
        }

        if (GUILayout.Button("Reset Local", GUILayout.Height(buttonTallHeight)))
        {
            ResetObject(true);
        }

        if (GUILayout.Button("Reset Parent", GUILayout.Height(buttonTallHeight)))
        {
            ResetParent();
        }

        if (GUILayout.Button("Align to View", GUILayout.Height(buttonTallHeight)))
        {
            MoveCameraToSceneView();
        }

        GUILayout.EndHorizontal();
    }

    void Creation()
    {
        // Label for Selection-Based Actions
        GUILayout.Label("Creation", EditorStyleExtensions.FancyHeader());
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Cube", GUILayout.Height(buttonTallHeight)))
        {
            CreatePrimitive(PrimitiveType.Cube);
        }

        if (GUILayout.Button("Sphere", GUILayout.Height(buttonTallHeight)))
        {
            CreatePrimitive(PrimitiveType.Sphere);
        }

        if (GUILayout.Button("TMP_TXT", GUILayout.Height(buttonTallHeight)))
        {
            CreateTMPText();
        }

        GUILayout.EndHorizontal();
    }

    void Activators()
    {
        GUILayout.Label("Activators", EditorStyleExtensions.FancyHeader());
        baseActivator = EditorGUILayout.ObjectField("Target Activator", baseActivator, typeof(BaseActivator), true) as BaseActivator;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Inward", GUILayout.Height(buttonTallHeight)))
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                obj.AddComponent<InwardActivator>();
            }
        }

        //// Disable buttons if no Activator is found on the selected GameObject
        if (GUILayout.Button("Set Activator", GUILayout.Height(buttonTallHeight)))
        {
            if (Selection.activeGameObject)
            {
                if (Selection.activeGameObject.TryGetComponent(out BaseActivator activator))
                {
                    baseActivator = activator;
                }
            }
        }

        if (GUILayout.Button("Add Outward", GUILayout.Height(buttonTallHeight)))
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                obj.AddComponent<OutwardActivator>();
            }
        }
        GUILayout.EndHorizontal();
        EditorGUI.BeginDisabledGroup(baseActivator == null);
        GUILayout.BeginHorizontal();

        //Single Target Buttons
        if (GUILayout.Button("Add To On", GUILayout.Height(buttonTallHeight)))
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                AddSetActiveToEvent(baseActivator.onEvent, obj, true);
            }
        }

        if (GUILayout.Button("Select Activator", GUILayout.Height(buttonTallHeight)))
        {
            Selection.activeGameObject = baseActivator.gameObject;
        }

        if (GUILayout.Button("Add To Off", GUILayout.Height(buttonTallHeight)))
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                AddSetActiveToEvent(baseActivator.offEvent, obj, false);
            }
        }
        EditorGUI.EndDisabledGroup();

        GUILayout.EndHorizontal();
    }

    void OffOnStart()
    {
        GUILayout.Label("Off On Start(_OOS)", EditorStyleExtensions.FancyHeader());
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Add _OOS", GUILayout.Height(buttonTallHeight)))
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                //This might need to be EndsWith
                if (!obj.name.Contains("_OOS"))
                {
                    obj.name += "_OOS";
                }
            }
        }

        if (GUILayout.Button("Set _OOS In Scene", GUILayout.Height(buttonTallHeight)))
        {
            // Display a confirmation dialog
            if (EditorUtility.DisplayDialog("Confirm Action",
                                            "Are you sure you want to set all '_OOS' objects in the scene as inactive?\n\nWARNING: This action is not undoable.",
                                            "Yes", "No"))
            {
                // Get all root objects in the active scene
                GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();

                // Iterate through all root objects and their children
                foreach (GameObject obj in allObjects)
                {
                    // Use a recursive function to search through the hierarchy
                    if (obj.name.EndsWith("_OOS"))
                    {
                        obj.SetActive(false);
                    }

                    foreach (Transform child in obj.GetComponentsInChildren<Transform>(true))
                    {
                        if (child.gameObject.name.EndsWith("_OOS"))
                        {
                            child.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        if (GUILayout.Button("Remove _OOS", GUILayout.Height(buttonTallHeight)))
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                if (obj.name.EndsWith("_OOS"))
                {
                    obj.name = obj.name.Replace("_OOS", "");
                }
            }
        }

        GUILayout.EndHorizontal();
    }

    void SetActive()
    {

        GUILayout.Label("Set Active", EditorStyleExtensions.FancyHeader());
        GUILayout.BeginHorizontal();


        if (GUILayout.Button("True", GUILayout.Height(buttonTallHeight)))
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                obj.SetActive(true);
            }
        }

        if (GUILayout.Button("Toggle", GUILayout.Height(buttonTallHeight)))
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                obj.SetActive(!obj.activeInHierarchy);
            }
        }

        if (GUILayout.Button("False", GUILayout.Height(buttonTallHeight)))
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                obj.SetActive(false);
            }
        }
        GUILayout.EndHorizontal();
    }

    public void Utility()
    {
        GUILayout.Label("Utility", EditorStyleExtensions.FancyHeader());

        selectedMat = EditorGUILayout.ObjectField("Primary Material", selectedMat, typeof(Material), true) as Material;
        secondaryMat = EditorGUILayout.ObjectField("Secondary Material", secondaryMat, typeof(Material), true) as Material;

        GUILayout.BeginHorizontal();

        //Single Target Buttons
        if (GUILayout.Button("Apply Primary", GUILayout.Height(buttonTallHeight)))
        {
            ApplyMaterials(selectedMat);
        }

        if (GUILayout.Button("Apply Secondary", GUILayout.Height(buttonTallHeight)))
        {
            ApplyMaterials(secondaryMat);
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Find Missing Scripts", GUILayout.Height(buttonTallHeight)))
        {
            SelectObjectsWithMissingScripts();
        }

        if (GUILayout.Button("Find Missing Mats", GUILayout.Height(buttonTallHeight)))
        {
            SelectObjectsWithNullMaterial();
        }
        GUILayout.EndHorizontal();

        selectedMonoScript = EditorGUILayout.ObjectField("Search Component ", selectedMonoScript, typeof(MonoScript), true) as MonoScript;
        if (GUILayout.Button("Select With Component", GUILayout.Height(buttonTallHeight)))
        {
            HighlightEverythingWithComponent();
        }
        selectionReplacement = EditorGUILayout.ObjectField("Replacement", selectionReplacement, typeof(GameObject), true) as GameObject;
        if (GUILayout.Button("Replace Selections", GUILayout.Height(buttonTallHeight))) // Adjust the width
        {
            ReplaceSelections();
        }

       
    }

    void Docs()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Docs", GUILayout.Height(buttonTallHeight)))
        {
            Application.OpenURL("https://3lbxr.notion.site/GroKit-Core-Landing-8914fe8fdde94b59af4b2337dd29ca49");
        }
        if (GUILayout.Button("GroKit Login", GUILayout.Height(buttonTallHeight)))
        {
            Application.OpenURL("https://realitylink.3lbxr.com/login");
        }

        if (GUILayout.Button("Help!", GUILayout.Height(buttonTallHeight)))
        {
            Application.OpenURL("https://3lbxr.notion.site/Helper-Window-15aacce4e6cb8022988bee745924bf68");
        }

        GUILayout.EndHorizontal();
    }

    public void MoveCameraToSceneView()
    {
        SceneView sceneView = SceneView.lastActiveSceneView;

        if (sceneView == null)
        {
            Debug.LogWarning("No active Scene view found.");
            return;
        }

        if (Selection.activeTransform == null)
        {
            Debug.LogWarning("No object selected. Please select an object in the hierarchy.");
            return;
        }
        // Get the target object's transform
        Transform selectedTransform = Selection.activeTransform;
        Undo.RecordObject(selectedTransform, "Camera Match");
        // Match position and rotation to the Scene view camera
        selectedTransform.position = sceneView.camera.transform.position;
        selectedTransform.rotation = sceneView.camera.transform.rotation;

        Debug.Log($"Moved {selectedTransform.name} to Scene view camera position.");
    }

    public void ResetParent()
    {
        if (EditorUtility.DisplayDialog("Reset Parent Confirmation",
            "This will set the parent object to default scale, while keeping the children at the same previous world scale.\n\n" +
            "WARNING: This action is not undoable. Do you wish to proceed?",
            "Yes", "No"))
        {
            foreach (Transform t in Selection.transforms)
            {
                List<Transform> children = new List<Transform>();
                var tempUndo = children;
                tempUndo.Add(t);
                for (int i = 0; i < t.childCount; i++)
                {
                    children.Add(t.GetChild(i));
                }
                foreach (Transform child in children)
                {
                    child.transform.parent = null;
                }
                t.transform.localScale = Vector3.one;
                foreach (Transform child in children)
                {
                    child.transform.parent = t;
                }
                EditorUtility.SetDirty(t);
            }
        }
    }

    private void HighlightEverythingWithComponent()
    {
        if (selectedMonoScript == null)
        {
            Debug.LogWarning("Please drag a MonoBehaviour script into the field to search for.");
            return;
        }

        // Get the type of the selected MonoBehaviour
        System.Type componentType = selectedMonoScript.GetClass();

        // Validate that the script is a MonoBehaviour
        if (componentType == null || !typeof(MonoBehaviour).IsAssignableFrom(componentType))
        {
            Debug.LogError("The selected script is not a valid MonoBehaviour.");
            return;
        }

        // Find all GameObjects in the scene, including inactive ones
        GameObject[] allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        // Prepare a list for selection
        var selectedObjects = new System.Collections.Generic.List<GameObject>();

        foreach (var gameObject in allGameObjects)
        {
            // Check if the GameObject is part of the current scene and not an asset
            if (gameObject.scene.isLoaded && gameObject.GetComponent(componentType) != null)
            {
                selectedObjects.Add(gameObject);
            }
        }

        // Set the selection in the editor
        Selection.objects = selectedObjects.ToArray();

        Debug.Log($"Highlighted {selectedObjects.Count} GameObjects with the component {componentType.Name}");
    }

    private void ReplaceSelections()
    {
        if (selectionReplacement == null)
        {
            Debug.LogError("No replacement GameObject selected!");
            return;
        }

        // Get all selected objects in the scene
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No objects selected!");
            return;
        }

        // Start an undo group
        Undo.IncrementCurrentGroup();
        int undoGroup = Undo.GetCurrentGroup();

        // Replace each selected object with the replacement
        foreach (GameObject selected in selectedObjects)
        {
            // Store the transform data of the current object
            Vector3 position = selected.transform.position;
            Quaternion rotation = selected.transform.rotation;
            Vector3 scale = selected.transform.localScale;

            // Record the deletion of the original object for undo
            Undo.DestroyObjectImmediate(selected);

            // Create a new instance of the replacement object
            GameObject newObject = PrefabUtility.InstantiatePrefab(selectionReplacement) as GameObject;
            if (newObject == null)
            {
                newObject = Instantiate(selectionReplacement);
                Undo.RegisterCreatedObjectUndo(newObject, "Replace Selection");
            }
            else
            {
                Undo.RegisterCreatedObjectUndo(newObject, "Replace Selection");
            }

            // Set the new object to match the original's transform
            newObject.transform.position = position;
            newObject.transform.rotation = rotation;
            newObject.transform.localScale = scale;
        }

        // Register the undo group
        Undo.CollapseUndoOperations(undoGroup);

        Debug.Log("Selections replaced and undo operation registered.");
    }

    public static void SelectObjectsWithMissingScripts()
    {
        // Get all GameObjects in the scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);
        // Create a list to hold the selected objects
        var selectedObjects = new System.Collections.Generic.List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Get all components on the object
            Component[] components = obj.GetComponents<Component>();

            // Iterate over the components to check for missing scripts
            foreach (Component component in components)
            {
                if (component == null)
                {
                    selectedObjects.Add(obj);
                    break; // Break to avoid duplicate additions
                }
            }
        }

        // Set the selection to the objects with missing scripts
        Selection.objects = selectedObjects.ToArray();

        Debug.Log($"Selected {selectedObjects.Count} object(s) with missing scripts.");
    }

    private void SelectObjectsWithNullMaterial()
    {
        // Get all GameObjects in the scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        // Create a list to hold the selected objects
        var selectedObjects = new System.Collections.Generic.List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            // Check if the object has a MeshRenderer and a null material
            if (renderer != null && HasNullMaterial(renderer))
            {
                selectedObjects.Add(obj);
            }
        }

        // Set the selection to the objects with null materials
        Selection.objects = selectedObjects.ToArray();

        Debug.Log($"Selected {selectedObjects.Count} object(s) with null materials.");
    }

    private bool HasNullMaterial(MeshRenderer renderer)
    {
        foreach (var mat in renderer.sharedMaterials)
        {
            if (mat == null)
                return true;
        }
        return false;
    }

    private List<GameObject> FindGameObjectsWithMissingScriptsInSelection()
    {
        List<GameObject> objectsWithMissingScripts = new List<GameObject>();

        // Get all selected GameObjects
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject obj in selectedObjects)
        {
            // Get all children, including the object itself
            Transform[] allTransforms = obj.GetComponentsInChildren<Transform>();

            foreach (Transform child in allTransforms)
            {
                // Get all components of the GameObject
                Component[] components = child.gameObject.GetComponents<Component>();

                foreach (Component component in components)
                {
                    if (component == null)
                    {
                        objectsWithMissingScripts.Add(child.gameObject);
                        break;
                    }
                }
            }
        }

        return objectsWithMissingScripts;
    }

    private void SelectAndCopyPrefabInstancesInScene()
    {
        // Get the selected prefab from the Project window
        GameObject selectedPrefab = Selection.activeObject as GameObject;

        if (selectedPrefab == null || PrefabUtility.GetPrefabAssetType(selectedPrefab) == PrefabAssetType.NotAPrefab)
        {
            Debug.LogWarning("Please select a valid prefab from the Project window.");
            return;
        }

        // Create a list to hold matching GameObjects in the scene
        var selectedInstances = new List<GameObject>();

        // Iterate through all GameObjects in the scene
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            // Check if the object is a prefab instance and matches the selected prefab
            GameObject prefabParent = PrefabUtility.GetCorrespondingObjectFromSource(obj);
            if (prefabParent == selectedPrefab)
            {
                selectedInstances.Add(obj);
            }
        }

        // Log a warning if no instances are found
        if (selectedInstances.Count == 0)
        {
            Debug.LogWarning($"No instances of the prefab '{selectedPrefab.name}' were found in the scene.");
            return;
        }

        // Create new prefab instances at the positions and rotations of the found instances
        Undo.IncrementCurrentGroup(); // Start a new undo group
        Undo.SetCurrentGroupName("Copy Prefab Instances"); // Give the undo action a name

        foreach (GameObject instance in selectedInstances)
        {
            // Create a new instance of the prefab
            GameObject newInstance = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);

            // Copy position and rotation
            newInstance.transform.position = instance.transform.position;
            newInstance.transform.rotation = instance.transform.rotation;
            newInstance.transform.localScale = instance.transform.localScale; // Preserve scale as well

            // Register the new instance for undo
            Undo.RegisterCreatedObjectUndo(newInstance, "Create Prefab Instance");
        }

        // Select the original instances in the scene
        Selection.objects = selectedInstances.ToArray();

        Debug.Log($"Copied and selected {selectedInstances.Count} instance(s) of the prefab '{selectedPrefab.name}' in the scene.");
    }

    private void ApplyMaterials(Material myMat)
    {
        if(myMat == null)
        {
            Debug.LogError("Material is Set to null this will create Magenta");
        }
        foreach (GameObject obj in Selection.gameObjects)
        {
            if (obj.gameObject.TryGetComponent(out MeshRenderer myRender))
            {
                Undo.RecordObject(myRender, "Change Material");

                // Change the material
                myRender.material = myMat;

                // Mark the object as dirty to ensure changes are saved
                EditorUtility.SetDirty(myRender);
            }
        }
    }

    private void MoveToPosition()
    {
        if (targetTransform != null && followerTransform != null)
        {
            foreach (Transform t in Selection.transforms)
            {
                Undo.RecordObject(t, "Move To Position");
                if(useLocal)
                {
                    t.localPosition = targetTransform.localPosition;
                }
                else
                {
                    t.position = targetTransform.position;
                }
                EditorUtility.SetDirty(t);
            }
        }
    }

    private void ParentToTarget()
    {
        if (targetTransform != null && followerTransform != null)
        {
            foreach (Transform t in Selection.transforms)
            {
                Undo.SetTransformParent(t, targetTransform, "Parent To Target");
                EditorUtility.SetDirty(t);
            }
        }
    }

    private void RotateToTarget()
    {
        if (targetTransform != null && followerTransform != null)
        {
            foreach (Transform t in Selection.transforms)
            {
                Undo.RecordObject(t, "Rotate To Target");
                if (useLocal)
                {
                    t.rotation = targetTransform.rotation;
                }
                else
                {
                    t.rotation = targetTransform.rotation;
                }

                EditorUtility.SetDirty(t);
            }
        }
    }

    private void ScaleTo()
    {
        if (targetTransform != null && followerTransform != null)
        {
            foreach (Transform t in Selection.transforms)
            {
                Undo.RecordObject(t, "Rotate To Target");
                if (useLocal)
                {
                    t.localScale = targetTransform.localScale;
                }
                else
                {
                    t.localScale = targetTransform.lossyScale;
                }

                EditorUtility.SetDirty(t);
            }
        }
    }

    public void ResetObject(bool isLocal = false)
    {
        foreach (Transform t in Selection.transforms)
        {
            Undo.RecordObject(t,"Reset Object");

            if(isLocal)
            {
                t.transform.localPosition = Vector3.zero;
                t.transform.localPosition = Vector3.zero;
            }
            else
            {
                t.transform.position = Vector3.zero;
                t.transform.rotation = Quaternion.identity;
                t.transform.localScale = Vector3.one;
            }
            EditorUtility.SetDirty(t);
        }
    }

    public void RandomY()
    {
        foreach (Transform t in Selection.transforms)
        {
            Undo.RecordObject(t, "Rotate To Target");

            // Calculate the rotation to match the target transform
            Quaternion myRotation = useLocal ? t.localRotation : t.rotation;

            // Add random rotation to the Y-axis
            float randomY = Random.Range(-180f, 180f); // Random angle between -180 and 180 degrees
            Quaternion randomYRotation = Quaternion.Euler(0, randomY, 0);

            if (useLocal)
            {
                t.localRotation = myRotation * randomYRotation;
            }
            else
            {
                t.rotation = myRotation * randomYRotation;
            }

            EditorUtility.SetDirty(t);
        }
    }

    private void CreateEmptyGameObject()
    {
        GameObject newObj = new GameObject("Empty GameObject");
        PositionAtSceneView(newObj);
        Undo.RegisterCreatedObjectUndo(newObj, "Create Empty GameObject");
        Selection.activeGameObject = newObj;
    }

    private void CreateEmptyChild()
    {
        if (Selection.activeTransform != null)
        {
            GameObject newChild = new GameObject("Empty Child");
            newChild.transform.parent = Selection.activeTransform;
            newChild.transform.localPosition = Vector3.zero;
            Undo.RegisterCreatedObjectUndo(newChild, "Create Empty Child");
            Selection.activeGameObject = newChild;
        }
    }

    private void CreateEmptyParent()
    {
        if (Selection.transforms.Length > 0)
        {
            // Get the shared parent of all selected objects
            Transform sharedParent = Selection.transforms[0].parent;
            foreach (Transform selected in Selection.transforms)
            {
                if (selected.parent != sharedParent)
                {
                    sharedParent = null; // If different parents are found, set to null (no shared parent)
                    break;
                }
            }

            // Create a new empty parent GameObject
            GameObject parentObject = new GameObject("Empty Parent");
            Undo.RegisterCreatedObjectUndo(parentObject, "Create Empty Parent");

            // Set the position to the average position of selected objects
            Vector3 averagePosition = Vector3.zero;
            foreach (Transform selected in Selection.transforms)
            {
                averagePosition += selected.position;
            }
            parentObject.transform.position = averagePosition / Selection.transforms.Length;

            // Assign shared parent if it exists
            if (sharedParent != null)
            {
                parentObject.transform.SetParent(sharedParent, true);
            }

            // Parent all selected transforms to the new parent
            foreach (Transform selected in Selection.transforms)
            {
                Undo.SetTransformParent(selected, parentObject.transform, "Parent to Empty Parent");
            }

            Selection.activeGameObject = parentObject;
        }
    }

    private void CreatePrimitive(PrimitiveType type)
    {
        GameObject cube = GameObject.CreatePrimitive(type);
        PositionAtSceneView(cube);
        Undo.RegisterCreatedObjectUndo(cube, "Create Cube");
        Selection.activeGameObject = cube;
    }



    private void AddFXSpawner()
    {
        GameObject sfxEvent = new GameObject("FXEvent");
        sfxEvent.AddComponent<FXSpawner>();
        if (Selection.activeTransform != null)
        {
            sfxEvent.transform.position = Selection.activeTransform.position;
            sfxEvent.transform.SetParent(Selection.activeTransform, true);
        }
        else
        {
            PositionAtSceneView(sfxEvent);
        }

        Undo.RegisterCreatedObjectUndo(sfxEvent, "Created SFXEvent");
        Selection.activeGameObject = sfxEvent;
    }

    private void CreateSFXEvent()
    {
        GameObject sfxEvent = new GameObject("SFXEvent");
        sfxEvent.AddComponent<SFXEvent>();
        if (Selection.activeTransform != null)
        {
            sfxEvent.transform.position = Selection.activeTransform.position;
            sfxEvent.transform.SetParent(Selection.activeTransform, true);
        }
        else
        {
            PositionAtSceneView(sfxEvent);
        }

        Undo.RegisterCreatedObjectUndo(sfxEvent, "Created SFXEvent");
        Selection.activeGameObject = sfxEvent;
    }

    private void CreateTMPText()
    {
        GameObject tmpTextObj = new GameObject("TMP Text");
        TextMeshPro tmpText = tmpTextObj.AddComponent<TextMeshPro>();
        tmpText.text = "New Text";

        if (Selection.activeTransform != null)
        {
            tmpTextObj.transform.position = Selection.activeTransform.position;
            tmpTextObj.transform.SetParent(Selection.activeTransform, true);
        }
        else
        {
            PositionAtSceneView(tmpTextObj);
        }

        Undo.RegisterCreatedObjectUndo(tmpTextObj, "Create TMP Text");
        Selection.activeGameObject = tmpTextObj;
    }

    // Position the GameObject at the Scene View if no selection exists
    private void PositionAtSceneView(GameObject obj)
    {
        if (Selection.activeTransform != null)
        {
            obj.transform.position = Selection.activeTransform.position;
        }
        else
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            if (sceneView != null)
            {
                obj.transform.position = sceneView.camera.transform.position + sceneView.camera.transform.forward * 5f;
            }
        }
    }

    /// <summary>
    /// ############################################################# ACTIVATOR SYSTEM ####################################
    /// ###################################################################################################################
    /// </summary>

    /// <summary>
    /// Adds SetActive calls to the UnityEvent for a list of GameObjects.
    /// </summary>
    /// <param name="unityEvent"></param>
    /// <param name="targets"></param>
    /// <param name="value"></param>
    private void AddObjectsToEvent(UnityEvent unityEvent, List<GameObject> targets, bool value)
    {
        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                AddSetActiveToEvent(unityEvent, target, value);
            }
        }
    }

    /// <summary>
    /// Adds the SetActive function with the specified value to a UnityEvent.
    /// </summary>
    /// <param name="unityEvent">The UnityEvent to modify.</param>
    /// <param name="targetObject">The target GameObject to set active.</param>
    /// <param name="value">The active state to set (true/false).</param>
    private void AddSetActiveToEvent(UnityEvent unityEvent, GameObject targetObject, bool value)
    {
        if (unityEvent == null || targetObject == null)
        {
            Debug.LogError("Invalid parameters for adding SetActive to UnityEvent.");
            return;
        }

        // Serialize the Activator to modify its UnityEvent
        SerializedObject activatorSerializedObject = new SerializedObject(baseActivator);
        SerializedProperty eventProperty = null;

        // Determine which event we're modifying
        if (unityEvent == baseActivator.onEvent)
        {
            eventProperty = activatorSerializedObject.FindProperty("onEvent");
        }
        else if (unityEvent == baseActivator.offEvent)
        {
            eventProperty = activatorSerializedObject.FindProperty("offEvent");
        }
        else
        {
            Debug.LogError("Unknown UnityEvent.");
            return;
        }

        // Add a new listener to the event
        int index = eventProperty.FindPropertyRelative("m_PersistentCalls.m_Calls").arraySize;
        eventProperty.FindPropertyRelative("m_PersistentCalls.m_Calls").InsertArrayElementAtIndex(index);
        SerializedProperty call = eventProperty.FindPropertyRelative("m_PersistentCalls.m_Calls").GetArrayElementAtIndex(index);

        // Set the target object
        call.FindPropertyRelative("m_Target").objectReferenceValue = targetObject;

        // Set the method name
        call.FindPropertyRelative("m_MethodName").stringValue = "SetActive";

        // Set the mode to accept a boolean argument
        call.FindPropertyRelative("m_Mode").enumValueIndex = (int)PersistentListenerMode.Bool;

        // Set the call state to RuntimeOnly
        call.FindPropertyRelative("m_CallState").enumValueIndex = (int)UnityEngine.Events.UnityEventCallState.RuntimeOnly;

        // Set the argument
        SerializedProperty arguments = call.FindPropertyRelative("m_Arguments");
        arguments.FindPropertyRelative("m_ObjectArgument").objectReferenceValue = null;
        arguments.FindPropertyRelative("m_ObjectArgumentAssemblyTypeName").stringValue = string.Empty;
        arguments.FindPropertyRelative("m_IntArgument").intValue = 0;
        arguments.FindPropertyRelative("m_FloatArgument").floatValue = 0f;
        arguments.FindPropertyRelative("m_StringArgument").stringValue = string.Empty;
        arguments.FindPropertyRelative("m_BoolArgument").boolValue = value;

        // Apply the modified properties
        activatorSerializedObject.ApplyModifiedProperties();

        // Mark the Activator as dirty to ensure it saves
        EditorUtility.SetDirty(baseActivator);

        //Debug.Log($"Added SetActive({value}) for {targetObject.name} to the UnityEvent with RuntimeOnly call state.");
    }
}
#endif