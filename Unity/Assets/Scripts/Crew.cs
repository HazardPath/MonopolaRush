using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crew : Cargo {
	// Map of jobs to sprite images for them
	//TODO public static readonly Dictionary<string, Sprite> 

	// What this crewmember's job is.
	public Jobs job;

	// Hold I'm in. If I'm not in a hold, null.
	public CrewHold crewHold = null;

	private Dictionary<Jobs, Sprite[]> spriteList = new Dictionary<Jobs, Sprite[]>();

	private SpriteRenderer spriteRenderer;

	public override void CrewHit(){
		if (crewSlotHitList.Count > 1) {
			GoHome ();
			return;
		} else {
			GameObject hit = crewSlotHitList[0];
			CrewHold hitHold = hit.GetComponent<CrewHold>();
			PlayerShip ship = PlayerShip.FindObjectOfType<PlayerShip>();
			if(ship.crew.Length>hitHold.bunkIndex && object.Equals(ship.crew[hitHold.bunkIndex], null)){
				//If I was anywhere before, unattach myself from there.
				UnattachSelf();
				//I hit a bunk that's empty, so I can put myself here.
				ship.crew[hitHold.bunkIndex] = this;
				crewHold = hitHold;
				//Move myself to line up with the bunk
				//And set this as my new home
				pos = new Vector3(crewHold.transform.position.x-0.32f, crewHold.transform.position.y+0.32f, -0.5f);
				transform.position = pos;
				return;
			}else{
				GoHome();
				return;
			}
		}
	}

	public override void UnattachSelf (){
		base.UnattachSelf ();
		if(crewHold != null){
			PlayerShip ship = PlayerShip.FindObjectOfType<PlayerShip>();
			ship.crew[crewHold.bunkIndex] = null;
			crewHold = null;
		}
	}

	public override void Start(){
		base.Start ();
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteList [Jobs.meche] = new Sprite[]{
			Resources.Load<Sprite>("Crew/In Cargo/Engineer"),
			Resources.Load<Sprite>("Crew/In Crew Slots/Engineer")
		};
		spriteList [Jobs.rogue] = new Sprite[]{
			Resources.Load<Sprite>("Crew/In Cargo/Grifter"),
			Resources.Load<Sprite>("Crew/In Crew Slots/Grifter")
		};
		spriteList [Jobs.medic] = new Sprite[]{
			Resources.Load<Sprite>("Crew/In Cargo/Medic"),
			Resources.Load<Sprite>("Crew/In Crew Slots/Medic")
		};
		spriteList [Jobs.grunt] = new Sprite[]{
			Resources.Load<Sprite>("Crew/In Cargo/Security"),
			Resources.Load<Sprite>("Crew/In Crew Slots/Security")
		};
		Update ();
	}

	public override void Update(){
		base.Update ();
		if (crewHold) {
			spriteRenderer.sprite = spriteList [job] [1];
		} else {
			spriteRenderer.sprite = spriteList [job] [0];
		}
	}
}
