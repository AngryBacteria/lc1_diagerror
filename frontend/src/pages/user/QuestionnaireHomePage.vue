<script setup lang="ts">
import FreeTextQuestion from '@/components/questionnaire/FreeTextQuestion.vue';
import { useTypedI18n } from '@/composables/useTypedI18n'
import { useFirebaseAuth } from 'vuefire'

//const data = await fetch('https://localhost:7139/weatherforecast')
//const json = data.json()
//console.log(json)
const { t } = useTypedI18n()
const auth = useFirebaseAuth()
console.log(auth)
</script>

<template>
  <h1>Questionnaire Home Page</h1>
  <v-btn append-icon="mdi-home"> I am a Button </v-btn>
  <p>{{ t('questionnaire.common.test') }}</p>
  <router-link to="/admin">admin</router-link>
  <br />
  <router-link to="/questionnaire">questionnaire</router-link>

  <v-form>
    <free-text-question></free-text-question>
    <free-text-question></free-text-question>
    <v-btn type="submit" class="mt-2">Submit</v-btn>
  </v-form>
</template>

<style>
.v-form {
  max-width: 1000px;
  margin-right: auto;
  margin-left: auto;
}
</style>
