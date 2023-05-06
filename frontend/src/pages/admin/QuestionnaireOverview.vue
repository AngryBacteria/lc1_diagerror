<script setup lang="ts">
import { ref } from 'vue'
import { computed } from 'vue'
import download from 'downloadjs'
import { watch } from 'vue'

const searchValue = ref('')
const maxPage = ref(60)
const page = ref()

const file = ref()

const filteredList = computed(() => {
  return items.filter((item) => {
    if (item.identifier.toLocaleLowerCase().startsWith(searchValue.value)) {
      return item
    }
  })
})

watch(file, () => {
  console.log(file.value)
})

function test() {
  download(JSON.stringify(items, null, 2), 'test.json', 'application/json')
}

const items = [
  {
    title: 'Questionnaire for Quality-Control at the university hospital in Bern',
    identifier: 'QC',
    language: 'EN'
  },
  {
    title: 'Testfragebogen für die Entwicklung',
    identifier: 'TF',
    language: 'DE'
  },
  {
    title: 'Test questionnaire for development',
    identifier: 'TF',
    language: 'EN'
  },
  {
    title: 'Questionnaire de test pour le développement',
    identifier: 'TF',
    language: 'FR'
  }
]
</script>

<template>
  <main style="margin: 1rem">
    <h1>Admin</h1>
    <v-text-field
      v-model="searchValue"
      label="Fragebogen mit Kürzel suchen"
      color="primary"
      prepend-inner-icon="mdi-cloud-search"
    >
    </v-text-field>

    <h3>Verfügbare Fragebogen</h3>
    <v-card
      v-for="item in filteredList"
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
