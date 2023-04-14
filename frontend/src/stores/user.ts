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
  const answers = useSessionStorage<any[]>('answers', [])
  const questionnaire = useSessionStorage<Questionnaire>('key', null, {
    serializer: StorageSerializers.object
  })
  const inviteCode = useSessionStorage<string>('inviteCode', null)
  const snackbarConfig = useLocalStorage<SnackbarConfig>('snackbarConfig', {
    visible: false,
    message: 'TestMessage',
    color: 'primary',
    timeout: '2000',
    location: 'top'
  })

  //init language
  i18n.locale.value = language.value

  /**
   * Changes the language of the website
   * @param systemLanguage One of the 3 valid Languages (EN, DE, FR)
   */
  function changeLanguage(systemLanguage: MessageLanguages) {
    language.value = systemLanguage
    i18n.locale.value = systemLanguage
  }

  /**
   * Deletes all the answers in the store
   */
  function clearAnswers() {
    answers.value = []
  }

  /**
   * Aborts the current questionnaire. Deletes all answers, the inviteCode and the questionnaire
   */
  function abortQuestionnaire() {
    answers.value = []
    questionnaire.value = null
    inviteCode.value = ''
  }

  //TODO submitting to backend and returning if it was successful
  /**
   * Function that builds valid answer objects for submitting them to the database
   * @param validForm Boolean to indicate if the form was valid or not
   */
  function submitQuestionnaire(): { success: boolean; error: any } {
    try {
      if (!questionnaire.value || !answers.value || !inviteCode)
        return { success: false, error: null }

      const formattedAnswers = questionnaire.value.questions.map((question) => {
        return {
          questionId: question.questionId,
          text: answers.value.at(question.index),
          date: new Date().toISOString().split('T')[0],
          invitationId: inviteCode.value
        }
      })
      console.log(formattedAnswers)
      return { success: true, error: null }
    } catch (error) {
      console.log(error)
      return { success: false, error: error }
    }
  }

  /**
   * Resets the snackBar config with default values
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
