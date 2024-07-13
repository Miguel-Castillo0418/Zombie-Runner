using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Objective : MonoBehaviour
{
    public GameObject ObjectiveTextObject;
    public TextMeshProUGUI ObjectiveText;
    [SerializeField] public string Description;
    [SerializeField] public bool Complete;
    public GameObject CompletedText;

    // Start is called before the first frame update
    void Start()
    {
        Complete = false;
        ObjectiveText = ObjectiveText.GetComponent<TextMeshProUGUI>();
        ObjectiveText.text = Description.ToString();
        ObjectiveTextObject.SetActive(false);
        CompletedText.SetActive(false);
        
    }
    

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ObjectiveTextObject.SetActive(true);
            CompletedText.SetActive(true);
        }
    }
}
