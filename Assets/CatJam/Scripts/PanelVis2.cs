using UnityEngine;

public class PanelVis2 : MonoBehaviour
{

    [SerializeField] private GameObject nextPanel;
    [SerializeField] PlayerHealth playerHealth; // Ayarlar paneli
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.currentHealth <= 0)
        {
            nextPanel.SetActive(true);
        }
    }
}
