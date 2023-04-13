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
  const apiEndpoint = 'https://localhost:7184'

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
  /**
   * Function to change the language of the website
   * @param systemLanguage One of the 3 valid Languages (EN, DE, FR)
   */
  function changeLanguage(systemLanguage: MessageLanguages) {
    language.value = systemLanguage
    i18n.locale.value = systemLanguage
  }

  /**
   * Deleted the answers in the store
   */
  function clearAnswers() {
    answers.value = []
  }

  /**
   * Function to abort the current Questionnaire. Deleted all answers, the inviteCode and the Questionnaire
   */
  function abortQuestionnaire() {
    answers.value = []
    questionnaire.value = null
    inviteCode.value = ''
  }

  //TODO submitting to backend
  /**
   * Function that builds valid Answer Objects for submitting them to the Database
   * @param validForm Boolean to indicate if the form was valid or not
   */
  function submitQuestionnaire() {
    if (!questionnaire.value || !answers.value || !inviteCode) return

    const formattedAnswers = questionnaire.value.questions.map((question) => {
      return {
        questionId: question.questionId,
        text: answers.value.at(question.index),
        date: new Date().toISOString().split('T')[0],
        invitationId: inviteCode.value
      }
    })
    console.log(formattedAnswers)
  }

  /**
   * Function to reset the snackBar Config with default values
   */
  function resetSnackbarConfig() {
    snackbarConfig.value = {
      visible: false,
      message: 'TestMessage',
      color: 'primary',
      timeout: '3000',
      location: 'top'
    }
  }

  /**
   * Watcher to keep the isLoggedIn ref up to date
   */
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
    submitQuestionnaire,
    questionnaire,
    apiEndpoint,
    inviteCode
  }
})
