using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TownDialogueScript : MonoBehaviour
{
    [SerializeField] private GlobalManagerScript globalManagerScript;
    [SerializeField] private Sprite[] NPCsprites;
    public SpriteRenderer NPCSpriteRenderer;
    public TextMeshProUGUI NPCDialogueText;
    public GameObject NPCDialoguePanel;
    public TextMeshProUGUI ChoiceLeftText;
    public TextMeshProUGUI ChoiceRightText;
    public Button ChoiceLeftButton;
    public Button ChoiceRightButton;

    private DialogueFile currentDialogueFile;
    private DialogueEntry currentEntry;

    [System.Serializable]
    public class DialogueChoice
    {
        public string text;
        public int nextDialogueID;
        public string value;
        public int change;
    }

    [System.Serializable]
    public class DialogueEntry
    {
        public string name;
        public int id;
        public string text;
        public DialogueChoice[] choices;
    }

    [System.Serializable]
    public class DialogueFile
    {
        public DialogueEntry[] dialogues;
    }

    void Start()
    {
        NPCDialoguePanel.SetActive(false);
        NPCSpriteRenderer.enabled = false;
    }

    // Loads a dialogue file from Resources/Dialogue/filename.json
    private void LoadDialogueFile(string filename)
    {
        string path = Path.Combine(Application.dataPath, "Dialogue", filename + ".json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            currentDialogueFile = JsonUtility.FromJson<DialogueFile>(json);
        }
        else
        {
            Debug.LogError("Dialogue file not found: " + path);
        }
    }

    private void ShowDialogue(int id)
    {
        NPCDialoguePanel.SetActive(true);
        if (currentDialogueFile == null) return;
        currentEntry = System.Array.Find(currentDialogueFile.dialogues, d => d.id == id);
        if (currentEntry == null)
        {
            EndDialogue();
            return;
        }

        NPCDialogueText.text = currentEntry.text;

        if (currentEntry.choices != null)
        {
            ChoiceLeftText.text = currentEntry.choices[0].text;
            ChoiceRightText.text = currentEntry.choices[1].text;

            ChoiceLeftButton.onClick.RemoveAllListeners();
            ChoiceRightButton.onClick.RemoveAllListeners();

            ChoiceLeftButton.onClick.AddListener(() => OnChoiceSelected(0));
            ChoiceRightButton.onClick.AddListener(() => OnChoiceSelected(1));

            ChoiceLeftButton.interactable = true;
            ChoiceRightButton.interactable = true;
        }
        else
        {
            ChoiceLeftButton.interactable = false;
            ChoiceRightButton.interactable = false;
        }
    }

    private void OnChoiceSelected(int index)
    {
        var choice = currentEntry.choices[index];
        if (!string.IsNullOrEmpty(choice.value))
        {
            ApplyVariableChange(choice.value, choice.change);
        }
        if (choice.nextDialogueID == -1)
        {
            EndDialogue();
        }
        else
        {
            ShowDialogue(choice.nextDialogueID);
        }
    }

    private void ApplyVariableChange(string variable, int change)
    {
        switch (variable)
        {
            case "mechHealth": globalManagerScript.mechHealth += change; break;
            case "waterAmount": globalManagerScript.waterAmount += change; break;
            case "maxWaterAmount": globalManagerScript.maxWaterAmount += change; break;
            case "townDriftBond": globalManagerScript.townDriftBond += change; break;
            case "townFlameBond": globalManagerScript.townFlameBond += change; break;
            case "townSaltBond": globalManagerScript.townSaltBond += change; break;
            case "npc1Story": globalManagerScript.npc1Story += change; break;
        }
        globalManagerScript.Refresh();
    }

    private void EndDialogue()
    {
        NPCDialogueText.text = "";
        ChoiceLeftButton.interactable = false;
        ChoiceRightButton.interactable = false;
    }

    // --- DIALOGUE ENTRY POINTS ---

    public void saltNPCDialogue()
    {
        NPCSpriteRenderer.sprite = NPCsprites[0];
        LoadDialogueFile("saltDialogue"); // saltDialogue.json
        ShowDialogue(1);
    }
    public void flameNPCDialogue()
    {
        NPCSpriteRenderer.sprite = NPCsprites[1];
        LoadDialogueFile("flameDialogue"); // flameDialogue.json
        ShowDialogue(1);
    }
    public void driftNPCDialogue()
    {
        NPCSpriteRenderer.sprite = NPCsprites[2];
        LoadDialogueFile("driftDialogue"); // driftDialogue.json
        ShowDialogue(1);
    }
    public void homeNPCDialogue()
    {
        LoadDialogueFile("homeDialogue"); // homeDialogue.json
        ShowDialogue(1);
    }
    public void encounterNPCDialogue()
    {
        int num = Random.Range(3, 6);
        NPCSpriteRenderer.sprite = NPCsprites[num];
        LoadDialogueFile("encounterDialogue"); // encounterDialogue.json
        ShowDialogue(1);
    }
}


