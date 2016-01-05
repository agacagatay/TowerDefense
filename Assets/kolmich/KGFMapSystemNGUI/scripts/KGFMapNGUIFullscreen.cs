using UnityEngine;
using System.Collections;

public class KGFMapNGUIFullscreen : MonoBehaviour
{
	void OnClick()
	{
		KGFMapSystem aMapSystem = KGFAccessor.GetObject<KGFMapSystem>();
		if (aMapSystem != null)
		{
			aMapSystem.SetFullscreen(!aMapSystem.GetFullscreen());
			aMapSystem.SetClickUsed();
		}
	}
}
