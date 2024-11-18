using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.Collections;

public class DialogManager : MonoBehaviour
{
  public static DialogManager instance { get; private set; } // Singleton instance
  public float dialogSpeed = 1.0f;
  public GameObject dialogObject;
  public TextMeshProUGUI dialogSpeakerText;
  public TextMeshProUGUI dialogSpeechText;
  private Queue<Tuple<string, string>> _dialogQueue = new(); // Queue of dialogs, each dialog is a tuple of speaker and speech.
  private Coroutine _dialogCoroutine; // Coroutine for dialog animation

  private void Awake()
  {
    // Singleton pattern
    if (instance != null && instance != this)
    {
      Destroy(this);
    }
    else
    {
      instance = this;
    }
  }

  private void Start()
  {
    dialogObject.SetActive(false);

    // Example usage
    // EnqueueDialog(
    //   new Tuple<string, string>("Speaker 1", "Hello, World!"),
    //   new Tuple<string, string>("Speaker 2", "Hi, there!"),
    //   new Tuple<string, string>("Speaker 1", "How are you?"),
    //   new Tuple<string, string>("Speaker 2", "I'm fine, thank you!"),
    //   new Tuple<string, string>("Speaker 1", "Good to hear that!"),
    //   new Tuple<string, string>("Speaker 2", "How about you?"),
    //   new Tuple<string, string>("Speaker 1", "I'm doing great!"),
    //   new Tuple<string, string>("Speaker 2", "That's awesome!"),
    //   new Tuple<string, string>("Speaker 1", "Thank you!"),
    //   new Tuple<string, string>("Speaker 2", "You're welcome!"),
    //   new Tuple<string, string>("Speaker 1", "Goodbye!"),
    //   new Tuple<string, string>("Speaker 2", "Goodbye!")
    // );
  }

  private void Update()
  {
    // Check if dialog is active and player press space or left mouse button / touch screen.
    if (dialogObject.activeSelf && (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1")))
    {
      // Check if dialog text is fully displayed, if it is then continue to the next dialog.
      if (dialogSpeechText.text == _dialogQueue.Peek().Item2)
      {
        _dialogQueue.Dequeue();
        if (_dialogQueue.Count == 0)
        {
          dialogObject.SetActive(false);
          dialogSpeakerText.text = "";
          dialogSpeechText.text = "";
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
        dialogSpeechText.text = _dialogQueue.Peek().Item2;
      }
    }
  }

  public void EnqueueDialog(params Tuple<string, string>[] dialogs)
  {
    foreach (Tuple<string, string> d in dialogs) { _dialogQueue.Enqueue(d); }
    if (!dialogObject.activeSelf)
    {
      dialogObject.SetActive(true);
      _dialogCoroutine = StartCoroutine(StartDialog());
    }
  }

  // Coroutine for dialog animation
  private IEnumerator StartDialog()
  {
    if (dialogObject.activeSelf) yield return null;

    Tuple<string, string> currentDialog = _dialogQueue.Peek();
    string speaker = currentDialog.Item1;
    string speech = currentDialog.Item2;
    dialogSpeakerText.text = speaker;
    dialogSpeechText.text = "";

    for (int i = 0; i < speech.Length; i++)
    {
      dialogSpeechText.text += speech[i];
      yield return new WaitForSecondsRealtime(0.025f / dialogSpeed);
    }
    dialogSpeechText.text = speech;
  }
}