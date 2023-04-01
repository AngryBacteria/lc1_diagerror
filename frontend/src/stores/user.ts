import { defineStore } from 'pinia'
import { useLocalStorage } from '@vueuse/core'
import type { MessageLanguages } from '@/plugins/i18n'
import { useI18n } from 'vue-i18n'
import { useCurrentUser } from 'vuefire'

export const useUserStore = defineStore('user', () => {
  //other fields
  const i18n = useI18n()
  const refUser = useCurrentUser()

  //store fields
  const language = useLocalStorage<MessageLanguages>('language', 'de')

  //init code
  i18n.locale.value = language.value

  function changeLanguage(systemLanguage: MessageLanguages) {
    language.value = systemLanguage
    i18n.locale.value = systemLanguage
  }

  return { language, changeLanguage, refUser }
})
