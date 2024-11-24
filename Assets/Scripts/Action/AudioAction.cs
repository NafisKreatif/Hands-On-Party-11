using System.Collections;
using UnityEngine;

public class AudioAction : SceneAction
{
  public AudioSource audioSource;
  public override string ActionName => "AudioAction";
  public override void Execute(string id)
  {
    dialogId = id;

    StartCoroutine(PlayAudio());
  }

  private IEnumerator PlayAudio()
  {
    audioSource.Play();
    yield return new WaitForSecondsRealtime(audioSource.clip.length);
    DialogManager.Instance.DialogDone(dialogId);
  }
}