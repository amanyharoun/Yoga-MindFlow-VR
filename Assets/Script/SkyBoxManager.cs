using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkyBoxManager : MonoBehaviour
{
    public Material[] skyboxes;
    public Sprite[] images;
    public int currentMaterial;
    public GameObject skyboxButtonPrefab;
    public Transform buttonParent;
    void Start()
    {
        for (int i = 0; i < skyboxes.Length; i++)
        {
            int ind = i;
            GameObject sb = Instantiate(skyboxButtonPrefab, buttonParent);
            sb.GetComponent<Image>().sprite = images[ind];
            sb.GetComponent<Button>().onClick.AddListener(() => { ChangeSkyboxByIndex(ind); });
            sb.GetComponentInChildren<TextMeshProUGUI>().text = skyboxes[ind].name = "";
        }
    }

    public void ChangeSkyboxByIndex(int index)
    {
        ChangeSkybox(skyboxes[index]);
    }
    void ChangeSkybox(Material skyboxMaterial)
    {
        RenderSettings.skybox = skyboxMaterial;

        // If the skybox uses procedural shaders or reflections, update them
        DynamicGI.UpdateEnvironment();
    }

}
