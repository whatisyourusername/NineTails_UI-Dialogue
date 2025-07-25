using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JetBrains.Annotations;

// 데이터 구조 정의
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

    [SerializeField] private string foxName;

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
                dialog = values[4].Replace("`", ",").Replace("구미호",foxName), // ` 를 ,로 변환
                animation = values[5],
                location = values[6],
                sound = values[7],
                bgmusic = values[8],
                bgImg = values[9],
                extra1 = values[10],
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

        if (line.charIndex == 0)
        {
            // 모든 이미지 비활성화
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
            // 모든 이미지 비활성화 후 해당 인덱스만 활성화
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

    // 다음 대사로 넘어가는 예시 (나중에 버튼에 연결)
    public void NextLine()
    {
        if (currentIndex < dialogLines.Count - 1)
        {
            currentIndex++;
            ShowLine(currentIndex);
        }
        else
        {
            Debug.Log("모든 대사 종료");
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
            Debug.Log("첫 대사 출력 완료");
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