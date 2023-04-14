<template>
  <v-form @submit.prevent v-model="validForm" ref="mainForm">
    <v-card>
      <h1>{{ store.questionnaire.title }}</h1>
      <p>{{ store.questionnaire.descriptionForCustomer }}</p>
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
    </template>

    <v-btn type="submit" @click="submitForm()" class="ma-2">
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
import SingleChoiceQuestion from '@/components/questionnaire/SingleChoiceQuestion.vue'
import { useTypedI18n } from '@/composables/useTypedI18n'
import { useUserStore } from '@/stores/user'
import { onMounted } from 'vue'
import { ref } from 'vue'

const store = useUserStore()
const { t } = useTypedI18n()
const validForm = ref(true)

//Validation on page reload
const mainForm = ref<any>(null)
onMounted(() => {
  if (store.answers.length != 0 && mainForm.value) mainForm.value.validate()
})

function submitForm() {
  mainForm.value.validate()
  if (validForm.value) {
    store.submitQuestionnaire()
  }
}
</script>

<style>
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
