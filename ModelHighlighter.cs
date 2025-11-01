using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class ModelHighlighter :MonoBehaviour
{
    public GameObjectparentObject; // Parent containing all models
    public GameObject[] models; // Array of models
    public Button[] buttons; // Buttons for interaction
    public AudioSource[] sounds; // Sounds linked to models
    public float blinkDuration = 2f; // Duration of blinking effect
    public float blinkInterval = 0.3f; // Time interval between blinks
    public Color blinkColor = Color.blue; // Blink color
    public Vector3 popOutOffset = new Vector3(0, 0.2f, 0); // Pop-out effect offset
    private Dictionary<GameObject, Coroutine>blinkCoroutines = new Dictionary<GameObject, Coroutine>();
    private Dictionary<GameObject, Color>originalColors = new Dictionary<GameObject, Color>();
    private Dictionary<GameObject, Vector3>originalPositions = new Dictionary<GameObject, Vector3>(); 
    private Dictionary<GameObject, int>tapCounts = new Dictionary<GameObject, int>(); 
    private void Start()
    {
        foreach (GameObject model in models)
        {
            if (model.TryGetComponent<Renderer>(out Renderer renderer))
            {
originalColors[model] = renderer.material.color;
            }
            // Store original position and tap count of each model
originalPositions[model] = model.transform.position;
tapCounts[model] = 0;
            // Add click event to models
model.AddComponent<BoxCollider>(); // Ensure collider is present
model.AddComponent<ModelClickHandler>().Initialize(this, model);
        }
        // Assign button click listeners
        for (int i = 0; i<buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() =>HandleButtonPress(models[index]));
        }
    }
    public void HandleButtonPress(GameObject model)
    {
        if (!parentObject.activeSelf)
        {
parentObject.SetActive(true);
        }
         int index = System.Array.IndexOf(models, model);
    if (index == -1) return; // Ensure model exists
    // Play the associated sound
    if (sounds.Length> index && sounds[index] != null)
    {
        sounds[index].Play();
    }
tapCounts[model]++;
        switch (tapCounts[model])
        {
            case 1:
StartBlinking(model);
                break;
            case 2:
MoveModel(model, popOutOffset); // Move forward
                break;
            case 3:
MoveModel(model, -popOutOffset); // Move backward to original position
tapCounts[model] = 0; // Reset count for next interaction
                break;
        }
    }
    private void StartBlinking(GameObject model)
    {
        if (blinkCoroutines.ContainsKey(model) &&blinkCoroutines[model] != null)
        {
StopCoroutine(blinkCoroutines[model]);
        }
blinkCoroutines[model] = StartCoroutine(BlinkModel(model));
    }
    private IEnumeratorBlinkModel(GameObject model)
    {
Renderer[] renderers = model.GetComponentsInChildren<Renderer>();
        float elapsedTime = 0f;
        bool isBlue = false;
        while (elapsedTime<blinkDuration)
        {
            foreach (Renderer renderer in renderers)
            {
renderer.material.color = isBlue ?blinkColor :originalColors[model];
            }
isBlue= !isBlue;
            yield return new WaitForSeconds(blinkInterval);
elapsedTime += blinkInterval;
        }
        ResetColor(model);
    }
    private void ResetColor(GameObject model)
    {
Renderer[] renderers = model.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
renderer.material.color = originalColors[model];
        }
    }
    private void MoveModel(GameObject model, Vector3 offset)
    {
model.transform.position += offset;
    }
    public void TogglePopOutModel(GameObject model)
    {
HandleButtonPress(model);
    }
}
// Click Handler Component
public class ModelClickHandler :MonoBehaviour
{
    private ModelHighlighter highlighter;
    private GameObject model;
    public void Initialize(ModelHighlighter highlighter, GameObject model)
    {
this.highlighter = highlighter;
this.model = model;
    }
    private void OnMouseDown()
    {
highlighter.TogglePopOutModel(model);
    }
}

