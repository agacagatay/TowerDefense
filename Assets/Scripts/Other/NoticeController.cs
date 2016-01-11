using UnityEngine;
using System.Collections;

public class NoticeController : MonoBehaviour
{
	[SerializeField] UILabel announcementLabel;
	[SerializeField] UILabel noticePrimaryLabel;
	[SerializeField] UILabel noticeSecondaryLabel;
	[SerializeField] TweenAlpha tweenAlpha;

	public static NoticeController instance;

	void Awake()
	{
		instance = this;
	}

	public void DisplayOneString(string noticeMessage, float holdTime, float fadeTime)
	{
		tweenAlpha.ResetToBeginning();
		announcementLabel.text = noticeMessage;
		tweenAlpha.delay = holdTime;
		tweenAlpha.duration = fadeTime;
		noticePrimaryLabel.gameObject.SetActive(false);
		noticeSecondaryLabel.gameObject.SetActive(false);
		tweenAlpha.gameObject.SetActive(true);
		tweenAlpha.PlayForward();
		StartCoroutine(WaitAndReset(holdTime + fadeTime));
	}

	public void DisplayTwoString(string primaryMessage, string secondaryMessage, float holdTime, float fadeTime)
	{
		tweenAlpha.ResetToBeginning();
		noticePrimaryLabel.text = primaryMessage;
		noticeSecondaryLabel.text = secondaryMessage;
		tweenAlpha.delay = holdTime;
		tweenAlpha.duration = fadeTime;
		announcementLabel.gameObject.SetActive(false);
		tweenAlpha.gameObject.SetActive(true);
		tweenAlpha.PlayForward();
		StartCoroutine(WaitAndReset(holdTime + fadeTime));
	}

	IEnumerator WaitAndReset(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		tweenAlpha.gameObject.SetActive(false);
	}
}
