#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Games
{
    public class World : MonoBehaviour
    {
        public static int frame;

        void Update()
        {
            Update(Time.deltaTime, this);
            frame++;
        }

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
            AvatarGen = ___avaGen00;

        }

        public AvatarGenerator AvatarGen;
        public List<Person> __Persons;

        public List<Person> Persons
        {
            get { return __Persons; }
            set
            {
                __Persons = value;
                foreach (var e in value)
                {
                    if (e.JustEntered)
                    {
                        e.JustEntered = false;
                    }
                }
            }
        }

        public EventController __eventController;

        public EventController eventController
        {
            get { return __eventController; }
            set
            {
                __eventController = value;
                if (!value.JustEntered) __eventController = value;
                else
                {
                    value.JustEntered = false;
                }
            }
        }

        public System.Int32 ___x00;
        public System.Int32 counter20;

        System.DateTime init_time = System.DateTime.Now;

        public void Update(float dt, World world)
        {
            var t = System.DateTime.Now;

            for (int x0 = 0; x0 < Persons.Count; x0++)
            {
                Persons[x0].Update(dt, world);
            }
            eventController.Update(dt, world);
            this.Rule0(dt, world);

        }





        int s0 = -1;

        public void Rule0(float dt, World world)
        {
            switch (s0)
            {

                case -1:

                    counter20 = -1;
                    if (
                    (((Enumerable.Range(0, ((1) + (((((AvatarGen.Amount) - (1))) - (0))))).ToList<System.Int32>())
                         .Count) == (0)))
                    {

                        goto case 0;
                    }
                    else
                    {

                        ___x00 =
                            (Enumerable.Range(0, ((1) + (((((AvatarGen.Amount) - (1))) - (0))))).ToList<System.Int32>())
                                [0];
                        goto case 2;
                    }
                case 2:
                    counter20 = ((counter20) + (1));
                    if (
                    (((((Enumerable.Range(0, ((1) + (((((AvatarGen.Amount) - (1))) - (0))))).ToList<System.Int32>())
                           .Count) == (counter20))) ||
                     (((counter20) >
                       ((Enumerable.Range(0, ((1) + (((((AvatarGen.Amount) - (1))) - (0))))).ToList<System.Int32>())
                           .Count)))))
                    {

                        goto case 0;
                    }
                    else
                    {

                        ___x00 =
                            (Enumerable.Range(0, ((1) + (((((AvatarGen.Amount) - (1))) - (0))))).ToList<System.Int32>())
                                [counter20];
                        goto case 3;
                    }
                case 3:
                    Persons = new Cons<Person>(new Person((

                            (AvatarGen.SettingsList).Select(__ContextSymbol1 => new {___characteristic00 = __ContextSymbol1})
                                .Select(
                                    __ContextSymbol2 =>
                                        new Casanova.Prelude.Tuple<System.Int32, System.Int32>(
                                            __ContextSymbol2.___characteristic00.Item1,
                                            UnityEngine.Random.Range(__ContextSymbol2.___characteristic00.Item2.Item1,
                                                __ContextSymbol2.___characteristic00.Item2.Item2)))
                                .ToList<Casanova.Prelude.Tuple<System.Int32, System.Int32>>())
                        .ToList<Casanova.Prelude.Tuple<System.Int32, System.Int32>>()), (Persons)).ToList<Person>();
                    s0 = 2;
                    return;
                case 0:
                    if (!(false))
                    {

                        s0 = 0;
                        return;
                    }
                    else
                    {

                        s0 = -1;
                        return;
                    }
                default:
                    return;
            }
        }







    }

    public class Player
    {
        public int frame;
        public bool JustEntered = true;
        private EventController controller;
        public int ID;

        public Player(EventController controller)
        {
            JustEntered = false;
            frame = World.frame;
            UnityPlayer ___unity_player00;
            ___unity_player00 = UnityPlayer.Spawn();
            isTriggeringEvent = false;
            eventController = controller;
            UnityPlayer = ___unity_player00;

        }

        public UnityEngine.Vector3 Position
        {
            get { return UnityPlayer.Position; }
        }

        public UnityPlayer UnityPlayer;

        public System.Boolean enabled
        {
            get { return UnityPlayer.enabled; }
            set { UnityPlayer.enabled = value; }
        }

        public EventController eventController;

        public UnityEngine.GameObject gameObject
        {
            get { return UnityPlayer.gameObject; }
        }

        public UnityEngine.HideFlags hideFlags
        {
            get { return UnityPlayer.hideFlags; }
            set { UnityPlayer.hideFlags = value; }
        }

        public System.Boolean isActiveAndEnabled
        {
            get { return UnityPlayer.isActiveAndEnabled; }
        }

        public System.Boolean isTriggeringEvent;

        public System.String name
        {
            get { return UnityPlayer.name; }
            set { UnityPlayer.name = value; }
        }

        public System.String tag
        {
            get { return UnityPlayer.tag; }
            set { UnityPlayer.tag = value; }
        }

        public UnityEngine.Transform transform
        {
            get { return UnityPlayer.transform; }
        }

        public System.Boolean useGUILayout
        {
            get { return UnityPlayer.useGUILayout; }
            set { UnityPlayer.useGUILayout = value; }
        }

        public EventObject ___event00;
        public System.Int32 counter10;
        public System.Single count_down1;

        public void Update(float dt, World world)
        {
            frame = World.frame;

            this.Rule0(dt, world);

        }





        int s0 = -1;

        public void Rule0(float dt, World world)
        {
            switch (s0)
            {

                case -1:
                    isTriggeringEvent = isTriggeringEvent;
                    s0 = 0;
                    return;
                case 0:

                    counter10 = -1;
                    if ((((eventController.PlayerEventsList).Count) == (0)))
                    {

                        s0 = -1;
                        return;
                    }
                    else
                    {

                        ___event00 = (eventController.PlayerEventsList)[0];
                        goto case 1;
                    }
                case 1:
                    counter10 = ((counter10) + (1));
                    if ((((((eventController.PlayerEventsList).Count) == (counter10))) ||
                         (((counter10) > ((eventController.PlayerEventsList).Count)))))
                    {

                        s0 = -1;
                        return;
                    }
                    else
                    {

                        ___event00 = (eventController.PlayerEventsList)[counter10];
                        goto case 2;
                    }
                case 2:
                    if (UnityEngine.Input.GetKey(KeyCode.F))
                    {

                        goto case 4;
                    }
                    else
                    {

                        s0 = 1;
                        return;
                    }
                case 4:
                    if (UnityPlayer.IsLookingAt(___event00.gameObject))
                    {

                        goto case 5;
                    }
                    else
                    {

                        goto case 6;
                    }
                case 5:
                    HelperFunctions.Log("Player triggered event");
                    UnityPlayer.TriggerEvent(___event00);
                    isTriggeringEvent = true;
                    s0 = 8;
                    return;
                case 8:
                    count_down1 = 0;
                    goto case 9;
                case 9:
                    if (((count_down1) > (0f)))
                    {

                        count_down1 = ((count_down1) - (dt));
                        s0 = 9;
                        return;
                    }
                    else
                    {

                        s0 = 1;
                        return;
                    }
                case 6:
                    isTriggeringEvent = false;
                    s0 = 1;
                    return;
                default:
                    return;
            }
        }







    }

    public class Event
    {
        public int frame;
        public bool JustEntered = true;
        public int ID;

        public Event()
        {
            JustEntered = false;
            frame = World.frame;
            UnityEvent ___unity_event00;
            ___unity_event00 = UnityEvent.SpawnRandomEvent();
            UnityEvent = ___unity_event00;

        }

        public System.Int32 AmountOfEvents
        {
            get { return UnityEvent.AmountOfEvents; }
            set { UnityEvent.AmountOfEvents = value; }
        }

        public System.Int32 AmountOfParticipants
        {
            get { return UnityEvent.AmountOfParticipants; }
            set { UnityEvent.AmountOfParticipants = value; }
        }

        public System.Int32 AmountOfPlayerEvents
        {
            get { return UnityEvent.AmountOfPlayerEvents; }
            set { UnityEvent.AmountOfPlayerEvents = value; }
        }

        public System.Int32 Completeness
        {
            get { return UnityEvent.Completeness; }
            set { UnityEvent.Completeness = value; }
        }

        public UnityEngine.GameObject GameObject
        {
            get { return UnityEvent.GameObject; }
        }

        public System.Int32 Id
        {
            get { return UnityEvent.Id; }
            set { UnityEvent.Id = value; }
        }

        public System.Int32 InterestLevel
        {
            get { return UnityEvent.InterestLevel; }
            set { UnityEvent.InterestLevel = value; }
        }

        public System.Boolean IsDestroyed
        {
            get { return UnityEvent.IsDestroyed; }
        }

        public System.Boolean IsPlayerControlled
        {
            get { return UnityEvent.IsPlayerControlled; }
            set { UnityEvent.IsPlayerControlled = value; }
        }

        public System.Int32 MaxAmountOfParticipants
        {
            get { return UnityEvent.MaxAmountOfParticipants; }
            set { UnityEvent.MaxAmountOfParticipants = value; }
        }

        public System.Collections.Generic.List<Casanova.Prelude.Tuple<System.Int32, System.Int32>> PersonalityMinimums
        {
            get { return UnityEvent.PersonalityMinimums; }
            set { UnityEvent.PersonalityMinimums = value; }
        }

        public UnityEngine.Vector3 Position
        {
            get { return UnityEvent.Position; }
            set { UnityEvent.Position = value; }
        }

        public System.Single Radius
        {
            get { return UnityEvent.Radius; }
            set { UnityEvent.Radius = value; }
        }

        public UnityEvent UnityEvent;
        public System.Single count_down2;

        public void Update(float dt, World world)
        {
            frame = World.frame;

            this.Rule0(dt, world);

        }





        int s0 = -1;

        public void Rule0(float dt, World world)
        {
            switch (s0)
            {

                case -1:
                    count_down2 = 1f;
                    goto case 6;
                case 6:
                    if (((count_down2) > (0f)))
                    {

                        count_down2 = ((count_down2) - (dt));
                        s0 = 6;
                        return;
                    }
                    else
                    {

                        goto case 4;
                    }
                case 4:
                    HelperFunctions.Log(Completeness);
                    Completeness = ((Completeness) + (10));
                    s0 = 0;
                    return;
                case 0:
                    if (((((Completeness) > (100))) || (((Completeness) == (100)))))
                    {

                        goto case 1;
                    }
                    else
                    {

                        s0 = -1;
                        return;
                    }
                case 1:
                    UnityEvent.Destroy();
                    s0 = -1;
                    return;
                default:
                    return;
            }
        }







    }

    public class EventController
    {
        public int frame;
        public bool JustEntered = true;
        public int ID;

        public EventController()
        {
            JustEntered = false;
            frame = World.frame;
            UnityEventController = new UnityEventController();
            CurrentEvents = (

                Enumerable.Empty<Event>()).ToList<Event>();

        }

        public List<Event> CurrentEvents;

        public System.Collections.Generic.List<EventObject> PlayerEventsList
        {
            get { return UnityEventController.PlayerEventsList; }
        }

        public UnityEventController UnityEventController;
        public System.Single count_down3;

        public void Update(float dt, World world)
        {
            frame = World.frame;

            this.Rule0(dt, world);
            this.Rule1(dt, world);
            for (int x0 = 0; x0 < CurrentEvents.Count; x0++)
            {
                CurrentEvents[x0].Update(dt, world);
            }
        }





        int s0 = -1;

        public void Rule0(float dt, World world)
        {
            switch (s0)
            {

                case -1:
                    if (!(((CurrentEvents.Count) > (0))))
                    {

                        s0 = -1;
                        return;
                    }
                    else
                    {

                        goto case 0;
                    }
                case 0:
                    CurrentEvents = (

                        (CurrentEvents).Select(__ContextSymbol5 => new {___event01 = __ContextSymbol5})
                            .Where(__ContextSymbol6 => !(__ContextSymbol6.___event01.IsDestroyed))
                            .Select(__ContextSymbol7 => __ContextSymbol7.___event01)
                            .ToList<Event>()).ToList<Event>();
                    s0 = -1;
                    return;
                default:
                    return;
            }
        }


        int s1 = -1;

        public void Rule1(float dt, World world)
        {
            switch (s1)
            {

                case -1:
                    count_down3 = 50f;
                    goto case 4;
                case 4:
                    if (((count_down3) > (0f)))
                    {

                        count_down3 = ((count_down3) - (dt));
                        s1 = 4;
                        return;
                    }
                    else
                    {

                        goto case 2;
                    }
                case 2:
                    HelperFunctions.Log("Spawned event!");
                    goto case 1;
                case 1:
                    if (!(UnityEventController.IsEventAvailable()))
                    {

                        s1 = 1;
                        return;
                    }
                    else
                    {

                        goto case 0;
                    }
                case 0:
                    CurrentEvents = new Cons<Event>(new Event(), (CurrentEvents)).ToList<Event>();
                    s1 = -1;
                    return;
                default:
                    return;
            }
        }






    }

    public class Person
    {
        public int frame;
        public bool JustEntered = true;
        private List<Casanova.Prelude.Tuple<System.Int32, System.Int32>> Settings;
        public int ID;

        public Person(List<Casanova.Prelude.Tuple<System.Int32, System.Int32>> Settings)
        {
            JustEntered = false;
            frame = World.frame;
            UnityNpc ___unity_npc00;
            ___unity_npc00 = UnityNpc.Spawn(Settings);
            settings = Settings;
            actionIds = (

                Enumerable.Empty<System.Int32>()).ToList<System.Int32>();
            UnityNpc = ___unity_npc00;
            PositionAvailable = false;

        }

        public System.Collections.Generic.List<System.Int32> ActionsToPerform
        {
            get { return UnityNpc.ActionsToPerform; }
        }

        public UnityEngine.Vector3 CurrentActionPosition
        {
            get { return UnityNpc.CurrentActionPosition; }
        }

        public System.Int32 Id
        {
            get { return UnityNpc.Id; }
        }

        public System.Boolean IsInEvent
        {
            get { return UnityNpc.IsInEvent; }
            set { UnityNpc.IsInEvent = value; }
        }

        public System.Boolean IsInEventRadius
        {
            get { return UnityNpc.IsInEventRadius; }
            set { UnityNpc.IsInEventRadius = value; }
        }

        public UnityEngine.Vector3 Position
        {
            get { return UnityNpc.Position; }
            set { UnityNpc.Position = value; }
        }

        public System.Boolean PositionAvailable;
        public UnityNpc UnityNpc;
        public List<System.Int32> actionIds;

        public System.Boolean enabled
        {
            get { return UnityNpc.enabled; }
            set { UnityNpc.enabled = value; }
        }

        public UnityEngine.GameObject gameObject
        {
            get { return UnityNpc.gameObject; }
        }

        public UnityEngine.HideFlags hideFlags
        {
            get { return UnityNpc.hideFlags; }
            set { UnityNpc.hideFlags = value; }
        }

        public System.Boolean isActiveAndEnabled
        {
            get { return UnityNpc.isActiveAndEnabled; }
        }

        public System.String name
        {
            get { return UnityNpc.name; }
            set { UnityNpc.name = value; }
        }

        public List<Casanova.Prelude.Tuple<System.Int32, System.Int32>> settings;

        public System.String tag
        {
            get { return UnityNpc.tag; }
            set { UnityNpc.tag = value; }
        }

        public UnityEngine.Transform transform
        {
            get { return UnityNpc.transform; }
        }

        public System.Boolean useGUILayout
        {
            get { return UnityNpc.useGUILayout; }
            set { UnityNpc.useGUILayout = value; }
        }

        public System.Single count_down4;
        public System.Single count_down5;
        public System.Single count_down6;
        public System.Int32 ___actionToExecute40;
        public System.Int32 ___actionToExecute51;
        public UnityEngine.Vector3 ___destination50;
        public System.Single ___distanceToDestination50;
        public System.Single count_down7;

        public void Update(float dt, World world)
        {
            frame = World.frame;

            this.Rule0(dt, world);
            this.Rule1(dt, world);
            this.Rule2(dt, world);
            this.Rule3(dt, world);
            this.Rule4(dt, world);
            this.Rule5(dt, world);
        }





        int s0 = -1;

        public void Rule0(float dt, World world)
        {
            switch (s0)
            {

                case -1:
                    count_down4 = 1f;
                    goto case 2;
                case 2:
                    if (((count_down4) > (0f)))
                    {

                        count_down4 = ((count_down4) - (dt));
                        s0 = 2;
                        return;
                    }
                    else
                    {

                        goto case 0;
                    }
                case 0:
                    IsInEvent = UnityNpc.IsInterestedInEvent();
                    s0 = -1;
                    return;
                default:
                    return;
            }
        }


        int s1 = -1;

        public void Rule1(float dt, World world)
        {
            switch (s1)
            {

                case -1:
                    if (!(IsInEvent))
                    {

                        s1 = -1;
                        return;
                    }
                    else
                    {

                        goto case 3;
                    }
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
                    if (((count_down5) > (0f)))
                    {

                        count_down5 = ((count_down5) - (dt));
                        s1 = 1;
                        return;
                    }
                    else
                    {

                        s1 = -1;
                        return;
                    }
                default:
                    return;
            }
        }


        int s2 = -1;

        public void Rule2(float dt, World world)
        {
            switch (s2)
            {

                case -1:
                    if (!(IsInEvent))
                    {

                        s2 = -1;
                        return;
                    }
                    else
                    {

                        goto case 2;
                    }
                case 2:
                    if (!(!(IsInEvent)))
                    {

                        s2 = 2;
                        return;
                    }
                    else
                    {

                        goto case 1;
                    }
                case 1:
                    UnityNpc.UpdateCurrentNodesCollection();
                    actionIds = UnityNpc.ActionsToPerform;
                    PositionAvailable = false;
                    s2 = -1;
                    return;
                default:
                    return;
            }
        }


        int s3 = -1;

        public void Rule3(float dt, World world)
        {
            switch (s3)
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
                    if (((count_down6) > (0f)))
                    {

                        count_down6 = ((count_down6) - (dt));
                        s3 = 2;
                        return;
                    }
                    else
                    {

                        goto case 0;
                    }
                case 0:
                    if (!(!(IsInEvent)))
                    {

                        s3 = 0;
                        return;
                    }
                    else
                    {

                        s3 = -1;
                        return;
                    }
                default:
                    return;
            }
        }


        int s4 = -1;

        public void Rule4(float dt, World world)
        {
            switch (s4)
            {

                case -1:
                    if (!(!(PositionAvailable)))
                    {

                        s4 = -1;
                        return;
                    }
                    else
                    {

                        goto case 7;
                    }
                case 7:
                    if (!(((actionIds.Count) > (0))))
                    {

                        s4 = 7;
                        return;
                    }
                    else
                    {

                        goto case 6;
                    }
                case 6:
                    ___actionToExecute40 = (actionIds)[0];
                    if (UnityNpc.IsPositionAvailable(___actionToExecute40))
                    {

                        goto case 0;
                    }
                    else
                    {

                        goto case 1;
                    }
                case 0:
                    UnityNpc.ClaimPosition(___actionToExecute40);
                    actionIds = actionIds;
                    PositionAvailable = true;
                    s4 = -1;
                    return;
                case 1:
                    actionIds = (

                        (actionIds).Select(__ContextSymbol10 => new {___id40 = __ContextSymbol10})
                            .Where(__ContextSymbol11 => !(((__ContextSymbol11.___id40) == (___actionToExecute40))))
                            .Select(__ContextSymbol12 => __ContextSymbol12.___id40)
                            .ToList<System.Int32>()).ToList<System.Int32>();
                    PositionAvailable = false;
                    s4 = -1;
                    return;
                default:
                    return;
            }
        }


        int s5 = -1;

        public void Rule5(float dt, World world)
        {
            switch (s5)
            {

                case -1:
                    if (!(PositionAvailable))
                    {

                        s5 = -1;
                        return;
                    }
                    else
                    {

                        goto case 12;
                    }
                case 12:
                    if (!(((actionIds.Count) > (0))))
                    {

                        s5 = 12;
                        return;
                    }
                    else
                    {

                        goto case 11;
                    }
                case 11:
                    ___actionToExecute51 = (actionIds)[0];
                    ___destination50 = UnityNpc.GetClaimedPosition(___actionToExecute51);
                    UnityNpc.MoveTo(___actionToExecute51);
                    ___distanceToDestination50 = UnityEngine.Vector3.Distance(Position, ___destination50);
                    if (((2.2f) > (___distanceToDestination50)))
                    {

                        goto case 1;
                    }
                    else
                    {

                        s5 = -1;
                        return;
                    }
                case 1:
                    UnityNpc.StopMovement();
                    UnityNpc.UpdateAccumulatedValues(___actionToExecute51);
                    count_down7 = UnityNpc.PlayAnimation(___actionToExecute51);
                    goto case 5;
                case 5:
                    if (((count_down7) > (0f)))
                    {

                        count_down7 = ((count_down7) - (dt));
                        s5 = 5;
                        return;
                    }
                    else
                    {

                        goto case 3;
                    }
                case 3:
                    UnityNpc.RemoveClaimToPosition(___actionToExecute51);
                    actionIds = (

                        (actionIds).Select(__ContextSymbol13 => new {___id51 = __ContextSymbol13})
                            .Where(__ContextSymbol14 => !(((__ContextSymbol14.___id51) == (___actionToExecute51))))
                            .Select(__ContextSymbol15 => __ContextSymbol15.___id51)
                            .ToList<System.Int32>()).ToList<System.Int32>();
                    PositionAvailable = false;
                    s5 = -1;
                    return;
                default:
                    return;
            }
        }






    }
}
