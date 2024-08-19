using System;
using _Scripts.Infra;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Ui
{
    public class CutsceneManager : MonoBehaviour
    {
        [Serializable]
        public class CutsceneStep
        {
            public Sprite image;
            [TextArea] public string text;
        }

        [SerializeField] private CutsceneStep[] cutsceneSteps;
        [SerializeField] private Image mainImage;
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private TextMeshProUGUI skipText;
        [SerializeField] private float typingSpeed = 0.05f;
        [SerializeField] private float fadeDuration = 1.0f;
        [SerializeField] private float betweenStepsDelay = 0.75f;
        [SerializeField] private float skipTextDisappearDelay = 2.0f;

        private bool _isTyping = false;
        private bool _isFading = false;

        private void Awake()
        {
            ResetScene();
        }

        private void Start()
        {
            skipText.canvasRenderer.SetAlpha(0.0f);
            mainImage.canvasRenderer.SetAlpha(0.0f);
            PlayCutscene().Forget();
        }

        private void Update()
        {
            if (!_isTyping && Input.anyKeyDown)
            {
                SkipTextVisibility();
            }

            if (Input.GetKeyDown(KeyCode.Space) && skipText.canvasRenderer.GetAlpha() > 0)
            {
                ServiceLocator.GetService<SceneLoader>().LoadNextScene().Forget();
            }
        }

        private void ResetScene()
        {
            mainText.text = "";
        }

        private async UniTask PlayCutscene()
        {
            foreach (var step in cutsceneSteps)
            {

                await ChangeImage(step.image);
                await TypeText(step.text);
                await UniTask.Delay(TimeSpan.FromSeconds(betweenStepsDelay));  
            }

            await UniTask.Delay(TimeSpan.FromSeconds(betweenStepsDelay));
            LoadSceneAfterCutscene();
        }

        private void LoadSceneAfterCutscene()
        {
            ServiceLocator.GetService<SceneLoader>().LoadNextScene().Forget();
        }

        private async UniTask TypeText(string text)
        {
            _isTyping = true;
            mainText.text = "";

            foreach (var letter in text.ToCharArray())
            {
                mainText.text += letter;
                await UniTask.Delay(TimeSpan.FromSeconds(typingSpeed));
            }
            
            _isTyping = false;
        }

        private async UniTask ChangeImage(Sprite newImage)
        {
            _isFading = true;

            if (mainImage.sprite != null)
            {
                await FadeOut(mainImage);
                await FadeOut(mainText);
            }

            mainImage.sprite = newImage;
            mainText.text = "";

            await FadeIn(mainImage);
            await FadeIn(mainText);

            _isFading = false;
        }

        private async UniTask FadeOut(Graphic graphic)
        {
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                graphic.canvasRenderer.SetAlpha(1 - t / fadeDuration);
                await UniTask.Yield();
            }
            graphic.canvasRenderer.SetAlpha(0);
        }

        private async UniTask FadeIn(Graphic graphic)
        {
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                graphic.canvasRenderer.SetAlpha(t / fadeDuration);
                await UniTask.Yield();
            }
            graphic.canvasRenderer.SetAlpha(1);
        }

        private void SkipTextVisibility()
        {
            skipText.CrossFadeAlpha(1.0f, 0.5f, false);
            HideSkipText().Forget();
        }

        private async UniTaskVoid HideSkipText()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(skipTextDisappearDelay));
            skipText.CrossFadeAlpha(0.0f, 0.5f, false);
        }
    }
}
