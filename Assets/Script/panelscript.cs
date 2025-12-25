using TMPro;
using UnityEngine;

public class panelscript : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text hikaye;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            panel.SetActive(!panel.activeSelf);
        }
        
    }
   
}
