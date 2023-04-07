<script setup lang="ts">
import FreeTextQuestion from '@/components/questionnaire/FreeTextQuestion.vue'
import LikertQuestion from '@/components/questionnaire/LikertQuestion.vue'
import NumberQuestion from '@/components/questionnaire/NumberQuestion.vue'
import SingleChoiceQuestionVue from '@/components/questionnaire/SingleChoiceQuestion.vue'
import { useUserStore } from '@/stores/user'
import { useFirebaseAuth } from 'vuefire'

//const data = await fetch('https://localhost:7139/weatherforecast')
//const json = data.json()
//console.log(json)
const auth = useFirebaseAuth()
const store = useUserStore()
console.log(auth)
</script>

<template>
  <router-link to="/admin">admin</router-link>
  <br />
  <router-link to="/questionnaire/test">questionnaire</router-link>

  <v-form>
    <free-text-question :index="0"></free-text-question>
    <likert-question :index="1"></likert-question>
    <SingleChoiceQuestionVue :index="2"></SingleChoiceQuestionVue>
    <NumberQuestion :index="3"></NumberQuestion>

    <v-btn type="submit" class="mt-2">Submit</v-btn>
    <v-btn @click="store.clearAnswers()" class="mt-2">Clear</v-btn>
  </v-form>
</template>

<style>
.v-form {
  max-width: 1000px;
  margin-right: auto;
  margin-left: auto;
}
</style>
