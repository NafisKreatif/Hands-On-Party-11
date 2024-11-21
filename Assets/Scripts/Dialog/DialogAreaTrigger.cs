using UnityEditor.Animations;
using UnityEngine;

public class DialogAreaTrigger : MonoBehaviour
{
    public bool hasPlayed = false;
    public TextAsset dialogResource; // CSV file with dialog lines each containing (speaker, speech, animationState)
    public AnimatorController animatorController;
    private DialogManager.DialogLineResource[] _dialogLines;

    void Start()
    {        
        // Parse dialogResource whic is a CSV file using '|' as delimiter
        string[] lines = dialogResource.text.Split("\r\n");        
        _dialogLines = new DialogManager.DialogLineResource[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split('|');
            Debug.Log(fields[0]);
            _dialogLines[i] = new DialogManager.DialogLineResource
            {
                speaker = fields[0],
                speech = fields[1],
                animationState = fields[2],
                animatorController = animatorController
            };            
        }
    }


    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            DialogManager.Instance.EnqueueDialog(_dialogLines);
            hasPlayed = true;
        }
    }
}
