using System;
using System.Collections;
using UnityEngine;

public class KGFMapNGUIMain : MonoBehaviour
{
	KGFMapSystem itsMapSystem;
	
	public UIPanel itsNGUIPanel;
	public UIPanel itsNGUIPanelFullscreen;
	
	void Start ()
	{
		KGFAccessor.GetExternal<KGFMapSystem>(OnMapSystemRegistered);
	}
	
	void OnMapSystemRegistered(object theSender, EventArgs theArgs)
	{
		KGFAccessor.KGFAccessorEventargs anArgs = (KGFAccessor.KGFAccessorEventargs)theArgs;
		itsMapSystem = (KGFMapSystem)anArgs.GetObject();
		itsMapSystem.EventFullscreenModeChanged += OnFullScreenModeChanged;
		
		UpdateNGUI();
	}
	
	void OnFullScreenModeChanged(object theSender, EventArgs theArgs)
	{
		UpdateNGUI();
	}
	
	void UpdateNGUI()
	{
		if (itsMapSystem == null)
			return;
		
		itsMapSystem.itsDataModuleMinimap.itsGlobalSettings.itsHideGUI = true;
		if (itsNGUIPanel != null)
		{
			itsNGUIPanel.enabled = !itsMapSystem.GetFullscreen();
			KGFMapSystem.KGFSetChildrenActiveRecursively(itsNGUIPanel.gameObject,itsNGUIPanel.enabled);
		}
		
		if (itsNGUIPanelFullscreen != null)
		{
			itsNGUIPanelFullscreen.enabled = itsMapSystem.GetFullscreen();
			KGFMapSystem.KGFSetChildrenActiveRecursively(itsNGUIPanelFullscreen.gameObject,itsNGUIPanelFullscreen.enabled);
		}
	}
}
