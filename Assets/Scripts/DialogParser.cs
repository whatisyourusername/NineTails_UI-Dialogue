using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// ������ ���� ����
[System.Serializable]
public class DialogLine
{
    public int index;
    public string charName;
    public int charIndex;
    public string imgName;
    public string dialog;
    public string animation;
    public string location;
    public string sound;
    public string bgmusic;
    public string bgImg;
    public string extra1;
}

public class DialogParser : MonoBehaviour
{

    [SerializeField] private TextAsset dialogData;
    [SerializeField] private TextMeshProUGUI charNameText;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private GameObject dialogUI;
    [SerializeField] private GameObject playerDialog;
    [SerializeField] private TextMeshProUGUI playerDialogBox;
    [SerializeField] private Image[] charImgs = new Image[5];

    private List<DialogLine> dialogLines = new List<DialogLine>();
    private int currentIndex = 0;

    void Start()
    {
        LoadDialog();
        ShowLine(0);
    }

    void LoadDialog()
    {
        string[] lines = dialogData.text.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        // ù ���� ����̹Ƿ� ����
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            // CSV �ʵ尡 �����Ǿ��� ��� ����
            while (values.Length < 11)
            {
                System.Array.Resize(ref values, 11);
            }

            DialogLine line = new DialogLine
            {
                index = int.Parse(values[0]),
                charName = values[1],
                charIndex = int.Parse(values[2]) - 1,
                imgName = values[3],
                dialog = values[4].Replace("`", ","), // ` �� ,�� ��ȯ
                animation = values[5],
                location = values[6],
                sound = values[7],
                bgmusic = values[8],
                bgImg = values[9],
                extra1 = values[10],
            };

            dialogLines.Add(line);
        }

        Debug.Log($"�� {dialogLines.Count}���� ��簡 �ε�Ǿ����ϴ�.");
    }

    void ShowLine(int index)
    {
        if (index < 0 || index >= dialogLines.Count)
        {
            Debug.LogWarning("��� �ε��� ������ ���");
            return;
        }

        DialogLine line = dialogLines[index];

        if (line.charIndex == 0)
        {
            // ��� �̹��� ��Ȱ��ȭ
            for (int i = 0; i < charImgs.Length; i++)
            {
                charImgs[i].gameObject.SetActive(false);
            }

            playerDialog.SetActive(true);
            dialogUI.SetActive(false);
            playerDialogBox.text = line.dialog;
        }
        else
        {
            // ��� �̹��� ��Ȱ��ȭ �� �ش� �ε����� Ȱ��ȭ
            for (int i = 0; i < charImgs.Length; i++)
            {
                charImgs[i].gameObject.SetActive(i == line.charIndex);
            }

            playerDialog.SetActive(false);
            dialogUI.SetActive(true);
            charNameText.text = line.charName;
            dialogText.text = line.dialog;
            SetLocation(line.location, line.charIndex);
        }
    }

    // ���� ���� �Ѿ�� ���� (���߿� ��ư�� ����)
    public void NextLine()
    {
        if (currentIndex < dialogLines.Count - 1)
        {
            currentIndex++;
            ShowLine(currentIndex);
        }
        else
        {
            Debug.Log("��� ��� ����");
        }
    }
    public void PreviousLine()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            ShowLine(currentIndex);
        }
        else
        {
            Debug.Log("ù ��� ��� �Ϸ�");
        }
    }
    void SetLocation(string location, int imgNum)
    {
        if (location == "left")
        {
            charImgs[imgNum].rectTransform.anchoredPosition = new Vector2(-450f, charImgs[imgNum].rectTransform.anchoredPosition.y);
        }
        else if (location == "right")
        {
            charImgs[imgNum].rectTransform.anchoredPosition = new Vector2(450f, charImgs[imgNum].rectTransform.anchoredPosition.y);
        }
        else if (location == "center")
        {
            charImgs[imgNum].rectTransform.anchoredPosition = new Vector2(0f, charImgs[imgNum].rectTransform.anchoredPosition.y);
        }
    }
}