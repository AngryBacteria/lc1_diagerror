<script setup lang="ts">
import { ref } from 'vue'
import download from 'downloadjs'
import { useDebouncedRef } from '@/composables/useDebouncedRef'
import { watch } from 'vue'
import { useFetch } from '@vueuse/core'
import type { PaginatedQuestionnaire } from '@/data/interfaces'
import { computed } from 'vue'
import { useUserStore } from '@/stores/user'

const searchValue = useDebouncedRef('')
const maxPage = ref(60)
const page = ref(1)
const store = useUserStore()
const globalIsFetching = ref(false)

/**
 * Computed reactive url for light questionnaires
 */
const lightUrl = computed(() => {
  return `${store.apiEndpoint}/questionnaire/light/filter?page=${page.value}&identifier=${searchValue.value}`
})

/**
 * Fetch new light questionnaires on url change
 */
watch(lightUrl, () => {
  fetchLightQuestionnaires()
})

/**
 * Function to fetch light questionnaires
 */
const {
  execute: executeLight,
  error: lightError,
  statusCode: lightStatusCode,
  isFetching: lightIsFetching,
  data: lightData
} = useFetch(lightUrl, { immediate: false, refetch: false }).get().json<PaginatedQuestionnaire>()

//TODO error handling
/**
 * Fetches new questionnaires and displays occured errors
 */
async function fetchLightQuestionnaires() {
  await executeLight()
  if (lightData.value && !lightError.value) {
    maxPage.value = lightData.value?.pageCount
  }

  if (lightError.value) {
    console.log(lightStatusCode.value, lightError.value)
  }
}

//TODO add language
//TODO problem with utf chars on downloaded files (öüä)
/**
 * Downloads questionnaire
 * @param identifier Identifier of questionnaire
 */
async function downloadQuestionnaires(identifier: string) {
  try {
    globalIsFetching.value = true
    const url = `https://localhost:7184/questionnaire/light/filter?identifier=${identifier}`
    const { error, statusCode, data } = await useFetch(url, {
      immediate: true,
      refetch: false
    })
      .get()
      .json<PaginatedQuestionnaire>()

    if (data.value && !error.value) {
      download(JSON.stringify(data.value, null, 2), 'test.json', 'application/json')
    } else {
      store.resetSnackbarConfig
      store.snackbarConfig.message = `Etwas ist schiefgelaufen beim download [CODE: ${statusCode.value}]`
      store.snackbarConfig.color = 'error'
      store.snackbarConfig.visible = true
    }
  } catch (error: any) {
    store.resetSnackbarConfig
    store.snackbarConfig.message = `Etwas ist schiefgelaufen beim download`
    store.snackbarConfig.color = 'error'
    store.snackbarConfig.visible = true
  } finally {
    globalIsFetching.value = false
  }
}

fetchLightQuestionnaires()
</script>

<template>
  <main style="margin: 1rem">
    <h1>Admin</h1>
    <v-text-field
      :loading="lightIsFetching || globalIsFetching"
      v-model="searchValue"
      label="Fragebogen mit Kürzel suchen"
      color="primary"
      prepend-inner-icon="mdi-cloud-search"
    >
    </v-text-field>

    <h3>Verfügbare Fragebogen</h3>
    <v-card
      v-for="item in lightData?.data"
      :key="`${item.identifier} - ${item.language}`"
      style="padding: 1rem; margin-bottom: 1rem"
    >
      <v-row>
        <v-col>
          <h4>{{ item.title }}</h4>
          <v-chip rounded class="mr-2" color="primary" label>
            <v-icon start icon="mdi-identifier"></v-icon>
            {{ item.identifier }}
          </v-chip>
          <v-chip rounded class="mr-2" color="primary" label>
            <v-icon start icon="mdi-translate"></v-icon>
            {{ item.language }}
          </v-chip>
        </v-col>
        <v-spacer></v-spacer>
        <v-col cols="auto" style="margin-top: auto; margin-bottom: auto">
          <v-btn color="secondary" prepend-icon="mdi-note-edit" style="margin-right: 1rem"
            >Editieren</v-btn
          >
          <v-btn
            prepend-icon="mdi-download"
            style="margin-right: 1rem"
            @click="downloadQuestionnaires(item.identifier)"
            >Download</v-btn
          >
          <v-btn color="secondary" prepend-icon="mdi-folder-plus" style="margin-right: 1rem"
            >Datei erstellen</v-btn
          >
        </v-col>
      </v-row>
    </v-card>

    <v-pagination
      style="margin-top;: auto"
      v-model="page"
      :length="maxPage"
      :total-visible="6"
      rounded
      active-color="primary"
      variant="flat"
      show-first-last-page
    ></v-pagination>
  </main>
</template>
