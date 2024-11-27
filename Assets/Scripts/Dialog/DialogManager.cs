using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
  /* 
    DialogLine has two types: speech and action
    In the dialog CSV file, DialogLine with type speech consists of `speech|<speaker>|<speech>|<animationState>`.
    While DialogLine with type action consists of `action|<actionName+actionId>|<isAsync>`.
  */
  public struct DialogLineResource
  {
    public enum DialogLineType
    {
      speech,
      action,
      slideshow,
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
  public GameObject slideshowObject;
  public TextMeshProUGUI slideshowText;

  [Header("Events")]

  [Tooltip("Event to be called when the dialog is started, parameter is dialog id.")]
  public UnityEvent<string> StartDialogEvent; // Event to be called when the dialog is started, parameter is dialog id.
  public UnityEvent<string> DialogDoneEvent; // Event to be called when the dialog is done, parameter is dialog id.

  public Dictionary<string, bool> dialogDoneState = new(); // Dictionary of dialog done state for each dialog line

  private Queue<DialogLineResource> _dialogQueue = new(); // Queue of dialogs, each dialog is a tuple of speaker and speech.
  private Coroutine _dialogCoroutine; // Coroutine for dialog animation
  private GravityRotationController _gravityRotationController;
  private AudioSource _dialogAudioSource;

  private void Awake()
  {
    // Singleton pattern
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
    }
    else
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }

    // Initialize dialog queue and dialog done state
    StartDialogEvent ??= new UnityEvent<string>();
    DialogDoneEvent ??= new UnityEvent<string>();

    _dialogAudioSource = GetComponent<AudioSource>();
  }

  private void Start()
  {
    dialogObject.SetActive(false);
    slideshowObject.SetActive(false);

    _gravityRotationController = FindFirstObjectByType<GravityRotationController>();
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  private void Update()
  {
    // Check if dialog is active and player press space or left mouse button / touch screen.
    if (_dialogQueue.Count != 0 && (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1")))
    {
      // Check if dialog text is fully displayed, if it is then continue to the next dialog.
      if (dialogDoneState[_dialogQueue.Peek().id])
      {
        NextDialog();
      }
      // If dialog text is not fully displayed, display the full text.
      else
      {
        if (_dialogQueue.Peek().type == DialogLineResource.DialogLineType.speech)
        {
          if (_dialogCoroutine != null) StopCoroutine(_dialogCoroutine);
          _dialogAudioSource.loop = false;
          speechText.text = _dialogQueue.Peek().speech;
          dialogDoneState[_dialogQueue.Peek().id] = true;
        }
        else if (_dialogQueue.Peek().type == DialogLineResource.DialogLineType.slideshow)
        {
          if (_dialogCoroutine != null) StopCoroutine(_dialogCoroutine);
          _dialogAudioSource.loop = false;
          slideshowText.text = _dialogQueue.Peek().speech;
          dialogDoneState[_dialogQueue.Peek().id] = true;
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
    if (_dialogQueue.Count != 0) _dialogQueue.Dequeue();
    if (_dialogQueue.Count == 0)
    {
      DisableDialogBox();
      dialogDoneState = new Dictionary<string, bool>();
      if (_gravityRotationController) _gravityRotationController.enabled = true;
    }
    else
    {
      if (_dialogCoroutine != null) StopCoroutine(_dialogCoroutine);
      _dialogCoroutine = StartCoroutine(StartDialog());
    }
  }

  // Coroutine for dialog animation
  private IEnumerator StartDialog()
  {
    if (_dialogQueue.Count == 0) yield break;
    DialogLineResource currentDialog = _dialogQueue.Peek();
    StartDialogEvent.Invoke(currentDialog.id);
    if (_gravityRotationController) _gravityRotationController.enabled = false;

    if (currentDialog.type == DialogLineResource.DialogLineType.speech)
    {
      dialogObject.SetActive(true);
      _dialogAudioSource.Play();
      _dialogAudioSource.loop = true;
      if (slideshowObject.activeSelf) StartCoroutine(FadeOutSlideshow());

      dialogDoneState[currentDialog.id] = false;
      speakerText.text = currentDialog.speaker;
      speechText.text = "";
      speakerProfileAnimator.runtimeAnimatorController = currentDialog.animatorController;
      speakerProfileAnimator.Play(currentDialog.animationState, 0);

      for (int i = 0; i < currentDialog.speech.Length; i++)
      {
        // Skip the text enclosed in '<' and '>'
        if (currentDialog.speech[i] == '<')
        {
          int j = i + 1;
          while (currentDialog.speech[j] != '>') j++;

          // Delay the text for a certain amount of time.
          if (currentDialog.speech.Substring(i + 1, j - i - 1).Contains("link="))
          {
            Debug.Log(currentDialog.speech.Substring(i + 1, j - i - 1).Split('=')[1]);
            float delay = float.Parse(currentDialog.speech.Substring(i + 1, j - i - 1).Split('=')[1]);

            _dialogAudioSource.loop = false;
            yield return new WaitForSecondsRealtime(delay);
            _dialogAudioSource.loop = true;
            _dialogAudioSource.Play();
          }
          else
          {
            slideshowText.text += currentDialog.speech.Substring(i, j - i + 1);
          }

          i = j + 1;
          if (currentDialog.speech[i] == '<')
          {
            i--;
            continue;
          }
        }
        if (speechText.text.Length < currentDialog.speech.Length) speechText.text += currentDialog.speech[i];
        yield return new WaitForSecondsRealtime(0.025f / dialogSpeed);
      }
      _dialogAudioSource.loop = false;
      speechText.text = currentDialog.speech;
      dialogDoneState[currentDialog.id] = true;
    }
    else if (currentDialog.type == DialogLineResource.DialogLineType.slideshow)
    {
      slideshowObject.SetActive(true);
      _dialogAudioSource.Play();
      _dialogAudioSource.loop = true;

      dialogDoneState[currentDialog.id] = false;
      slideshowText.text = "";

      for (int i = 0; i < currentDialog.speech.Length; i++)
      {
        // Skip the text enclosed in '<' and '>'
        if (currentDialog.speech[i] == '<')
        {
          int j = i + 1;
          while (currentDialog.speech[j] != '>') j++;

          // Delay the text for a certain amount of time.
          if (currentDialog.speech.Substring(i + 1, j - i - 1).Contains("link="))
          {
            Debug.Log(currentDialog.speech.Substring(i + 1, j - i - 1).Split('=')[1]);
            float delay = float.Parse(currentDialog.speech.Substring(i + 1, j - i - 1).Split('=')[1]);

            _dialogAudioSource.loop = false;
            yield return new WaitForSecondsRealtime(delay);
            _dialogAudioSource.loop = true;
            _dialogAudioSource.Play();
          }
          else
          {
            slideshowText.text += currentDialog.speech.Substring(i, j - i + 1);
          }

          // Skip the next text enclosed in '<' and '>'.
          i = j + 1;
          if (currentDialog.speech[i] == '<')
          {
            i--;
            continue;
          }
        }
        if (slideshowText.text.Length < currentDialog.speech.Length) slideshowText.text += currentDialog.speech[i];
        yield return new WaitForSecondsRealtime(0.025f / dialogSpeed);
      }
      _dialogAudioSource.loop = false;
      slideshowText.text = currentDialog.speech;
      dialogDoneState[currentDialog.id] = true;
    }
    else
    {
      DisableDialogBox();
      dialogDoneState[currentDialog.id] = false;
      currentDialog.sceneAction.Execute(currentDialog.id);
      if (currentDialog.isSceneAsync) DialogDone(currentDialog.id);
    }
  }

  // Notify the dialog manager that the dialog line has finished executing.
  public void DialogDone(string id)
  {
    dialogDoneState[id] = true;
    DialogDoneEvent.Invoke(id);
    if (_dialogQueue.Count != 0 && _dialogQueue.Peek().id == id) NextDialog();
  }

  public void ResetAllTriggers()
  {
    foreach (DialogAreaTrigger trigger in FindObjectsByType<DialogAreaTrigger>(FindObjectsSortMode.None))
    {
      trigger.Reset();
    }
  }

  private void DisableDialogBox()
  {
    dialogObject.SetActive(false);
    speakerText.text = "";
    speechText.text = "";

    if (slideshowObject.activeSelf) StartCoroutine(FadeOutSlideshow());
  }

  private IEnumerator FadeOutSlideshow()
  {
    Image slideshowImage = slideshowObject.GetComponent<Image>();

    float time = 0;
    while (time <= 2.0f)
    {
      slideshowImage.color = new Color(slideshowImage.color.r, slideshowImage.color.g, slideshowImage.color.b, 1 - time / 2.0f);
      slideshowText.color = new Color(slideshowText.color.r, slideshowText.color.g, slideshowText.color.b, 1 - time / 2.0f);
      time += Time.deltaTime;
      yield return null;
    }

    slideshowObject.SetActive(false);
    slideshowText.text = "";
    slideshowImage.color = new Color(slideshowImage.color.r, slideshowImage.color.g, slideshowImage.color.b, 1);
    slideshowText.color = new Color(slideshowText.color.r, slideshowText.color.g, slideshowText.color.b, 1);
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    _gravityRotationController = FindFirstObjectByType<GravityRotationController>();
  }
}