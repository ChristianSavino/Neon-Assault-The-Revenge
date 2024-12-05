using UnityEngine;

namespace Keru.Scripts.Game.Entities.Player
{
    public class PlayerThirdPersonAnimations : MonoBehaviour
    {
        [SerializeField] private Animator _body;
        [SerializeField] private Animator _shadow;

        public void SetConfig()
        {

        }

        public void SetParameter(string parameterName, float value)
        {
            _body.SetFloat(parameterName, value);
            _shadow.SetFloat(parameterName, value);
        }

        public void SetParameter(string parameterName, bool value)
        {
            _body.SetBool(parameterName, value);
            _shadow.SetBool(parameterName, value);
        }

        public void PlayAnimation(string animationName, int layer = 0, int timeAt = 0)
        {
            _body.Play(animationName, layer, timeAt);
            _shadow.Play(animationName, layer, timeAt);
        }
    }
}