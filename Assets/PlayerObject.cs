using UnityEngine;

namespace Assets
{
    public class PlayerObject : MonoBehaviour
    {
        private const string GameObjectModelName = "FPSController";

        public static PlayerObject Instantiate()
        {
            var position = new Vector3(12, 1.4f, 3);

            var playerObject = (Instantiate(
                    Resources.Load(GameObjectModelName),
                    position,
                    Quaternion.identity)
                as GameObject).GetComponent<PlayerObject>();

            return playerObject;
        }

        public bool IsLookingAt(GameObject anotherGameObject)
        {
            RaycastHit hit;
            var charCtrl = GetComponent<CharacterController>();
            var origin = transform.position + charCtrl.center;

            if (Physics.SphereCast(origin, 1, transform.forward, out hit, 5))
            {
                return anotherGameObject.gameObject == hit.collider.gameObject;
            }
            return false;
        }

        public void TriggerEvent(EventObject eventObject)
        {
            if (!EventController.ActiveEvents.ContainsKey(eventObject.Id))
                EventController.ActiveEvents.Add(eventObject.Id, eventObject);
            else
                EventController.ActiveEvents.Remove(eventObject.Id);

        }
    }
}
                                                                                                                                                                                                                                   