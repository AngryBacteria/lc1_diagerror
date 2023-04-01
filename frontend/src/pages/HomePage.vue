<script setup lang="ts">
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
  <h1>Hello</h1>
  <v-btn append-icon="mdi-home"> I am a Button </v-btn>
  <p>{{ t('common.test') }}</p>
  <router-link to="/admin">admin</router-link>
  <br />
  <router-link to="/questionnaire">questionnaire</router-link>
</template>
