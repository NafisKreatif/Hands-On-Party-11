using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Animations;
using System.Linq;

public class DialogManager : MonoBehaviour
{
  public struct DialogLineResource
  {
    public string speaker;

    public string speech;
    public string animationState; // Animation state name played during the line from the animatorController
    public AnimatorController animatorController; // Animator controller for the speaker profile, every Controller 
  }

  public static DialogManager Instance { get; private set; } // Singleton instance
  public float dialogSpeed = 1.0f;
  public GameObject dialogObject;
  public TextMeshProUGUI speakerText;
  public TextMeshProUGUI speechText;
  public Animator speakerProfileAnimator;
  private Queue<DialogLineResource> _dialogQueue = new(); // Queue of dialogs, each dialog is a tuple of speaker and speech.
  private Coroutine _dialogCoroutine; // Coroutine for dialog animation

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
  }

  private void Start()
  {
    dialogObject.SetActive(false);
  }

  private void Update()
  {
    // Check if dialog is active and player press space or left mouse button / touch screen.
    if (dialogObject.activeSelf && (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1")))
    {
      // Check if dialog text is fully displayed, if it is then continue to the next dialog.
      if (speechText.text == _dialogQueue.Peek().speech)
      {
        _dialogQueue.Dequeue();
        if (_dialogQueue.Count == 0)
        {
          dialogObject.SetActive(false);
          speakerText.text = "";
          speechText.text = "";
        }
        else
        {
          _dialogCoroutine = StartCoroutine(StartDialog());
        }
      }
      // If dialog text is not fully displayed, display the full text.
      else
      {
        if (_dialogCoroutine != null) StopCoroutine(_dialogCoroutine);
        speechText.text = _dialogQueue.Peek().speech;
      }
    }
  }

  public void EnqueueDialog(params DialogLineResource[] dialogs)
  {
    foreach (DialogLineResource d in dialogs) { _dialogQueue.Enqueue(d); }
    if (!dialogObject.activeSelf)
    {
      dialogObject.SetActive(true);
      _dialogCoroutine = StartCoroutine(StartDialog());
    }
  }

  // Coroutine for dialog animation
  private IEnumerator StartDialog()
  {
    if (!dialogObject.activeSelf) yield return null;

    DialogLineResource currentDialog = _dialogQueue.Peek();
    speakerText.text = currentDialog.speaker;
    speechText.text = "";
    speakerProfileAnimator.runtimeAnimatorController = currentDialog.animatorController;        
    speakerProfileAnimator.Rebind();
    speakerProfileAnimator.Play(currentDialog.animationState, 0, 0.0f);    

    for (int i = 0; i < currentDialog.speech.Length; i++)
    {
      speechText.text += currentDialog.speech[i];
      yield return new WaitForSecondsRealtime(0.025f / dialogSpeed);
    }
    speechText.text = currentDialog.speech;
  }
}