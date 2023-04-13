<template>
  <v-form @submit.prevent v-model="validForm" ref="myForm">
    <p>{{ store.questionnaire.descriptionForCustomer }}</p>
    <template v-for="(question) in store.questionnaire.questions" :key="question.questionId">
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

    <v-btn :disabled="!validForm" type="submit" @click="store.submitQuestionnaire()" class="ma-2">
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
//TODO fix issue that questionnaire button gets disabled on language change
const myForm = ref<any>(null)
onMounted(() => {
  if (store.answers.length != 0 && myForm.value) myForm.value.validate()
})
</script>

<style>
.v-form {
  max-width: 1000px;
  margin-right: auto;
  margin-left: auto;
}
</style>
