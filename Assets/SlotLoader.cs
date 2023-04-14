using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject[] SlotButtons = new GameObject[3];
    [SerializeField]
    private int type;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            // check if save file exists in persistent data path
            string path = Application.persistentDataPath + "/save" + i + ".json";
            if (System.IO.File.Exists(path))
            {
                // if it does, load it
                string json = System.IO.File.ReadAllText(path);
                SaveState SaveSlot = new SaveState();
                SaveSlot = JsonUtility.FromJson<SaveState>(json);
                // set save slot button text to save title
                SlotButtons[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = SaveSlot.saveTitle;
            }
            else if (type == 1)
            {
                // Disable button
                SlotButtons[i].GetComponent<Button>().enabled = false;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
