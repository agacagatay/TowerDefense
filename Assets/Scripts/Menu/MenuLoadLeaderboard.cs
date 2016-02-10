using UnityEngine;
using System.Collections;

public class MenuLoadLeaderboard : MonoBehaviour
{
	void OnClick()
	{
		GameCenterManager.ShowLeaderboard("leaderboard_points");
	}
}
