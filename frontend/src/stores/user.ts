import { watch, ref } from 'vue'
import { defineStore } from 'pinia'
import { useLocalStorage, useSessionStorage } from '@vueuse/core'
import type { MessageLanguages } from '@/plugins/i18n'
import { useI18n } from 'vue-i18n'
import { useCurrentUser } from 'vuefire'
import type { SnackbarConfig } from '@/components/global/GlobalSnackbar.vue'

export const useUserStore = defineStore('user', () => {
  //other fields
  const i18n = useI18n()
  const refUser = useCurrentUser()
  const isLoggedIn = ref(false)

  //store fields
  const language = useLocalStorage<MessageLanguages>('language', 'de')

  const answers = useSessionStorage<Array<String>>('answers', [])

  const snackbarConfig = useLocalStorage<SnackbarConfig>('snackbarConfig', {
    visible: false,
    message: 'TestMessage',
    color: 'primary',
    timeout: '2000',
    location: 'top'
  })

  //init code
  i18n.locale.value = language.value

  //functions
  function changeLanguage(systemLanguage: MessageLanguages) {
    language.value = systemLanguage
    i18n.locale.value = systemLanguage
  }

  function clearAnswers() {
    answers.value = []
  }

  function resetSnackbarConfig() {
    snackbarConfig.value = {
      visible: false,
      message: 'TestMessage',
      color: 'primary',
      timeout: '3000',
      location: 'top'
    }
  }

  watch(
    refUser,
    (newRefUser) => {
      isLoggedIn.value = !!newRefUser
    },
    { deep: true }
  )

  return {
    language,
    changeLanguage,
    refUser,
    isLoggedIn,
    snackbarConfig,
    resetSnackbarConfig,
    answers,
    clearAnswers
  }
})
