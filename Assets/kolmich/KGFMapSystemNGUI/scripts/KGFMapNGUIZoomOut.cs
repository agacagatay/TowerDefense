using UnityEngine;
using System.Collections;

public class KGFMapNGUIZoomOut : MonoBehaviour
{
	void OnClick()
	{
		KGFMapSystem aMapSystem = KGFAccessor.GetObject<KGFMapSystem>();
		if (aMapSystem != null)
		{
			aMapSystem.ZoomOut();
			aMapSystem.SetClickUsed();
		}
	}
}
