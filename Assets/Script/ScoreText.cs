using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreText : MonoBehaviour
{
    /// <summary>
    /// 表示用のスコア
    /// </summary>
    private int score_;
    /// <summary>
    /// テキスト本体
    /// </summary>
    private TMP_Text scoreText_;
    private void Start()
    {
        score_ = 0;
        scoreText_ = GetComponent<TMP_Text>();
    }
    /// <summary>
    /// スコア更新とテキストへの適用
    /// </summary>
    /// <param name="score">新しいスコア値</param>
    public void SetScore(int score)
    { 
        score_ += score;
        UpdateScoreText();
    }
    /// <summary>
    /// テキストの更新
    /// </summary>
    private void UpdateScoreText()
    {
        //int a = 10;
        //string sa = $"a is {a}";
        scoreText_.text = $"SCORE:{score_:00000001}";
    }

}   
