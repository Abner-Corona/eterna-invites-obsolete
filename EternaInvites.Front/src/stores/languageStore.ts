import { defineStore, acceptHMRUpdate } from 'pinia'

// Adjust the import path as needed
interface LanguageInfo {
  code: string
  name: string
  nativeName: string
  flag: string
  region?: string
}

interface LanguageStoreState {
  currentLanguage: string | null
  availableLanguages: LanguageInfo[]
}

export const useLanguageStore = defineStore('language', {
  state: (): LanguageStoreState => ({
    currentLanguage: navigator.language.split('-')[0] || 'es', // Will be detected from device/browser
    availableLanguages: [
      {
        code: 'es',
        name: 'Spanish',
        nativeName: 'Español',
        flag: '🇪🇸',
        region: 'ES',
      },
      {
        code: 'en',
        name: 'English',
        nativeName: 'English',
        flag: '🇺🇸',
        region: 'US',
      },
    ],
  }),

  getters: {
    getCurrentLanguage: (state) => {
      if (!state.currentLanguage) {
        // Automatically detect language from browser/device settings
        const browserLanguage = navigator.language.split('-')[0] // Get language code (e.g., 'en', 'es')
        const detectedLanguage = state.availableLanguages.find((lang) => lang.code === browserLanguage || lang.code === browserLanguage)
        if (detectedLanguage) {
          state.currentLanguage = detectedLanguage.code
        }
      }
      return state.currentLanguage! // Default to English if not set
    },
  },

  actions: {
    changeLanguage(languageCode: string) {
      const language = this.availableLanguages.find((lang) => lang.code === languageCode)
      if (language) {
        this.currentLanguage = language.code
      }
    },
  },

  // Enable automatic persistence - Pinia handles localStorage automatically
  // No manual localStorage operations needed anywhere in the application
  persist: true,
})

if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useLanguageStore, import.meta.hot))
}
