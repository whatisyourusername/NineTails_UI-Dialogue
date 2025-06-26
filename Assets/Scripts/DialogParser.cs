using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// ������ ���� ����
[System.Serializable]
public class DialogLine
{
    public int index;
    public string charName;
    public string imgName;
    public string dialog;
    public string animation;
    public string location;
    public string extra1;
    public string extra2;
    public string extra3;
}

public class DialogParser : MonoBehaviour
{
    [SerializeField] private TextAsset dialogData;
    [SerializeField] private TextMeshProUGUI charNameText;
    [SerializeField] private TextMeshProUGUI dialogText;

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
            while (values.Length < 9)
            {
                System.Array.Resize(ref values, 9);
            }

            DialogLine line = new DialogLine
            {
                index = int.Parse(values[0]),
                charName = values[1],
                imgName = values[2],
                dialog = values[3].Replace("`", ","), // ` �� ,�� ��ȯ
                animation = values[4],
                location = values[5],
                extra1 = values[6],
                extra2 = values[7],
                extra3 = values[8],
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
        charNameText.text = line.charName;
        dialogText.text = line.dialog;

        // ���⿡ imgName, animation, location ���� ó�� �߰� ����
    }

    // ���� ���� �Ѿ�� ���� (���߿� ��ư�� ����)
    public void NextLine()
    {
        currentIndex++;
        if (currentIndex < dialogLines.Count)
        {
            ShowLine(currentIndex);
        }
        else
        {
            Debug.Log("��� ��� ����");
        }
    }
}
