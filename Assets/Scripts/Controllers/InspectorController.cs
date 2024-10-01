using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class InspectorController : MonoBehaviour
    {
#pragma warning disable IDE0051 // Remove unused private members
        private void Update()
#pragma warning restore IDE0051 // Remove unused private members
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log(hit.collider.name);
            }
        }
    }
}
