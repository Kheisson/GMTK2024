using _Scripts.Audio;
using _Scripts.Infra;
using _Scripts.Ui.Popups;
using Animations;
using Cysharp.Threading.Tasks;

namespace Controls.StateMachine
{
    public class DeathState : State
    {
        public DeathState(PlayerResources playerResources, FiniteStateMachine stateMachine) : base(playerResources, stateMachine)
        {
        }

        public override void Enter()
        {
            _playerResources.Animator.SetTrigger(AnimationConstants.DEATH_KEY);
            ServiceLocator.GetService<AudioManager>().PlaySfx(AudioConstants.LOSE);
            ServiceLocator.GetService<PopupManager>().ShowPopupAsync(EPopup.GameOver, metadata: _playerResources.PlayerType.ToString()).Forget();
        }
    }
}