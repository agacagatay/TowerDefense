using UnityEngine;
using System.Collections;

public class KGFMapNGUILock : MonoBehaviour
{
	void OnClick()
	{
		KGFMapSystem aMapSystem = KGFAccessor.GetObject<KGFMapSystem>();
		if (aMapSystem != null)
		{
			aMapSystem.SetModeStatic(!aMapSystem.GetModeStatic());
			aMapSystem.SetClickUsed();
		}
	}
}
