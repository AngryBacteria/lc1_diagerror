import { watch, ref } from 'vue'
import { defineStore } from 'pinia'
import { StorageSerializers, useLocalStorage, useSessionStorage } from '@vueuse/core'
import type { MessageLanguages } from '@/plugins/i18n'
import { useI18n } from 'vue-i18n'
import { useCurrentUser } from 'vuefire'
import type { SnackbarConfig } from '@/components/global/GlobalSnackbar.vue'
import type { Questionnaire } from '@/data/interfaces'

export const useUserStore = defineStore('user', () => {
  //other fields
  const i18n = useI18n()
  const refUser = useCurrentUser()
  const isLoggedIn = ref(false)
  const apiEndpoint = 'https://localhost:7139'

  //store fields
  const language = useLocalStorage<MessageLanguages>('language', 'de')
  const snackbarConfig = useLocalStorage<SnackbarConfig>('snackbarConfig', {
    visible: false,
    message: 'TestMessage',
    color: 'primary',
    timeout: '2000',
    location: 'top'
  })

  const answers = useSessionStorage<any[]>('answers', [])
  const questionnaire = useSessionStorage<Questionnaire>('key', null, {
    serializer: StorageSerializers.object
  })
  const inviteCode = useSessionStorage<string>('inviteCode', null)

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

  function abortQuestionnaire() {
    answers.value = []
    questionnaire.value = null
    inviteCode.value = ''
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
    clearAnswers,
    abortQuestionnaire,
    questionnaire,
    apiEndpoint,
    inviteCode
  }
})
