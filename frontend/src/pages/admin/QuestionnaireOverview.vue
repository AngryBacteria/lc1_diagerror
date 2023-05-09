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
const dialog = ref(false)

/**
 * Computed reactive url for light questionnaires
 */
const lightUrl = computed(() => {
  const value = (searchValue.value as string).toUpperCase()
  return `${store.apiEndpoint}/questionnaire/light/filter?page=${page.value}&identifier=${value}`
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

//TODO test various new implemented methods

/**
 * Fetches new questionnaires and displays occured errors
 */
async function fetchLightQuestionnaires() {
  await executeLight()
  if (lightData.value && !lightError.value) {
    maxPage.value = lightData.value?.pageCount
  }

  if (lightError.value) {
    store.resetSnackbarConfig
    store.snackbarConfig.message = `Etwas ist schiefgelaufen beim anzeigen [CODE: ${lightStatusCode.value}]`
    store.snackbarConfig.color = 'error'
    store.snackbarConfig.visible = true
  }
}

/**
 * Downloads questionnaire
 * @param identifier Identifier of questionnaire
 * @param language Language of questionnaire
 */
async function downloadQuestionnaires(url: string, fileAppend: string) {
  try {
    globalIsFetching.value = true
    const { error, statusCode, data } = await useFetch(url, {
      immediate: true,
      refetch: false
    })
      .get()
      .json<PaginatedQuestionnaire>()

    if (data.value && !error.value) {
      download(
        JSON.stringify(data.value.data, null, 2),
        `${data.value.data[0].identifier}[${data.value.data[0].language}]-[${fileAppend}].json`,
        'text/plain'
      )
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

async function createFileOnServer(identifier: string, language: string) {
  try {
    globalIsFetching.value = true
    const url = `${store.apiEndpoint}/questionnaire/file/create?identifier=${identifier}&language=${language}`
    const { error, statusCode, data } = await useFetch(url, {
      immediate: true,
      refetch: false
    })
      .get()
      .json<any>()

    if (data.value && !error.value) {
      store.resetSnackbarConfig
      store.snackbarConfig.message = `Datei wurde erstellt: ${data.value.fileName}`
      store.snackbarConfig.timeout = '10000'
      store.snackbarConfig.color = 'primary'
      store.snackbarConfig.visible = true
    } else {
      store.resetSnackbarConfig
      store.snackbarConfig.message = `Etwas ist schiefgelaufen beim Erstellen der Datei [CODE: ${statusCode.value}]`
      store.snackbarConfig.color = 'error'
      store.snackbarConfig.visible = true
    }
  } catch (e: any) {
    store.resetSnackbarConfig
    store.snackbarConfig.message = `Etwas ist schiefgelaufen beim Erstellen der Datei`
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

    <section class="top">
      <h3>Verfügbare Fragebogen</h3>
      <v-dialog v-model="dialog" width="auto">
        <template v-slot:activator="{ props }">
          <v-btn density="compact" icon="mdi-help" color="error" v-bind="props"></v-btn>
        </template>

        <v-card>
          <v-card-text>
            Ein fragebogen kann auf mehrere weisen heruntergeladen werden. Folgend werden diese
            erklärt;
            <ul style="padding: 1rem">
              <li>
                <b>Fragebogen:</b> Es wird ein JSON heruntergeladen mit nur dem Fragebogen ohne
                Antworten
              </li>
              <li>
                <b>Antworten (letzte 30 Tage):</b> Es wird ein JSON heruntergeladen dem Fragebogen
                und den Antworten der letzten 30 Tage. Es wird hier limitiert, da über den Browser
                grössere Anfragen nicht möglich sind
              </li>
              <li>
                <b>Datei mit allen Antworten erstellen:</b> Auf dem Server wird eine JSON Datei
                erstellt mit allen Antworten zu diesem Fragebogen. Der Path zur Datei wird
                dargestellt nachdem sie erstellt wurde
              </li>
            </ul>
          </v-card-text>
          <v-card-actions>
            <v-btn
              color="primary"
              variant="flat"
              @click="dialog = false"
              style="margin-right: auto; margin-left: auto"
              >Schliessen</v-btn
            >
          </v-card-actions>
        </v-card>
      </v-dialog>
    </section>
    <v-card
      v-for="item in lightData?.data"
      :key="`${item.identifier} - ${item.language}`"
      style="padding: 1rem; margin-bottom: 1rem; margin-top: 1rem"
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
          <v-btn
            prepend-icon="mdi-download"
            style="margin-right: 1rem"
            color="primary"
            @click="
              downloadQuestionnaires(
                `${store.apiEndpoint}/questionnaire/light/filter?identifier=${item.identifier}&language=${item.language}`,
                'Fragebogen'
              )
            "
          >
            Fragebogen
          </v-btn>
          <v-btn
            prepend-icon="mdi-download"
            style="margin-right: 1rem"
            color="secondary"
            @click="
              downloadQuestionnaires(
                `${store.apiEndpoint}/questionnaire/complete/filter?identifier=${item.identifier}&language=${item.language}&lastDays=30`,
                'Antworten(30-Tage)'
              )
            "
          >
            Antworten (letzte 30 Tagen)
          </v-btn>
          <v-btn
            prepend-icon="mdi-folder-plus"
            style="margin-right: 1rem"
            color="primary"
            @click="createFileOnServer(item.identifier, item.language)"
          >
            Datei mit Allen Antworten erstellen
          </v-btn>
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

<style>
.top {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
}
</style>
