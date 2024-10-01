using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class SetupController : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            // Set the random seed globally when the game starts
            Random.InitState(Constants.RANDOM_SEED);
        }
    }
}
