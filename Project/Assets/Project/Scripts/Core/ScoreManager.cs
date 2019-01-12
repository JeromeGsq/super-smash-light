using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public int score;
	public int maxScore;

	public Text ScoreText;

	List<int> step = new List<int>();

	private void Start()
	{
		step.Add(maxScore / 3);
		step.Add(maxScore / 3 * 2);
		step.Add(maxScore);
	}

	private void RefreshScore()
	{
		this.ScoreText.text = score.ToString();
	}

	public void AddScore(int nb)
	{
		score += nb;
		if(score > maxScore)
		{
			score = maxScore;
		}
		RefreshScore();
	}

	public void LostCharge()
	{
		int tmpScore;
		tmpScore = 0;
		foreach(int s in step)
		{
			if(score > s)
			{
				tmpScore = s;
			}
		}
		score = tmpScore;
		RefreshScore();
	}
}
