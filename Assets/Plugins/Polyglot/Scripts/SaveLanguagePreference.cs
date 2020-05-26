#if UNITY_5
using JetBrains.Annotations;
#endif
using UnityEngine;

namespace Polyglot
{
    public class SaveLanguagePreference : MonoBehaviour, ILocalize
    {
        [SerializeField]
        private string preferenceKey = "Polyglot.SelectedLanguage";

#if UNITY_5
        [UsedImplicitly]
#endif
        public void Start()
        {

            Localization.Instance.SelectedLanguage = (Language) PlayerPrefs.GetInt(preferenceKey);
            Localization.Instance.AddOnLocalizeEvent(this);
        }

        public void OnLocalize()
        {
            SystemLanguage sysLang = Application.systemLanguage;
            if (sysLang == SystemLanguage.English) {
                Localization.Instance.SelectedLanguage = Language.English;
            }else if (sysLang == SystemLanguage.Spanish) {
                Localization.Instance.SelectedLanguage = Language.Spanish;
            }else if (sysLang == SystemLanguage.French) {
                Localization.Instance.SelectedLanguage = Language.French;
            }else if (sysLang == SystemLanguage.Japanese) {
                Localization.Instance.SelectedLanguage = Language.Japanese;
            }else if (sysLang == SystemLanguage.ChineseSimplified) {
                Localization.Instance.SelectedLanguage = Language.Simplified_Chinese;
            }else if (sysLang == SystemLanguage.Arabic) {
                Localization.Instance.SelectedLanguage = Language.Arabic;
            }

            PlayerPrefs.SetInt(preferenceKey, (int) Localization.Instance.SelectedLanguage);
        }
    }
}
