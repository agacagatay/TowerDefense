using UnityEngine;
using System.Collections;

public class HealthBarController : MonoBehaviour
{
	[SerializeField] UIWidget primaryHealthBar;
	[SerializeField] UIWidget secondaryHealthBar;
	[SerializeField] UIWidget barrierHealthBar;
	[SerializeField] UIWidget turretHealthBar;
	[SerializeField] UIWidget enemyHealthBar;
	UISprite healthBarSprite;
	AllyStructureController structureController;
	EnemyUnitVariables adversaryUnitVariables;
	public AllyStructureController StructureController { get { return structureController; }}
	public EnemyUnitVariables AdversaryUnitVariables { get { return adversaryUnitVariables; }}

	public static HealthBarController instance;

	void Awake()
	{
		instance = this;
	}

	void OnEnable()
	{
		EasyTouch.On_SimpleTap += On_SimpleTap;
	}

	void OnDisable()
	{
		UnsubscribeEvent();
	}

	void OnDestroy()
	{
		UnsubscribeEvent();
	}

	void UnsubscribeEvent()
	{
		EasyTouch.On_SimpleTap -= On_SimpleTap;
	}

	void On_SimpleTap(Gesture gesture)
	{
		DisableAllHealthBars();

		if (gesture.pickedObject != null && gesture.pickedObject.layer == 10)
		{
			structureController = gesture.pickedObject.GetComponent<AllyStructureController>();

			if (structureController.IsPrimaryStructure)
			{
				EnablePrimaryHealthBar(gesture.pickedObject.transform, structureController);
			}
			else if (structureController.IsSecondaryStructure)
			{
				EnableSecondaryHealthBar(gesture.pickedObject.transform, structureController);
			}
			else if (structureController.IsTurret)
			{
				EnableTurretHealthBar(gesture.pickedObject.transform, structureController);
			}
			else
			{
				EnableBarrierHealthBar(gesture.pickedObject.transform, structureController);
			}
		}
		else if (gesture.pickedObject.tag == "EnemyGround" || gesture.pickedObject.tag == "EnemyAir")
		{
			adversaryUnitVariables = gesture.pickedObject.GetComponent<EnemyUnitVariables>();
			EnableEnemyHealthBar(gesture.pickedObject.transform, adversaryUnitVariables);
		}
	}

	public void EnablePrimaryHealthBar(Transform structureTransform, AllyStructureController allyStructureController)
	{
		primaryHealthBar.SetAnchor(structureTransform);
		UILabel healthBarLabel = turretHealthBar.GetComponentInChildren<UILabel>();
		healthBarLabel.text = allyStructureController.StructureName;
		healthBarSprite = primaryHealthBar.GetComponentInChildren<UISprite>();
		healthBarSprite.fillAmount = (allyStructureController.StructureHealth * 1f) / allyStructureController.InitialStructureHealth;
		primaryHealthBar.gameObject.SetActive(true);
	}

	public void EnableSecondaryHealthBar(Transform structureTransform, AllyStructureController allyStructureController)
	{
		secondaryHealthBar.SetAnchor(structureTransform);
		UILabel healthBarLabel = turretHealthBar.GetComponentInChildren<UILabel>();
		healthBarLabel.text = allyStructureController.StructureName;
		healthBarSprite = secondaryHealthBar.GetComponentInChildren<UISprite>();
		healthBarSprite.fillAmount = (allyStructureController.StructureHealth * 1f) / allyStructureController.InitialStructureHealth;
		secondaryHealthBar.gameObject.SetActive(true);
	}

	public void EnableBarrierHealthBar(Transform structureTransform, AllyStructureController allyStructureController)
	{
		barrierHealthBar.SetAnchor(structureTransform);
		UILabel healthBarLabel = turretHealthBar.GetComponentInChildren<UILabel>();
		healthBarLabel.text = allyStructureController.StructureName;
		healthBarSprite = barrierHealthBar.GetComponentInChildren<UISprite>();
		healthBarSprite.fillAmount = (allyStructureController.StructureHealth * 1f) / allyStructureController.InitialStructureHealth;
		barrierHealthBar.gameObject.SetActive(true);
	}

	public void EnableTurretHealthBar(Transform structureTransform, AllyStructureController allyStructureController)
	{
		turretHealthBar.SetAnchor(structureTransform);
		UILabel healthBarLabel = turretHealthBar.GetComponentInChildren<UILabel>();
		healthBarLabel.text = allyStructureController.StructureName;
		healthBarSprite = turretHealthBar.GetComponentInChildren<UISprite>();
		healthBarSprite.fillAmount = (allyStructureController.StructureHealth * 1f) / allyStructureController.InitialStructureHealth;
		turretHealthBar.gameObject.SetActive(true);
	}

	public void EnableEnemyHealthBar(Transform structureTransform, EnemyUnitVariables enemyUnitVariables)
	{
		enemyHealthBar.SetAnchor(structureTransform);
		UILabel healthBarLabel = enemyHealthBar.GetComponentInChildren<UILabel>();
		healthBarLabel.text = enemyUnitVariables.UnitName;
		healthBarSprite = enemyHealthBar.GetComponentInChildren<UISprite>();
		healthBarSprite.fillAmount = (enemyUnitVariables.UnitHealth * 1f) / enemyUnitVariables.InitialHealth;
		enemyHealthBar.gameObject.SetActive(true);
	}

	public void UpdateHealthBar(AllyStructureController allyStructureController)
	{
		if (StructureController != null && allyStructureController.gameObject == StructureController.gameObject)
		{
			healthBarSprite.fillAmount = (allyStructureController.StructureHealth * 1f) / allyStructureController.InitialStructureHealth;
		}
	}

	public void UpdateEnemyHealthBar(EnemyUnitVariables enemyUnitVariables)
	{
		if (AdversaryUnitVariables != null && enemyUnitVariables.gameObject == AdversaryUnitVariables.gameObject)
		{
			healthBarSprite.fillAmount = (enemyUnitVariables.UnitHealth * 1f) / enemyUnitVariables.InitialHealth;
		}
	}

	public void DisableAllHealthBars()
	{
		primaryHealthBar.gameObject.SetActive(false);
		secondaryHealthBar.gameObject.SetActive(false);
		barrierHealthBar.gameObject.SetActive(false);
		turretHealthBar.gameObject.SetActive(false);
		enemyHealthBar.gameObject.SetActive(false);
	}
		
	public void DisableEnemyHealthBar(GameObject enemyUnit)
	{
		if (AdversaryUnitVariables != null && enemyUnit == AdversaryUnitVariables.gameObject)
			enemyHealthBar.gameObject.SetActive(false);
	}
}
