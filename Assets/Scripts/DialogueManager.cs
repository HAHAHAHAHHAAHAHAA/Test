using UnityEngine;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class DialogueLine
{
    public AudioClip audioClip; // Аудиодорожка реплики
    public string subtitleText; // Текст субтитров
    public Sprite speakerIcon; // Иконка говорящего
    public float delayAfter = 0.5f; // Задержка после завершения аудио
}

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI subtitleText;
    [SerializeField] private UnityEngine.UI.Image speakerImage;

    [Header("Dialogue Settings")]
    [SerializeField] private DialogueLine[] dialogueLines;

    private AudioSource audioSource;
    private Queue<DialogueLine> dialogueQueue = new Queue<DialogueLine>();
    private bool isDialogueActive = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(DialogueLine[] lines)
    {
        dialogueQueue.Clear();
        foreach (var line in lines)
        {
            dialogueQueue.Enqueue(line);
        }
        isDialogueActive = true;
        PlayNextLine();
    }

    private void PlayNextLine()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = dialogueQueue.Dequeue();
        StartCoroutine(PlayLine(currentLine));
    }

    private System.Collections.IEnumerator PlayLine(DialogueLine line)
    {
        // Настройка UI
        dialoguePanel.SetActive(true);
        subtitleText.text = line.subtitleText;
        speakerImage.sprite = line.speakerIcon;

        // Воспроизведение аудио
        audioSource.clip = line.audioClip;
        audioSource.Play();

        // Ждем завершения аудио + задержку
        yield return new WaitForSeconds(line.audioClip.length + line.delayAfter);

        PlayNextLine();
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueActive = false;
    }

    void Update()
    {
        // Пропуск диалога по нажатию кнопки (например, Space)
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.Stop();
            StopAllCoroutines();
            PlayNextLine();
        }
    }

    // Пример запуска диалога
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartDialogue(dialogueLines);
        }
        this.gameObject.GetComponent<Collider>().enabled = false;
    }
}