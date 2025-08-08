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

    private string currentWeekName;

    [Header("Click Settings")]
    [SerializeField] private InputActionAsset InputActions;
    private InputAction clickAction;
    public bool pass = false;
    public bool end = false;
    private bool temp;
    public ClosingScript closingScript;
    public bool doOnce;

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

    public void LoadDialogueFile(string filename, System.Action onLoaded = null)
    {
        string resourcePath = "dialogue/" + filename; // Resources/dialogue/filename.json
        TextAsset jsonAsset = Resources.Load<TextAsset>(resourcePath);
        if (jsonAsset == null)
        {
            Debug.LogError("Dialogue file not found in Resources: " + resourcePath);
            return;
        }
        currentDialogueFile = JsonUtility.FromJson<DialogueFile>(jsonAsset.text);
        if (onLoaded != null)
            onLoaded.Invoke();
    }

    private void ShowDialogue(int id, string name = null)
    {
        if (currentDialogueFile == null) return;

        if (name != null)
        {
            currentEntry = System.Array.Find(currentDialogueFile.dialogues, d => d.id == id && d.name == name);
        }
        else if (currentDialogueFileFileName == "encounterDialogue")
        {
            currentEntry = System.Array.Find(currentDialogueFile.dialogues, d => d.id == id && d.name == npcName);
        }
        else
        {
            currentEntry = System.Array.Find(currentDialogueFile.dialogues, d => d.id == id);
            Debug.Log("NO NAME WAS FOUND TENICALLY");
        }

        dialogueID = id;

        if (currentEntry == null)
        {
            EndDialogue();
            return;
        }

        NPCDialogueText.text = currentEntry.text;

        Debug.Log($"ShowDialogue: Looking for id={id}, name={currentEntry.name}. Found entry: {(currentEntry != null ? currentEntry.text : "null")}");

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
            // Use week name for salt/flame/drift dialogues
            if (currentNPC == 1 || currentNPC == 2 || currentNPC == 3)
                ShowDialogue(choice.nextDialogueID, currentWeekName);
            else
                ShowDialogue(choice.nextDialogueID);
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
        if (clickAction.triggered && (pass || end) && doOnce)
        {
            doOnce = false; // Prevent multiple triggers
            Debug.Log("CLICK");

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
            doOnce = true;
        }
        else
        {
            doOnce = true;
            end = true;
        }
    }

    // --- DIALOGUE ENTRY POINTS ---

    public void saltNPCDialogue()
    {
        currentNPC = 1;
        NPCSpriteRenderer.sprite = NPCsprites[0];
        string weekName = "Week" + globalManagerScript.week;

        LoadDialogueFile("saltDialogue", () =>
        {
            DialogueEntry firstEntry = System.Array.Find(
                currentDialogueFile.dialogues,
                d => d.name == weekName
            );

            if (firstEntry != null)
            {
                currentEntry = firstEntry;
                dialogueID = firstEntry.id;
                currentWeekName = weekName;
                ShowDialogue(dialogueID, currentWeekName);
            }
            else
            {
                Debug.LogError("No salt dialogue found for week: " + globalManagerScript.week);
                EndDialogue();
            }
        });
    }

    public void flameNPCDialogue()
    {
        currentNPC = 2;
        NPCSpriteRenderer.sprite = NPCsprites[1];
        LoadDialogueFile("flameDialogue"); // flameDialogue.json

        string weekName = "Week" + globalManagerScript.week;
        DialogueEntry firstEntry = System.Array.Find(
            currentDialogueFile.dialogues,
            d => d.name == weekName
        );

        if (firstEntry != null)
        {
            currentEntry = firstEntry;
            dialogueID = firstEntry.id;
            currentWeekName = weekName;
            ShowDialogue(dialogueID, currentWeekName);
        }
        else
        {
            Debug.LogError("No flame dialogue found for week: " + globalManagerScript.week);
            EndDialogue();
        }
    }

    public void driftNPCDialogue()
    {
        currentNPC = 3;
        NPCSpriteRenderer.sprite = NPCsprites[2];
        LoadDialogueFile("driftDialogue"); // driftDialogue.json

        string weekName = "Week" + globalManagerScript.week;
        DialogueEntry firstEntry = System.Array.Find(
            currentDialogueFile.dialogues,
            d => d.name == weekName
        );

        if (firstEntry != null)
        {
            currentEntry = firstEntry;
            dialogueID = firstEntry.id;
            currentWeekName = weekName;
            ShowDialogue(dialogueID, currentWeekName);
        }
        else
        {
            Debug.LogError("No drift dialogue found for week: " + globalManagerScript.week);
            EndDialogue();
        }
    }

    public void encounterNPCDialogue(int num)
    {
        end = false;
        currentNPC = num + 1;
        homeButton.interactable = false;
        travelButton.interactable = false;

        npcName = "NPC" + (num - 2);
        NPCSpriteRenderer.sprite = NPCsprites[num];

        int[] npc1Ids = { 1, 2, 5 };
        int[] npc2Ids = { 1, 4, 5 };
        int[] npc3Ids = { 1, 12, 16 };

        int weekIndex = Mathf.Clamp(globalManagerScript.week - 1, 0, 2);

        int startId = -1;
        switch (num)
        {
            case 3: startId = npc1Ids[weekIndex]; break;
            case 4: startId = npc2Ids[weekIndex]; break;
            case 5: startId = npc3Ids[weekIndex]; break;
            default:
                Debug.LogError("Invalid NPC number for encounter dialogue: " + num);
                EndDialogue();
                return;
        }

        Debug.Log($"Loading encounter dialogue for NPC: {npcName} with start ID: {startId} (week {globalManagerScript.week})");

        LoadDialogueFile("encounterDialogue", () =>
        {
            // Now the JSON is loaded, so ShowDialogue will work
            ShowDialogue(startId);
        });
    }
}
