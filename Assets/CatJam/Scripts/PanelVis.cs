using UnityEngine;

public class PanelVis : MonoBehaviour
{

    [SerializeField] private GameObject nextPanel;
    [SerializeField] PlayerEmote playerEmote; // Ayarlar paneli
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerEmote.stress >= playerEmote.maxStress)
        {
            nextPanel.SetActive(true);
        }
    }
}
