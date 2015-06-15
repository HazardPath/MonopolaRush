using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrewPortrait : MonoBehaviour {

	private static Dictionary<Jobs, Sprite> spriteList = null;
	
	private SpriteRenderer spriteRenderer;

	public Jobs whoAmI;

	// Use this for initialization
	void Start () {
		if (spriteList==null) {
			spriteList = new Dictionary<Jobs, Sprite>();
			spriteList[Jobs.grunt] = Resources.Load<Sprite>("Crew/In Conversation/Security");
			spriteList[Jobs.meche] = Resources.Load<Sprite>("Crew/In Conversation/Engineer");
			spriteList[Jobs.rogue] = Resources.Load<Sprite>("Crew/In Conversation/Grifter");
			spriteList[Jobs.medic] = Resources.Load<Sprite>("Crew/In Conversation/Medic");
		}
		Update ();
	}
	
	// Update is called once per frame
	void Update () {
		spriteRenderer.sprite = spriteList [whoAmI];
	}
}
