#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Games {public class World : MonoBehaviour{
public static int frame;
void Update () { Update(Time.deltaTime, this); 
 frame++; }
public bool JustEntered = true;


public void Start()
	{
		AvatarGenerator ___avaGen00;
		___avaGen00 = AvatarGenerator.Find();
		UnityAction.SpawnActions();
		UnityEventController.SpawnAllPlayerEvents();
		eventController = new EventController();
		Persons = (

Enumerable.Empty<Person>()).ToList<Person>();
		MainPlayer = new Player(eventController);
		AvatarGen = ___avaGen00;
		
}
		public AvatarGenerator AvatarGen;
	public Player MainPlayer;
	public List<Person> __Persons;
	public List<Person> Persons{  get { return  __Persons; }
  set{ __Persons = value;
 foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
}
} }
 }
	public EventController __eventController;
	public EventController eventController{  get { return  __eventController; }
  set{ __eventController = value;
 if(!value.JustEntered) __eventController = value; 
 else{ value.JustEntered = false;
}
 }
 }
	public System.Int32 ___x00;
	public System.Int32 counter20;
	public Casanova.Prelude.Tuple<System.String,System.Collections.Generic.List<Casanova.Prelude.Tuple<System.Int32,System.Int32>>> ___parsedperson10;
	public System.Int32 counter21;
	public System.Collections.Generic.List<Casanova.Prelude.Tuple<System.Int32,System.Int32>> ___personalities10;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;

		MainPlayer.Update(dt, world);
		for(int x0 = 0; x0 < Persons.Count; x0++) { 
			Persons[x0].Update(dt, world);
		}
		eventController.Update(dt, world);
		this.Rule0(dt, world);
		this.Rule1(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ switch (s0)
	{

	case -1:
	if(!(!(AvatarGen.AutoGenerateCharacters)))
	{

	s0 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	
	counter20 = -1;
	if((((Enumerable.Range(0,((1) + (((((AvatarGen.Amount) - (1))) - (0))))).ToList<System.Int32>()).Count) == (0)))
	{

	goto case 0;	}else
	{

	___x00 = (Enumerable.Range(0,((1) + (((((AvatarGen.Amount) - (1))) - (0))))).ToList<System.Int32>())[0];
	goto case 2;	}
	case 2:
	counter20 = ((counter20) + (1));
	if((((((Enumerable.Range(0,((1) + (((((AvatarGen.Amount) - (1))) - (0))))).ToList<System.Int32>()).Count) == (counter20))) || (((counter20) > ((Enumerable.Range(0,((1) + (((((AvatarGen.Amount) - (1))) - (0))))).ToList<System.Int32>()).Count)))))
	{

	goto case 0;	}else
	{

	___x00 = (Enumerable.Range(0,((1) + (((((AvatarGen.Amount) - (1))) - (0))))).ToList<System.Int32>())[counter20];
	goto case 3;	}
	case 3:
	Persons = new Cons<Person>(new Person((

(AvatarGen.SettingsList).Select(__ContextSymbol1 => new { ___characteristic00 = __ContextSymbol1 })
.Select(__ContextSymbol2 => new Casanova.Prelude.Tuple<System.Int32, System.Int32>(__ContextSymbol2.___characteristic00.Item1,UnityEngine.Random.Range(__ContextSymbol2.___characteristic00.Item2.Item1,__ContextSymbol2.___characteristic00.Item2.Item2)))
.ToList<Casanova.Prelude.Tuple<System.Int32, System.Int32>>()).ToList<Casanova.Prelude.Tuple<System.Int32, System.Int32>>(),"random"), (Persons)).ToList<Person>();
	s0 = 2;
return;
	case 0:
	if(!(false))
	{

	s0 = 0;
return;	}else
	{

	s0 = -1;
return;	}	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ switch (s1)
	{

	case -1:
	if(!(AvatarGen.AutoGenerateCharacters))
	{

	s1 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	
	counter21 = -1;
	if((((PersonParser.ParsedPersons).Count) == (0)))
	{

	goto case 0;	}else
	{

	___parsedperson10 = (PersonParser.ParsedPersons)[0];
	goto case 2;	}
	case 2:
	counter21 = ((counter21) + (1));
	if((((((PersonParser.ParsedPersons).Count) == (counter21))) || (((counter21) > ((PersonParser.ParsedPersons).Count)))))
	{

	goto case 0;	}else
	{

	___parsedperson10 = (PersonParser.ParsedPersons)[counter21];
	goto case 3;	}
	case 3:
	___personalities10 = ___parsedperson10.Item2;
	Persons = new Cons<Person>(new Person((

(___personalities10).Select(__ContextSymbol3 => new { ___personality10 = __ContextSymbol3 })
.Select(__ContextSymbol4 => new Casanova.Prelude.Tuple<System.Int32, System.Int32>(__ContextSymbol4.___personality10.Item1,__ContextSymbol4.___personality10.Item2))
.ToList<Casanova.Prelude.Tuple<System.Int32, System.Int32>>()).ToList<Casanova.Prelude.Tuple<System.Int32, System.Int32>>(),___parsedperson10.Item1), (Persons)).ToList<Person>();
	s1 = 2;
return;
	case 0:
	if(!(false))
	{

	s1 = 0;
return;	}else
	{

	s1 = -1;
return;	}	
	default: return;}}
	





}
public class Player{
public int frame;
public bool JustEntered = true;
private EventController controller;
	public int ID;
public Player(EventController controller)
	{JustEntered = false;
 frame = World.frame;
		UnityPlayer ___unity_player00;
		___unity_player00 = UnityPlayer.Initialize();
		isTriggeringEvent = false;
		eventController = controller;
		UnityPlayer = ___unity_player00;
		
}
		public UnityEngine.Vector3 Position{  get { return UnityPlayer.Position; }
 }
	public UnityPlayer UnityPlayer;
	public System.Boolean enabled{  get { return UnityPlayer.enabled; }
  set{UnityPlayer.enabled = value; }
 }
	public EventController eventController;
	public UnityEngine.GameObject gameObject{  get { return UnityPlayer.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityPlayer.hideFlags; }
  set{UnityPlayer.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityPlayer.isActiveAndEnabled; }
 }
	public System.Boolean isTriggeringEvent;
	public System.String name{  get { return UnityPlayer.name; }
  set{UnityPlayer.name = value; }
 }
	public System.String tag{  get { return UnityPlayer.tag; }
  set{UnityPlayer.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityPlayer.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityPlayer.useGUILayout; }
  set{UnityPlayer.useGUILayout = value; }
 }
	public Event ___event00;
	public System.Int32 counter10;
	public System.Single count_down1;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World world){ switch (s0)
	{

	case -1:
	
	counter10 = -1;
	if((((eventController.AllPlayerEvents).Count) == (0)))
	{

	s0 = -1;
return;	}else
	{

	___event00 = (eventController.AllPlayerEvents)[0];
	goto case 1;	}
	case 1:
	counter10 = ((counter10) + (1));
	if((((((eventController.AllPlayerEvents).Count) == (counter10))) || (((counter10) > ((eventController.AllPlayerEvents).Count)))))
	{

	s0 = -1;
return;	}else
	{

	___event00 = (eventController.AllPlayerEvents)[counter10];
	goto case 2;	}
	case 2:
	if(UnityEngine.Input.GetKey(___event00.TriggerKey))
	{

	goto case 4;	}else
	{

	s0 = 1;
return;	}
	case 4:
	HelperFunctions.Log("Player triggered event");
	UnityPlayer.TriggerPlayerEvent(___event00._eventObject);
	eventController.CurrentEvents = new Cons<Event>(___event00, (eventController.CurrentEvents)).ToList<Event>();
	s0 = 5;
return;
	case 5:
	count_down1 = 1f;
	goto case 6;
	case 6:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s0 = 6;
return;	}else
	{

	s0 = 1;
return;	}	
	default: return;}}
	






}
public class Event{
public int frame;
public bool JustEntered = true;
private System.String Type;
private EventController controller;
	public int ID;
public Event(System.String Type, EventController controller)
	{JustEntered = false;
 frame = World.frame;
		UnityEvent ___unity_event00;
		___unity_event00 = UnityEvent.SpawnRandomEvent(Type);
		eventController = controller;
		UnityEvent = ___unity_event00;
		
}
		public System.Int32 AmountOfEvents{  get { return UnityEvent.AmountOfEvents; }
  set{UnityEvent.AmountOfEvents = value; }
 }
	public System.Int32 AmountOfParticipants{  get { return UnityEvent.AmountOfParticipants; }
  set{UnityEvent.AmountOfParticipants = value; }
 }
	public System.Int32 AmountOfPlayerEvents{  get { return UnityEvent.AmountOfPlayerEvents; }
  set{UnityEvent.AmountOfPlayerEvents = value; }
 }
	public System.Int32 Completeness{  get { return UnityEvent.Completeness; }
  set{UnityEvent.Completeness = value; }
 }
	public UnityEngine.GameObject GameObject{  get { return UnityEvent.GameObject; }
 }
	public System.Int32 Id{  get { return UnityEvent.Id; }
  set{UnityEvent.Id = value; }
 }
	public System.Int32 InterestLevel{  get { return UnityEvent.InterestLevel; }
  set{UnityEvent.InterestLevel = value; }
 }
	public System.Boolean IsDestroyed{  get { return UnityEvent.IsDestroyed; }
 }
	public System.Boolean IsPlayerControlled{  get { return UnityEvent.IsPlayerControlled; }
  set{UnityEvent.IsPlayerControlled = value; }
 }
	public System.Int32 MaxAmountOfParticipants{  get { return UnityEvent.MaxAmountOfParticipants; }
  set{UnityEvent.MaxAmountOfParticipants = value; }
 }
	public System.Collections.Generic.List<Casanova.Prelude.Tuple<System.Int32,System.Int32>> PersonalityMinimums{  get { return UnityEvent.PersonalityMinimums; }
  set{UnityEvent.PersonalityMinimums = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityEvent.Position; }
  set{UnityEvent.Position = value; }
 }
	public System.Single Radius{  get { return UnityEvent.Radius; }
  set{UnityEvent.Radius = value; }
 }
	public System.String TriggerKey{  get { return UnityEvent.TriggerKey; }
  set{UnityEvent.TriggerKey = value; }
 }
	public UnityEvent UnityEvent;
	public EventObject _eventObject{  get { return UnityEvent._eventObject; }
  set{UnityEvent._eventObject = value; }
 }
	public EventController eventController;
	public System.Single count_down2;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World world){ switch (s0)
	{

	case -1:
	count_down2 = 2.5f;
	goto case 11;
	case 11:
	if(((count_down2) > (0f)))
	{

	count_down2 = ((count_down2) - (dt));
	s0 = 11;
return;	}else
	{

	goto case 2;	}
	case 2:
	if(UnityEventController.IsEventReady(this.Id))
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	HelperFunctions.Log(Completeness);
	Completeness = ((Completeness) + (10));
	s0 = 3;
return;
	case 3:
	if(((((Completeness) > (100))) || (((Completeness) == (100)))))
	{

	goto case 4;	}else
	{

	s0 = -1;
return;	}
	case 4:
	UnityEvent.Destroy();
	Completeness = Completeness;
	s0 = -1;
return;
	case 1:
	Completeness = 0;
	s0 = -1;
return;	
	default: return;}}
	






}
public class EventController{
public int frame;
public bool JustEntered = true;
	public int ID;
public EventController()
	{JustEntered = false;
 frame = World.frame;
		UnityEventController = new UnityEventController();
		CurrentEvents = (

Enumerable.Empty<Event>()).ToList<Event>();
		AllPlayerEvents = (

Enumerable.Empty<Event>()).ToList<Event>();
		
}
		public List<Event> AllPlayerEvents;
	public List<Event> CurrentEvents;
	public System.Collections.Generic.List<EventObject> PlayerEventsList{  get { return UnityEventController.PlayerEventsList; }
 }
	public UnityEventController UnityEventController;
	public System.Single count_down3;
	public System.Int32 ___x21;
	public System.Int32 counter22;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		for(int x0 = 0; x0 < AllPlayerEvents.Count; x0++) { 
			AllPlayerEvents[x0].Update(dt, world);
		}
		for(int x0 = 0; x0 < CurrentEvents.Count; x0++) { 
			CurrentEvents[x0].Update(dt, world);
		}
	}





	int s0=-1;
	public void Rule0(float dt, World world){ switch (s0)
	{

	case -1:
	if(!(((CurrentEvents.Count) > (0))))
	{

	s0 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	CurrentEvents = (

(CurrentEvents).Select(__ContextSymbol9 => new { ___event01 = __ContextSymbol9 })
.Where(__ContextSymbol10 => !(__ContextSymbol10.___event01.IsDestroyed))
.Select(__ContextSymbol11 => __ContextSymbol11.___event01)
.ToList<Event>()).ToList<Event>();
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ switch (s1)
	{

	case -1:
	count_down3 = UnityEngine.Random.Range(50f,100f);
	goto case 4;
	case 4:
	if(((count_down3) > (0f)))
	{

	count_down3 = ((count_down3) - (dt));
	s1 = 4;
return;	}else
	{

	goto case 2;	}
	case 2:
	HelperFunctions.Log("Spawned event!");
	goto case 1;
	case 1:
	if(!(UnityEventController.IsEventAvailable()))
	{

	s1 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	CurrentEvents = new Cons<Event>(new Event("normalEvent",this), (CurrentEvents)).ToList<Event>();
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ switch (s2)
	{

	case -1:
	
	counter22 = -1;
	if((((Enumerable.Range(0,((1) + (((((PlayerEventsList.Count) - (1))) - (0))))).ToList<System.Int32>()).Count) == (0)))
	{

	goto case 0;	}else
	{

	___x21 = (Enumerable.Range(0,((1) + (((((PlayerEventsList.Count) - (1))) - (0))))).ToList<System.Int32>())[0];
	goto case 2;	}
	case 2:
	counter22 = ((counter22) + (1));
	if((((((Enumerable.Range(0,((1) + (((((PlayerEventsList.Count) - (1))) - (0))))).ToList<System.Int32>()).Count) == (counter22))) || (((counter22) > ((Enumerable.Range(0,((1) + (((((PlayerEventsList.Count) - (1))) - (0))))).ToList<System.Int32>()).Count)))))
	{

	goto case 0;	}else
	{

	___x21 = (Enumerable.Range(0,((1) + (((((PlayerEventsList.Count) - (1))) - (0))))).ToList<System.Int32>())[counter22];
	goto case 3;	}
	case 3:
	AllPlayerEvents = new Cons<Event>(new Event("playerEvent",this), (AllPlayerEvents)).ToList<Event>();
	s2 = 2;
return;
	case 0:
	if(!(false))
	{

	s2 = 0;
return;	}else
	{

	s2 = -1;
return;	}	
	default: return;}}
	





}
public class Person{
public int frame;
public bool JustEntered = true;
private List<Casanova.Prelude.Tuple<System.Int32, System.Int32>> Settings;
private System.String modelName;
	public int ID;
public Person(List<Casanova.Prelude.Tuple<System.Int32, System.Int32>> Settings, System.String modelName)
	{JustEntered = false;
 frame = World.frame;
		UnityNpc ___unity_npc00;
		___unity_npc00 = UnityNpc.Spawn(Settings,modelName);
		settings = Settings;
		actionIds = (

Enumerable.Empty<System.Int32>()).ToList<System.Int32>();
		UnityNpc = ___unity_npc00;
		TimeToInteract = false;
		PositionAvailable = false;
		
}
		public System.Collections.Generic.List<System.Int32> ActionsToPerform{  get { return UnityNpc.ActionsToPerform; }
 }
	public UnityEngine.Vector3 CurrentActionPosition{  get { return UnityNpc.CurrentActionPosition; }
 }
	public System.Int32 Id{  get { return UnityNpc.Id; }
 }
	public System.Boolean Interacting{  get { return UnityNpc.Interacting; }
  set{UnityNpc.Interacting = value; }
 }
	public System.Boolean InteractionTarget{  get { return UnityNpc.InteractionTarget; }
  set{UnityNpc.InteractionTarget = value; }
 }
	public System.Boolean IsInEvent{  get { return UnityNpc.IsInEvent; }
  set{UnityNpc.IsInEvent = value; }
 }
	public System.Boolean IsInEventRadius{  get { return UnityNpc.IsInEventRadius; }
  set{UnityNpc.IsInEventRadius = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityNpc.Position; }
  set{UnityNpc.Position = value; }
 }
	public System.Boolean PositionAvailable;
	public System.Boolean TimeToInteract;
	public UnityNpc UnityNpc;
	public List<System.Int32> actionIds;
	public System.Boolean enabled{  get { return UnityNpc.enabled; }
  set{UnityNpc.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityNpc.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityNpc.hideFlags; }
  set{UnityNpc.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityNpc.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnityNpc.name; }
  set{UnityNpc.name = value; }
 }
	public List<Casanova.Prelude.Tuple<System.Int32, System.Int32>> settings;
	public System.String tag{  get { return UnityNpc.tag; }
  set{UnityNpc.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityNpc.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityNpc.useGUILayout; }
  set{UnityNpc.useGUILayout = value; }
 }
	public System.Single count_down4;
	public System.Single count_down5;
	public System.Single count_down6;
	public System.Int32 ___actionToExecute40;
	public System.Int32 ___actionToExecute51;
	public UnityEngine.Vector3 ___destination50;
	public System.Single ___distanceToDestination50;
	public System.Single count_down7;
	public System.Single count_down8;
	public System.Single count_down10;
	public System.Single count_down9;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
		this.Rule6(dt, world);
		this.Rule7(dt, world);
		this.Rule8(dt, world);
		this.Rule9(dt, world);
		this.Rule10(dt, world);
		this.Rule11(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ switch (s0)
	{

	case -1:
	count_down4 = 1f;
	goto case 2;
	case 2:
	if(((count_down4) > (0f)))
	{

	count_down4 = ((count_down4) - (dt));
	s0 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	IsInEvent = UnityNpc.IsInterestedInEvent();
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ switch (s1)
	{

	case -1:
	if(!(IsInEvent))
	{

	s1 = -1;
return;	}else
	{

	goto case 3;	}
	case 3:
	UnityNpc.UpdateCurrentNodesCollection();
	actionIds = UnityNpc.ActionsToPerform;
	PositionAvailable = false;
	s1 = 0;
return;
	case 0:
	count_down5 = 2f;
	goto case 1;
	case 1:
	if(((count_down5) > (0f)))
	{

	count_down5 = ((count_down5) - (dt));
	s1 = 1;
return;	}else
	{

	s1 = -1;
return;	}	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ switch (s2)
	{

	case -1:
	if(!(IsInEvent))
	{

	s2 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	if(!(!(IsInEvent)))
	{

	s2 = 2;
return;	}else
	{

	goto case 1;	}
	case 1:
	UnityNpc.UpdateCurrentNodesCollection();
	actionIds = UnityNpc.ActionsToPerform;
	PositionAvailable = false;
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ switch (s3)
	{

	case -1:
	actionIds = actionIds;
	PositionAvailable = false;
	s3 = 4;
return;
	case 4:
	UnityNpc.UpdateCurrentNodesCollection();
	actionIds = UnityNpc.ActionsToPerform;
	PositionAvailable = false;
	s3 = 1;
return;
	case 1:
	count_down6 = 25f;
	goto case 2;
	case 2:
	if(((count_down6) > (0f)))
	{

	count_down6 = ((count_down6) - (dt));
	s3 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	if(!(!(IsInEvent)))
	{

	s3 = 0;
return;	}else
	{

	s3 = -1;
return;	}	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ switch (s4)
	{

	case -1:
	if(!(!(PositionAvailable)))
	{

	s4 = -1;
return;	}else
	{

	goto case 7;	}
	case 7:
	if(!(((actionIds.Count) > (0))))
	{

	s4 = 7;
return;	}else
	{

	goto case 6;	}
	case 6:
	___actionToExecute40 = (actionIds)[0];
	if(UnityNpc.IsPositionAvailable(___actionToExecute40))
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	UnityNpc.ClaimPosition(___actionToExecute40);
	actionIds = actionIds;
	PositionAvailable = true;
	s4 = -1;
return;
	case 1:
	actionIds = (

(actionIds).Select(__ContextSymbol14 => new { ___id40 = __ContextSymbol14 })
.Where(__ContextSymbol15 => !(((__ContextSymbol15.___id40) == (___actionToExecute40))))
.Select(__ContextSymbol16 => __ContextSymbol16.___id40)
.ToList<System.Int32>()).ToList<System.Int32>();
	PositionAvailable = false;
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ switch (s5)
	{

	case -1:
	if(!(PositionAvailable))
	{

	s5 = -1;
return;	}else
	{

	goto case 11;	}
	case 11:
	if(!(((actionIds.Count) > (0))))
	{

	s5 = 11;
return;	}else
	{

	goto case 10;	}
	case 10:
	___actionToExecute51 = (actionIds)[0];
	___destination50 = UnityNpc.GetClaimedPosition(___actionToExecute51);
	UnityNpc.MoveTo(___actionToExecute51);
	___distanceToDestination50 = UnityEngine.Vector3.Distance(Position,___destination50);
	if(((2.2f) > (___distanceToDestination50)))
	{

	goto case 1;	}else
	{

	s5 = -1;
return;	}
	case 1:
	UnityNpc.UpdateAccumulatedValues(___actionToExecute51);
	count_down7 = UnityNpc.PlayAnimation(___actionToExecute51);
	goto case 5;
	case 5:
	if(((count_down7) > (0f)))
	{

	count_down7 = ((count_down7) - (dt));
	s5 = 5;
return;	}else
	{

	goto case 3;	}
	case 3:
	UnityNpc.RemoveClaimToPosition(___actionToExecute51);
	actionIds = (

(actionIds).Select(__ContextSymbol17 => new { ___id51 = __ContextSymbol17 })
.Where(__ContextSymbol18 => !(((__ContextSymbol18.___id51) == (___actionToExecute51))))
.Select(__ContextSymbol19 => __ContextSymbol19.___id51)
.ToList<System.Int32>()).ToList<System.Int32>();
	PositionAvailable = false;
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ switch (s6)
	{

	case -1:
	if(!(InteractionTarget))
	{

	s6 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	if(!(!(Interacting)))
	{

	s6 = 2;
return;	}else
	{

	goto case 1;	}
	case 1:
	UnityNpc.UpdateCurrentNodesCollection();
	actionIds = UnityNpc.ActionsToPerform;
	PositionAvailable = false;
	s6 = -1;
return;	
	default: return;}}
	

	int s7=-1;
	public void Rule7(float dt, World world){ switch (s7)
	{

	case -1:
	if(!(Interacting))
	{

	s7 = -1;
return;	}else
	{

	goto case 3;	}
	case 3:
	if(!(!(Interacting)))
	{

	s7 = 3;
return;	}else
	{

	goto case 2;	}
	case 2:
	UnityNpc.FreeInteractionTarget();
	UnityNpc.UpdateCurrentNodesCollection();
	actionIds = UnityNpc.ActionsToPerform;
	PositionAvailable = false;
	s7 = -1;
return;	
	default: return;}}
	

	int s8=-1;
	public void Rule8(float dt, World world){ switch (s8)
	{

	case -1:
	if(!(Interacting))
	{

	s8 = -1;
return;	}else
	{

	goto case 3;	}
	case 3:
	UnityNpc.UpdateCurrentNodesCollection();
	actionIds = UnityNpc.ActionsToPerform;
	PositionAvailable = false;
	s8 = 0;
return;
	case 0:
	count_down8 = 2f;
	goto case 1;
	case 1:
	if(((count_down8) > (0f)))
	{

	count_down8 = ((count_down8) - (dt));
	s8 = 1;
return;	}else
	{

	s8 = -1;
return;	}	
	default: return;}}
	

	int s9=-1;
	public void Rule9(float dt, World world){ switch (s9)
	{

	case -1:
	if(!(TimeToInteract))
	{

	s9 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	if(UnityNpc.InteractionAvailable())
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	Interacting = true;
	s9 = 3;
return;
	case 3:
	if(!(!(TimeToInteract)))
	{

	s9 = 3;
return;	}else
	{

	s9 = -1;
return;	}
	case 1:
	Interacting = false;
	s9 = -1;
return;	
	default: return;}}
	

	int s10=-1;
	public void Rule10(float dt, World world){ switch (s10)
	{

	case -1:
	if(((TimeToInteract) == (false)))
	{

	goto case 7;	}else
	{

	goto case 8;	}
	case 7:
	Interacting = false;
	s10 = -1;
return;
	case 8:
	Interacting = Interacting;
	s10 = -1;
return;	
	default: return;}}
	

	int s11=-1;
	public void Rule11(float dt, World world){ switch (s11)
	{

	case -1:
	TimeToInteract = false;
	s11 = 4;
return;
	case 4:
	count_down10 = 15f;
	goto case 5;
	case 5:
	if(((count_down10) > (0f)))
	{

	count_down10 = ((count_down10) - (dt));
	s11 = 5;
return;	}else
	{

	goto case 3;	}
	case 3:
	TimeToInteract = true;
	s11 = 1;
return;
	case 1:
	count_down9 = 5f;
	goto case 2;
	case 2:
	if(((count_down9) > (0f)))
	{

	count_down9 = ((count_down9) - (dt));
	s11 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	TimeToInteract = false;
	s11 = -1;
return;	
	default: return;}}
	





}
}                                                                                                                                       