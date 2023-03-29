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
  <main>
    <h1>Hello</h1>
    <v-btn> I am a Button </v-btn>
    <v-icon icon="mdi-home" />
    <p>{{ t('common.test') }}</p>
  </main>
</template>
