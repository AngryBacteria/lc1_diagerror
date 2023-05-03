import { watch, ref } from 'vue'
import { defineStore } from 'pinia'
import { StorageSerializers, useFetch, useLocalStorage, useSessionStorage } from '@vueuse/core'
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

  /**
   * Function that builds valid answer objects for submitting them to the database
   */
  async function submitQuestionnaire(): Promise<{ success: boolean; error: any }> {
    try {
      if (!questionnaire.value || !answers.value || !inviteCode)
        return { success: false, error: 'Form Not Valid' }

      const formattedAnswers = prepareAnswerObjects()
      //TODO remove
      console.log(formattedAnswers)
      const url = `${apiEndpoint}/answer`
      const { error, statusCode } = await useFetch(url, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formattedAnswers)
      })

      if (!error.value && statusCode.value == 200) {
        return { success: true, error: null }
      } else {
        return { success: false, error: error }
      }
    } catch (error) {
      console.error(error)
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
   * Prepares the answer Objects for submitting
   */
  function prepareAnswerObjects(): any[] {
    const currentDate = new Date().toISOString().split('T')[0]
    const formattedAnswers = questionnaire.value.questions.map((question) => {
      if (question.questiontype != 'MultipleChoice') {
        return {
          questionId: question.questionId,
          text: answers.value.at(question.index) ? answers.value.at(question.index) : '',
          date: currentDate,
          invitationId: inviteCode.value
        }
      } else {
        const answerArray = answers.value.at(question.index)
        answerArray.sort((a: number, b: number) => a - b)
        let answerOutput = ''
        if (answerArray.length > 0) {
          answerOutput = answerArray.join('|')
        }
        return {
          questionId: question.questionId,
          text: answerOutput,
          date: currentDate,
          invitationId: inviteCode.value
        }
      }
    })
    return formattedAnswers
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
