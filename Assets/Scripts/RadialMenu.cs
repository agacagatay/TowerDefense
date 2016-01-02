using UnityEngine;
using System.Collections;

public class RadialMenu : MonoBehaviour
{
	[SerializeField] Vector2 center = new Vector2(500f, 500f); // position of center button
	[SerializeField] float radius = 125f; // pixels radius to center of button;
	[SerializeField] Texture centerButton;  
	[SerializeField] Texture[] normalButtons;
	[SerializeField] Texture[] selectedButtons;

	int ringCount; 
	Rect centerRect;
	Rect[] ringRects;
	float angle;
	bool showButtons = false;
	float index;

	void Start()
	{
		ringCount = normalButtons.Length;
		angle = 360f / ringCount;

		centerRect.x = center.x - centerButton.width * 0.5f;
		centerRect.y = center.y - centerButton.height * 0.5f;
		centerRect.width = centerButton.width;
		centerRect.height = centerButton.height;

		ringRects = new Rect[ringCount];

		var w = normalButtons[0].width;
		var h = normalButtons[0].height;
		var rect = new Rect(0f, 0f, w, h);

		Vector2 v = new Vector2(radius, 0f);

		for (var i = 0; i < ringCount; i++) {
			rect.x = center.x + v.x - w * 0.5f;
			rect.y = center.y + v.y - h * 0.5f;
			ringRects[i] = rect;
			v = Quaternion.AngleAxis(angle, Vector3.forward) * v;
		}
	}

	void OnGUI()
	{
		var e = Event.current;

		if (e.type == EventType.MouseDown && centerRect.Contains(e.mousePosition))
		{
			showButtons = true;
			index = -1f;
		}    

		if (e.type == EventType.MouseUp)
		{
			if (showButtons)
			{
				Debug.Log("User selected #" + index);
			}

			showButtons = false;
		}

		if (e.type == EventType.MouseDrag)
		{
			var v = e.mousePosition - center;
			var a = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
			a += angle / 2f;

			if (a < 0)
				a = a + 360f;

			index = a / angle;
		}

		GUI.DrawTexture(centerRect, centerButton);

		if (showButtons)
		{
			for (var i = 0; i < normalButtons.Length; i++)
			{
				if (i != index) 
					GUI.DrawTexture(ringRects[i], normalButtons[i]);
				else
					GUI.DrawTexture(ringRects[i], selectedButtons[i]);
			}
		}
	}
}
