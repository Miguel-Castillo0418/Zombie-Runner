using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ObjectiveComplete : MonoBehaviour
{
    public bool Complete;
    public string TextComplete;
    public TextMeshProUGUI Text;
    [SerializeField] private GameObject key;
    [SerializeField] private Transform keyPos;
    private bool hasSpawnKey = false;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (this.name == "ObjectiveComplete (Puzzle)" && !hasSpawnKey)
            {
                Instantiate(key, keyPos.position, Quaternion.identity);
                hasSpawnKey = true;
            }
            Complete = true;
            Text.text = TextComplete.ToString();
            StartCoroutine(WaitForSec());
        }
    }
    void Start()
    {
        Text = Text.GetComponent<TextMeshProUGUI>();
    }
    public IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(3);
        DestroyImmediate(Text);
    }


}



