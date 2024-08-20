using System;
using System.Collections.Generic;
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
            await UniTask.Yield();
            await UniTask.WaitUntil(() => Mathf.Approximately(Time.timeScale, 1));
            
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

        private List<string> ParseRichTextSegments(string text)
        {
            var segments = new List<string>();
            var index = 0;
            
            while (index < text.Length)
            {
                if (text[index] == '<')
                {
                    var endTagIndex = text.IndexOf('>', index);
                    
                    if (endTagIndex != -1)
                    {
                        var tag = text.Substring(index, endTagIndex - index + 1);
                        segments.Add(tag);
                        index = endTagIndex + 1;
                    }
                    else
                    {
                        segments.Add(text.Substring(index));
                        break;
                    }
                }
                else
                {
                    var nextTagIndex = text.IndexOf('<', index);
                    
                    if (nextTagIndex != -1)
                    {
                        var content = text.Substring(index, nextTagIndex - index);
                        segments.Add(content);
                        index = nextTagIndex;
                    }
                    else
                    {
                        segments.Add(text.Substring(index));
                        break;
                    }
                }
            }
            return segments;
        }

        private async UniTask TypeText(string text)
        {
            _isTyping = true;
            mainText.text = "";

            var segments = ParseRichTextSegments(text);

            foreach (var segment in segments)
            {
                if (segment.StartsWith("<"))
                {
                    mainText.text += segment;
                }
                else
                {
                    foreach (var letter in segment.ToCharArray())
                    {
                        mainText.text += letter;
                        await UniTask.Delay(TimeSpan.FromSeconds(typingSpeed));
                    }
                }
            }

            _isTyping = false;
        }
        
        private async UniTask ChangeImage(Sprite newImage)
        {
            if (mainImage.sprite != null)
            {
                await UniTask.WhenAll(
                    FadeOut(mainImage),
                    FadeOut(mainText));
            }

            mainImage.sprite = newImage;
            mainText.text = "";

            await UniTask.WhenAll(
                FadeIn(mainImage),
                FadeIn(mainText));
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
