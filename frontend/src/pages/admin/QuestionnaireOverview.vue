<script setup lang="ts">
import { ref } from 'vue'
import download from 'downloadjs'
import { useDebouncedRef } from '@/composables/useDebouncedRef'
import { watch } from 'vue'
import { useFetch } from '@vueuse/core'
import type { PaginatedQuestionnaire } from '@/data/interfaces'

const searchValue = useDebouncedRef('')
const maxPage = ref(60)
const page = ref(1)

//TODO make this computed
const lightUrl = `https://localhost:7184/questionnaire/light/filter?page=${page.value}&identifier=${searchValue.value}`
const { execute, error, statusCode, isFetching, data } = useFetch(lightUrl, { immediate: false })
  .get()
  .json<PaginatedQuestionnaire>()

function test() {
  download(JSON.stringify('', null, 2), 'test.json', 'application/json')
}

watch(searchValue, () => {
  fetchQuestionnaire()
})

//TODO error handling
async function fetchQuestionnaire() {
  await execute()
  console.log(statusCode.value)
  if (data.value && !error.value) {
    maxPage.value = data.value?.pageCount
  }
}

fetchQuestionnaire()
</script>

<template>
  <main style="margin: 1rem">
    <h1>Admin</h1>
    <v-text-field
      :loading="isFetching"
      v-model="searchValue"
      label="Fragebogen mit Kürzel suchen"
      color="primary"
      prepend-inner-icon="mdi-cloud-search"
    >
    </v-text-field>

    <h3>Verfügbare Fragebogen</h3>
    <v-card
      v-for="item in data?.data"
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
          <v-btn prepend-icon="mdi-download" @click="test()">Download</v-btn>
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
