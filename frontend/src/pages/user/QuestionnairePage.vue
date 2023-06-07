<template>
  <v-form @submit.prevent v-model="validForm" ref="mainForm">
    <v-card style="margin-top: 1rem">
      <h1>{{ store.questionnaire.title }}</h1>
      <p>{{ store.questionnaire.descriptionForCustomer }}</p>
      <v-divider class="my-2" :thickness="3"></v-divider>
      <p style="color: #1fa481">
        * {{ t('questionnaire.navigation.questionOptionalExplanation') }}
      </p>
    </v-card>
    <template v-for="question in store.questionnaire.questions" :key="question.questionId">
      <FreeTextQuestion
        v-if="question.questiontype === 'FreeText'"
        :index="question.index"
        :question="question"
      />
      <LikertQuestionVue
        v-if="question.questiontype === 'Likert'"
        :index="question.index"
        :question="question"
      />
      <SingleChoiceQuestion
        v-if="question.questiontype === 'SingleChoice'"
        :index="question.index"
        :question="question"
      />
      <MultipleChoiceQuestion
        v-if="question.questiontype === 'MultipleChoice'"
        :index="question.index"
        :question="question"
      />
    </template>

    <v-btn type="submit" @click="submitForm()" class="ma-2" :loading="submitLoading">
      {{ t('questionnaire.navigation.submitQuestionnaire') }}
    </v-btn>
    <v-btn @click="store.abortQuestionnaire()" class="ma-2" color="error">
      {{ t('questionnaire.navigation.abortQuestionnaire') }}
    </v-btn>
  </v-form>
</template>

<script setup lang="ts">
import FreeTextQuestion from '@/components/questionnaire/FreeTextQuestion.vue'
import LikertQuestionVue from '@/components/questionnaire/LikertQuestion.vue'
import MultipleChoiceQuestion from '@/components/questionnaire/MultipleChoiceQuestion.vue'
import SingleChoiceQuestion from '@/components/questionnaire/SingleChoiceQuestion.vue'
import { useTypedI18n } from '@/composables/useTypedI18n'
import router from '@/router'
import { useUserStore } from '@/stores/user'
import { onMounted } from 'vue'
import { ref } from 'vue'

const store = useUserStore()
const { t } = useTypedI18n()

const validForm = ref(true)
const mainForm = ref<any>(null)
const submitLoading = ref(false)

/**
 * Form validation on page reload if answers are already existing
 */
onMounted(() => {
  initialValidation()
})

/**
 * Submits the questionnaire if the form is valid
 */
async function submitForm() {
  submitLoading.value = true
  await mainForm.value.validate()
  if (validForm.value) {
    const { success, error } = await store.submitQuestionnaire()

    if (success) {
      tidyAfterSubmit()
    } else {
      displaySubmitError(error)
    }
  } else {
    displayInvalidFormError()
  }
  submitLoading.value = false
}

/**
 * Checks if any answers are present. If there are, it validates the form
 */
async function initialValidation() {
  const isEmpty = store.answers.every((item) => {
    if (Array.isArray(item) && item.length == 0) {
      return true
    }
    if (!Array.isArray(item) && !item) {
      return true
    } else {
      return false
    }
  })

  if (!isEmpty) {
    mainForm.value.validate()
  }
}

/**
 * Scrolls smoothly to the first validation error if any exist
 */
function displayInvalidFormError() {
  const errorMessage = document.querySelector('.v-messages__message:first-of-type')?.parentElement
    ?.parentElement?.parentElement?.parentElement

  if (errorMessage) {
    errorMessage.scrollIntoView({
      behavior: 'smooth',
      block: 'center',
      inline: 'center'
    })
  }
}

/**
 * Displays the errors that can emerge while trying to submit
 * @param error Thrown error while submitting
 */
function displaySubmitError(error: any) {
  console.log(error)
  store.resetSnackbarConfig
  store.snackbarConfig.message = t('questionnaire.navigation.submitError')
  store.snackbarConfig.color = 'error'
  store.snackbarConfig.visible = true
}

/**
 * Prepares the browser for a new questionnaire after one has been submitted
 */
function tidyAfterSubmit() {
  router.push('/questionnaire/thanks')
  store.abortQuestionnaire()
}
</script>

<style scoped>
.v-form {
  max-width: 1000px;
  margin-right: auto;
  margin-left: auto;
}

.v-card {
  padding: 1rem;
  margin: 1rem;
}
</style>
