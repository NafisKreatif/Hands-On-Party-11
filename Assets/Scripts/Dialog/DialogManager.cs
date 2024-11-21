using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Animations;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Events;

public class DialogManager : MonoBehaviour
{
  /* 
    DialogLine has two types: speech and action
    In the dialog CSV file, DialogLine with type speech consists of `speech|<speaker>|<speech>|<animationState>`.
    While DialogLine with type action consists of `action|<actionName>|<isAsync>`.
  */
  public struct DialogLineResource
  {
    public enum DialogLineType
    {
      speech,
      action,
    }

    public string id;
    public DialogLineType type;
    public string speaker;

    public string speech;
    public string animationState; // Animation state name played during the line from the animatorController
    public AnimatorController animatorController; // Animator controller for the speaker profile, every Controller 
    public SceneAction sceneAction; // Scene action to be executed after the line
    public bool isSceneAsync; // If true, the scene action will be executed asynchronously
  }

  public static DialogManager Instance { get; private set; } // Singleton instance

  public float dialogSpeed = 1.0f;

  [Header("Prefab References")]
  public GameObject dialogObject;
  public TextMeshProUGUI speakerText;
  public TextMeshProUGUI speechText;
  public Animator speakerProfileAnimator;

  [Header("Events")]

  [Tooltip("Event to be called when the dialog is started, parameter is dialog id.")]
  public UnityEvent<string> StartDialogEvent; // Event to be called when the dialog is started, parameter is dialog id.

  private Queue<DialogLineResource> _dialogQueue = new(); // Queue of dialogs, each dialog is a tuple of speaker and speech.
  private Coroutine _dialogCoroutine; // Coroutine for dialog animation
  private Dictionary<string, bool> _dialogDoneState = new(); // Dictionary of dialog done state for each dialog line

  private void Awake()
  {
    // Singleton pattern
    if (Instance != null && Instance != this)
    {
      Destroy(this);
    }
    else
    {
      Instance = this;
    }

    // Initialize dialog queue and dialog done state
    StartDialogEvent ??= new UnityEvent<string>();
  }

  private void Start()
  {
    dialogObject.SetActive(false);
  }

  private void Update()
  {
    // Check if dialog is active and player press space or left mouse button / touch screen.
    if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1"))
    {
      // Check if dialog text is fully displayed, if it is then continue to the next dialog.
      if (_dialogDoneState[_dialogQueue.Peek().id])
      {
        NextDialog();
      }
      // If dialog text is not fully displayed, display the full text.
      else
      {
        if (_dialogQueue.Peek().type == DialogLineResource.DialogLineType.speech)
        {
          Debug.LogWarning("Skip");
          if (_dialogCoroutine != null) StopCoroutine(_dialogCoroutine);
          speechText.text = _dialogQueue.Peek().speech;
          _dialogDoneState[_dialogQueue.Peek().id] = true;
        }
      }
    }
  }

  public void EnqueueDialog(params DialogLineResource[] dialogs)
  {
    foreach (DialogLineResource d in dialogs) { _dialogQueue.Enqueue(d); }
    _dialogCoroutine = StartCoroutine(StartDialog());
  }

  private void NextDialog()
  {
    _dialogQueue.Dequeue();
    if (_dialogQueue.Count == 0)
    {
      dialogObject.SetActive(false);
      speakerText.text = "";
      speechText.text = "";
      _dialogDoneState = new Dictionary<string, bool>();
    }
    else
    {
      _dialogCoroutine = StartCoroutine(StartDialog());
    }
  }

  // Coroutine for dialog animation
  private IEnumerator StartDialog()
  {
    DialogLineResource currentDialog = _dialogQueue.Peek();
    StartDialogEvent.Invoke(currentDialog.id);

    if (currentDialog.type == DialogLineResource.DialogLineType.speech)
    {
      dialogObject.SetActive(true);

      _dialogDoneState[currentDialog.id] = false;
      speakerText.text = currentDialog.speaker;
      speechText.text = "";
      speakerProfileAnimator.runtimeAnimatorController = currentDialog.animatorController;
      speakerProfileAnimator.Play(currentDialog.animationState, 0);

      for (int i = 0; i < currentDialog.speech.Length; i++)
      {
        speechText.text += currentDialog.speech[i];
        yield return new WaitForSecondsRealtime(0.025f / dialogSpeed);
      }
      speechText.text = currentDialog.speech;
      _dialogDoneState[currentDialog.id] = true;
    }
    else
    {
      dialogObject.SetActive(false);
      speakerText.text = "";
      speechText.text = "";
      currentDialog.sceneAction.Execute(currentDialog.id);
      if (currentDialog.isSceneAsync) NextDialog();
      else _dialogDoneState[currentDialog.id] = false;      
    }
  }

  // Notify the dialog manager that the dialog line has finished executing.
  public void DialogDone(string id)
  {
    _dialogDoneState[id] = true;
    if (_dialogQueue.Peek().id == id) NextDialog();
  }

  public void ResetAllTriggers()
  {
    foreach (DialogAreaTrigger trigger in FindObjectsByType<DialogAreaTrigger>(FindObjectsSortMode.None))
    {
      trigger.Reset();
    }
  }
}