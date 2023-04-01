import { watch, ref } from 'vue'
import { defineStore } from 'pinia'
import { useLocalStorage } from '@vueuse/core'
import type { MessageLanguages } from '@/plugins/i18n'
import { useI18n } from 'vue-i18n'
import { useCurrentUser } from 'vuefire'

export const useUserStore = defineStore('user', () => {
  //other fields
  const i18n = useI18n()
  const refUser = useCurrentUser()
  const isLoggedIn = ref(false)

  //store fields
  const language = useLocalStorage<MessageLanguages>('language', 'de')

  //init code
  i18n.locale.value = language.value

  //functions
  function changeLanguage(systemLanguage: MessageLanguages) {
    language.value = systemLanguage
    i18n.locale.value = systemLanguage
  }

  watch(
    refUser,
    (newRefUser) => {
      isLoggedIn.value = !!newRefUser
    },
    { deep: true }
  )

  return { language, changeLanguage, refUser, isLoggedIn }
})
