<template>
  <p>Questionnaire code loaded: {{ store.inviteCode }}</p>
  <v-form v-model="validForm">
    <div v-for="(question, index) in store.questionnaire.questions" :key="question.questionId">
      <FreeTextQuestion :index="index" :question="question"></FreeTextQuestion>
    </div>

    <v-btn :disabled="!validForm" type="submit" class="mt-2">Submit</v-btn>
    <v-btn @click="store.clearAnswers()" class="mt-2">Clear</v-btn>
    <v-btn @click="store.abortQuestionnaire()" class="mt-2">Abort</v-btn>
  </v-form>
</template>

<script setup lang="ts">
import FreeTextQuestion from '@/components/questionnaire/FreeTextQuestion.vue'
import { useUserStore } from '@/stores/user'
import { ref } from 'vue'

const store = useUserStore()
const validForm = ref(false)
</script>

<style>
.v-form {
  max-width: 1000px;
  margin-right: auto;
  margin-left: auto;
}
</style>
