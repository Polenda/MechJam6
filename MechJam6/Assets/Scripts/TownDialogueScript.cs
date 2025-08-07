using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TownDialogueScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GlobalManagerScript globalManagerScript;
    [SerializeField] private TownManagerScript TownScript;
    [SerializeField] private Button travelButton;
    [SerializeField] private Button homeButton;

    [Header("Dialogue Settings")]
    [SerializeField] private Sprite[] NPCsprites;
    public SpriteRenderer NPCSpriteRenderer;
    public TextMeshProUGUI NPCDialogueText;
    public GameObject NPCDialoguePanel;
    public TextMeshProUGUI ChoiceLeftText;
    public TextMeshProUGUI ChoiceRightText;
    public Button ChoiceLeftButton;
    public Button ChoiceRightButton;

    [Header("Dialogue File Settings")]
    private DialogueFile currentDialogueFile;
    private DialogueEntry currentEntry;
    private int dialogueID;
    public string currentDialogueFileFileName;
    private string npcName;
    private int currentNPC;

    [Header("Click Settings")]
    [SerializeField] private InputActionAsset InputActions;
    private InputAction clickAction;
    public bool pass = false;
    public bool end = false;
    private bool temp;
    public ClosingScript closingScript;

    [System.Serializable]
    public class DialogueChoice
    {
        public string text;
        public int nextDialogueID;
        public string[] value; // Array of variable names
        public int[] change;   // Array of changes
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
        InputActions = TownScript.InputActions;
        clickAction = InputActions.FindAction("Click");
        NPCDialoguePanel.SetActive(false);
        NPCSpriteRenderer.enabled = false;
    }

    // Loads a dialogue file from Resources/Dialogue/filename.json
    private void LoadDialogueFile(string filename)
    {
        currentDialogueFileFileName = filename;
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
        if (currentDialogueFile == null) return;

        if (currentDialogueFileFileName == "encounterDialogue")
        {
            currentEntry = System.Array.Find(
                currentDialogueFile.dialogues,
                d => d.id == id && d.name == npcName
            );
        }
        else
        {
            currentEntry = System.Array.Find(currentDialogueFile.dialogues, d => d.id == id);
        }

        dialogueID = id;

        if (currentEntry == null)
        {
            EndDialogue();
            return;
        }

        NPCDialogueText.text = currentEntry.text;

        Debug.Log($"ShowDialogue: Looking for id={id}, name={npcName}. Found entry: {(currentEntry != null ? currentEntry.text : "null")}");

        if (currentEntry.choices != null)
        {
            if (currentEntry.choices.Length == 2)
            {
                ChoiceLeftText.text = currentEntry.choices[0].text;
                ChoiceRightText.text = currentEntry.choices[1].text;

                ChoiceLeftButton.onClick.RemoveAllListeners();
                ChoiceRightButton.onClick.RemoveAllListeners();

                ChoiceLeftButton.onClick.AddListener(() => OnChoiceSelected(0));
                ChoiceRightButton.onClick.AddListener(() => OnChoiceSelected(1));

                ChoiceLeftButton.interactable = true;
                ChoiceRightButton.interactable = true;

                // Check all values for left choice
                if (currentEntry.choices[0].value != null && currentEntry.choices[0].change != null)
                {
                    for (int i = 0; i < currentEntry.choices[0].value.Length; i++)
                    {
                        if (currentEntry.choices[0].value[i] == "waterAmount")
                        {
                            if ((globalManagerScript.waterAmount * -1) > currentEntry.choices[0].change[i])
                            {
                                ChoiceLeftButton.interactable = false;
                            }
                        }
                    }
                }
                // Check all values for right choice
                if (currentEntry.choices[1].value != null && currentEntry.choices[1].change != null)
                {
                    for (int i = 0; i < currentEntry.choices[1].value.Length; i++)
                    {
                        if (currentEntry.choices[1].value[i] == "waterAmount")
                        {
                            if ((globalManagerScript.waterAmount * -1) > currentEntry.choices[1].change[i])
                            {
                                ChoiceRightButton.interactable = false;
                            }
                        }
                    }
                }
            }
            else
            {
                ChoiceLeftButton.interactable = false;
                ChoiceRightButton.interactable = false;
            }
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
        if (choice.value != null && choice.change != null)
        {
            ApplyVariableChange(choice.value, choice.change);
        }
        if (choice.nextDialogueID == -1)
        {
            switch (currentNPC)
            {
                case 1: EndDialogue(); break;
                case 2: EndDialogue(); break;
                case 3: EndDialogue(); break;
                case 4: NewDialogue(); break;
                case 5: NewDialogue(); break;
                case 6: NewDialogue(); break;
            }
        }
        else
        {
            ShowDialogue(choice.nextDialogueID);
            // temp = false;
            // StartCoroutine(timer(temp));

        }

        if (globalManagerScript.mechHealth <= 0)
        {
            EndGameScript.Instance.myBool = 2;
            closingScript.ClosingGame();
        }

        Debug.Log($"OnChoiceSelected: {choice.text}, value: {(choice.value != null ? string.Join(",", choice.value) : "null")}, change: {(choice.change != null ? string.Join(",", choice.change) : "null")}");
    }

    private void ApplyVariableChange(string[] variables, int[] changes)
    {
        Debug.Log("changing variables called");
        for (int i = 0; i < variables.Length; i++)
        {
            string variable = variables[i];
            int change = changes[i];
            switch (variable)
            {
                case "mechHealth": globalManagerScript.mechHealth += change; break;
                case "waterAmount": globalManagerScript.waterAmount += change; break;
                case "maxWaterAmount": globalManagerScript.maxWaterAmount += change; break;
                case "townDriftBond": globalManagerScript.townDriftBond += change; break;
                case "townFlameBond": globalManagerScript.townFlameBond += change; break;
                case "townSaltBond": globalManagerScript.townSaltBond += change; break;
            }
            Debug.Log("Updated variable: " + variable);
        }
        
        globalManagerScript.Refresh();
    }

    private void EndDialogue()
    {
        ChoiceLeftButton.interactable = false;
        ChoiceRightButton.interactable = false;
        homeButton.interactable = true;
        travelButton.interactable = true;
        switch (currentNPC)
        {
            case 1: globalManagerScript.saltStory += dialogueID; break;
            case 2: globalManagerScript.flameStory += dialogueID; break;
            case 3: globalManagerScript.driftStory += dialogueID; break;
        }
            
    }
    private void NewDialogue()
    {
        NPCDialogueText.text = "";
        ChoiceLeftButton.interactable = false;
        ChoiceRightButton.interactable = false;
        switch (currentNPC)
        {
            case 4: globalManagerScript.npc1Story += dialogueID; break;
            case 5: globalManagerScript.npc2Story += dialogueID; break;
            case 6: globalManagerScript.npc3Story += dialogueID; break;
        }
            
        StartCoroutine(continueDialogue());
    }
    IEnumerator continueDialogue()
    {
        yield return StartCoroutine(TownScript.fadeScript.FadeToBlack());
        TownScript.encounter();
    }

    void Update()
    {
        if (clickAction.triggered && (pass || end))
        {


            if (currentEntry != null && currentEntry.choices != null && currentEntry.choices.Length == 1)
            {
                var choice = currentEntry.choices[0];
                if (choice.nextDialogueID == -1 && pass)
                {
                    pass = false; // Prevent multiple triggers
                    NewDialogue();
                    Debug.Log("Single choice dialogue ended.");
                }
                else if (choice.nextDialogueID == -1 && end)
                {
                    end = false; // Prevent multiple triggers
                    EndDialogue();
                    Debug.Log("Single choice dialogue will end on next click.");
                }
                else if (end)
                {
                    end = false; // Reset end to false after handling single choice
                    ShowDialogue(choice.nextDialogueID);
                    Debug.Log("Single choice advanced to next dialogue ID: " + choice.nextDialogueID + " end thing: " + end);
                    temp = false;
                    StartCoroutine(timer(temp));
                }
                else
                {
                    pass = false; // Reset pass to false after handling single choice
                    ShowDialogue(choice.nextDialogueID);
                    Debug.Log("Single choice advanced to next dialogue ID: " + choice.nextDialogueID);
                    temp = true;
                    StartCoroutine(timer(temp));
                }
            }
        }
        if (end && currentEntry.choices.Length == 1 && currentEntry.choices[0].nextDialogueID == -1)
        {
            end = false; // Reset end to false after handling single choice
            EndDialogue();
        }
        if (pass && currentEntry.choices.Length == 1 && currentEntry.choices[0].nextDialogueID == -1)
        {
            pass = false; // Reset pass to false after handling single choice
            NewDialogue();
        }
    }

    IEnumerator timer(bool thisTemp)
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Timer completed, setting end to true.");
        if (thisTemp)
        {
            pass = true;
        }
        else
        {
            end = true;
        }
    }

    // --- DIALOGUE ENTRY POINTS ---

    public void saltNPCDialogue()
    {
        currentNPC = 1;
        NPCSpriteRenderer.sprite = NPCsprites[0];
        LoadDialogueFile("saltDialogue"); // saltDialogue.json
        ShowDialogue(globalManagerScript.saltStory + 1);
    }
    public void flameNPCDialogue()
    {
        currentNPC = 2;
        NPCSpriteRenderer.sprite = NPCsprites[2];
        LoadDialogueFile("flameDialogue"); // flameDialogue.json
        ShowDialogue(globalManagerScript.flameStory + 1);
    }
    public void driftNPCDialogue()
    {
        currentNPC = 3;
        NPCSpriteRenderer.sprite = NPCsprites[1];
        LoadDialogueFile("driftDialogue"); // driftDialogue.json
        ShowDialogue(globalManagerScript.driftStory + 1);
    }
    public void encounterNPCDialogue(int num)
    {
        end = false;
        currentNPC = num+1;
        homeButton.interactable = false;
        travelButton.interactable = false;

        npcName = "NPC" + (num - 2);
        NPCSpriteRenderer.sprite = NPCsprites[num];
        LoadDialogueFile("encounterDialogue"); // encounterDialogue.json
        Debug.Log("Loading encounter dialogue for NPC: " + npcName + " with ID: " + num);
        switch (num)
        {
            case 3: ShowDialogue(globalManagerScript.npc1Story + 1); break;
            case 4: ShowDialogue(globalManagerScript.npc2Story + 1); break;
            case 5: ShowDialogue(globalManagerScript.npc3Story + 1); break;
            default: Debug.LogError("Invalid NPC number for encounter dialogue: " + num); break;
        }
    }
}
