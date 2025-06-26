using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 데이터 구조 정의
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

        // 첫 줄은 헤더이므로 생략
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            // CSV 필드가 누락되었을 경우 방지
            while (values.Length < 9)
            {
                System.Array.Resize(ref values, 9);
            }

            DialogLine line = new DialogLine
            {
                index = int.Parse(values[0]),
                charName = values[1],
                imgName = values[2],
                dialog = values[3].Replace("`", ","), // ` 를 ,로 변환
                animation = values[4],
                location = values[5],
                extra1 = values[6],
                extra2 = values[7],
                extra3 = values[8],
            };

            dialogLines.Add(line);
        }

        Debug.Log($"총 {dialogLines.Count}개의 대사가 로드되었습니다.");
    }

    void ShowLine(int index)
    {
        if (index < 0 || index >= dialogLines.Count)
        {
            Debug.LogWarning("대사 인덱스 범위를 벗어남");
            return;
        }

        DialogLine line = dialogLines[index];
        charNameText.text = line.charName;
        dialogText.text = line.dialog;

        // 여기에 imgName, animation, location 등의 처리 추가 가능
    }

    // 다음 대사로 넘어가는 예시 (나중에 버튼에 연결)
    public void NextLine()
    {
        currentIndex++;
        if (currentIndex < dialogLines.Count)
        {
            ShowLine(currentIndex);
        }
        else
        {
            Debug.Log("모든 대사 종료");
        }
    }
}
