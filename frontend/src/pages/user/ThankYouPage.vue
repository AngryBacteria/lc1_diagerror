<template>
  <v-card
    class="d-flex flex-column align-center"
    max-width="60%"
    elevation="3"
    style="margin-right: auto; margin-left: auto"
  >
    <v-icon icon="mdi-charity" size="100px" color="primary"> </v-icon>

    <h1 class="text-center">{{ t('questionnaire.thanks.title') }}</h1>
    <h4 class="text-center">{{ t('questionnaire.thanks.subtitle') }}</h4>
    <div class="buttons">
      <v-btn
        prepend-icon="mdi-message-processing"
        color="primary"
        style="margin-top: 1rem"
        @click="openInNewTab('https://www.insel.ch/de/kontakt')"
      >
        {{ t('questionnaire.thanks.contactInsel') }}
      </v-btn>
      <v-btn
        prepend-icon="mdi-note-plus"
        color="secondary"
        style="margin-top: 1rem"
        @click="router.push('/questionnaire/invite')"
      >
        {{ t('questionnaire.thanks.newQuestionnaire') }}
      </v-btn>
    </div>
  </v-card>
</template>

<script setup lang="ts">
import { useTypedI18n } from '@/composables/useTypedI18n'
import { useRouter } from 'vue-router'

const router = useRouter()
const { t } = useTypedI18n()

function openInNewTab(url: string) {
  window.open(url)
}
</script>

<style scoped>
.v-card {
  padding: 1rem;
  margin: 1rem;
}

.buttons {
  display: flex;
  flex-direction: row;
  gap: 0.5rem;
  flex-wrap: wrap;
  justify-content: center;
}

@media only screen and (max-width: 600px) {
  .v-card {
    min-width: 95%;
  }
}
</style>
