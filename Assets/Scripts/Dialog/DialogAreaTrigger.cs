using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DialogAreaTrigger : MonoBehaviour
{
    [Header("States")]

    public bool hasPlayed = false;
    public bool playOnlyOnce = false;
    public string id;
    private static Dictionary<string, bool> HasPlayedDictionary = new();

    [Header("Resources")]

    [Tooltip("A CSV file with dialoglines in which eahc line has two types: speech and action. a dialog line with type speech consists of `speech|<speaker>|<speech>|<animationController>|<animationState>`. While DialogLine with type action consists of `action|<actionName+actionId>|<isAsync>`.")]
    public TextAsset dialogResource; // CSV file with dialog lines each containing (<type>, <speaker|actionName>, <speech|isAsync>, <animationState|null>).
    [Tooltip("List of scene actions to be executed. Only scenes from this list can be executed by the dialogResource.")]
    public List<SceneAction> sceneActionScripts; // List of scene actions to be executed.
    [Tooltip("Animator controller for the speaker profile, every Controller has to have a state with the same name as the animationState in the dialogResource.")]
    public RuntimeAnimatorController[] animatorControllers;

    [Header("Events")]
    [Tooltip("Event to be called when the dialog area is entered.")]
    public UnityEvent EnterEvent; // Event to be called when the dialog area is entered.
    [Tooltip("Event to be called when the dialog is reset.")]
    public UnityEvent ResetEvent; // Event to be called when the dialog is reset.
    private DialogManager.DialogLineResource[] _dialogLines;

    void Start()
    {
        // Parse dialogResource whic is a CSV file using '|' as delimiter
        string[] lines = dialogResource.text.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
        _dialogLines = new DialogManager.DialogLineResource[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split('|');
            if (fields.Length == 0)
            {
                Debug.LogError("Invalid dialog line: " + lines[i]);
                continue;
            }

            _dialogLines[i] = new DialogManager.DialogLineResource
            {
                id = gameObject.GetHashCode() + "_" + i
            };
            switch (fields[0])
            {
                case "speech":
                    if (fields.Length < 5)
                    {
                        Debug.LogError("Invalid dialog line: " + lines[i]);
                        break;
                    }
                    _dialogLines[i].type = DialogManager.DialogLineResource.DialogLineType.speech;
                    _dialogLines[i].speaker = fields[1];
                    _dialogLines[i].speech = fields[2];
                    _dialogLines[i].animationState = fields[4];
                    _dialogLines[i].animatorController = animatorControllers.First(v => v.name == fields[3]);
                    break;
                case "action":
                    if (fields.Length < 3)
                    {
                        Debug.LogError("Invalid dialog line: " + lines[i]);
                        break;
                    }
                    _dialogLines[i].type = DialogManager.DialogLineResource.DialogLineType.action;
                    _dialogLines[i].sceneAction = sceneActionScripts.Find(v => (v.ActionName + v.actionId) == fields[1]);
                    if (_dialogLines[i].sceneAction == null) Debug.LogError("Scene action not found: " + fields[1]);
                    _dialogLines[i].isSceneAsync = fields[2] == "async";
                    break;
                case "slideshow":
                    if (fields.Length < 2)
                    {
                        Debug.LogError("Invalid dialog line: " + lines[i]);
                        break;
                    }
                    _dialogLines[i].type = DialogManager.DialogLineResource.DialogLineType.slideshow;
                    _dialogLines[i].speech = fields[1];
                    break;
                default:
                    Debug.LogError("Invalid dialog line type: " + fields[0]);
                    break;
            }
        }

        // Initialize events
        EnterEvent ??= new UnityEvent();
        ResetEvent ??= new UnityEvent();
        ResetEvent.AddListener(OnReset);

        if (!HasPlayedDictionary.ContainsKey(id)) HasPlayedDictionary[id] = false;
    }


    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !(playOnlyOnce ? HasPlayedDictionary[id] : hasPlayed))
        {
            DialogManager.Instance.EnqueueDialog(_dialogLines);
            if (playOnlyOnce) HasPlayedDictionary[id] = true;
            else hasPlayed = true;
            EnterEvent.Invoke();
        }
    }

    public void Reset()
    {
        ResetEvent.Invoke();
    }

    private void OnReset()
    {
        hasPlayed = false;
    }
}
