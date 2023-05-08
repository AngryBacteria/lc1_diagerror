<template>
  <main>
    <h1>Ein valides Questionnaire JSON Array kann hier hochgeladen werden</h1>
    <v-btn @click="uploadQuestionnaire()" :loading="fetching">Hochladen</v-btn>
    <v-textarea
      style="max-height: 80vh"
      :loading="fetching"
      color="primary"
      :rows="20"
      v-model="questionnaire"
    >
    </v-textarea>
  </main>
</template>

<script setup lang="ts">
import { useUserStore } from '@/stores/user'
import { ref } from 'vue'

const questionnaire = ref('')
const store = useUserStore()
const fetching = ref(false)

async function uploadQuestionnaire() {
  try {
    fetching.value = true
    if (questionnaire.value.length == 0) {
      store.resetSnackbarConfig
      store.snackbarConfig.message = `Das Inputfeld darf nicht leer sein`
      store.snackbarConfig.color = 'error'
      store.snackbarConfig.visible = true
      return
    }

    const url = `${store.apiEndpoint}/questionnaire/light`
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(JSON.parse(questionnaire.value))
    })

    if (response.ok) {
      store.resetSnackbarConfig
      store.snackbarConfig.message = `Fragebogen erfolgreich hochgeladen`
      store.snackbarConfig.color = 'primary'
      store.snackbarConfig.visible = true
      questionnaire.value = ''
    } else {
      store.resetSnackbarConfig
      store.snackbarConfig.message = `Fragebogen konnte nicht hochgeladen werden [CODE: ${response.status}]`
      store.snackbarConfig.color = 'error'
      store.snackbarConfig.visible = true
    }
  } catch (e: any) {
    store.resetSnackbarConfig
    store.snackbarConfig.message = `Fragebogen konnte nicht hochgeladen werden [String ist kein JSON]`
    store.snackbarConfig.color = 'error'
    store.snackbarConfig.visible = true
  } finally {
    fetching.value = false
  }
}
</script>

<style>
main {
  padding: 1rem;
}

.v-btn {
  margin-top: 1rem;
  margin-bottom: 1rem;
}
</style>
