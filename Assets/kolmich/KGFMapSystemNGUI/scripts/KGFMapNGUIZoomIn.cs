using UnityEngine;
using System.Collections;

public class KGFMapNGUIZoomIn : MonoBehaviour
{
	void OnClick()
	{
		KGFMapSystem aMapSystem = KGFAccessor.GetObject<KGFMapSystem>();
		if (aMapSystem != null)
		{
			aMapSystem.ZoomIn();
			aMapSystem.SetClickUsed();
		}
	}
}
