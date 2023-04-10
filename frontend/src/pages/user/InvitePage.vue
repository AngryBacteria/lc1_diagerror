<template>
  <div v-if="store.questionnaire && !error">
    <QuestionnairePage></QuestionnairePage>
  </div>
  <div v-if="error">
    <v-alert
      icon="mdi-alert-circle-outline"
      :text="'Error in Request: ' + error"
      color="error"
      rounded
      elevation="3"
      closable
      close-icon="mdi-close"
    ></v-alert>
  </div>
  <div v-if="alreadyUsed">Code has already been used</div>

  <v-card
    v-if="!store.questionnaire || error"
    style="margin: 2rem auto 0 auto; padding: 1rem"
    max-width="50%"
    elevation="1"
  >
    <div class="text-h6 font-weight-regular mb-2">
      {{ t('questionnaire.navigation.provideCode') }}
    </div>
    <v-form @submit.prevent v-model="validForm">
      <v-text-field
        v-model="store.inviteCode"
        autocomplete="off"
        :label="t('questionnaire.navigation.invitationCode')"
        rounded
        color="primary"
        :rules="[
          () => !!store.inviteCode || t('questionnaire.validation.fieldRequired'),
          () => store.inviteCode.length === 8 || t('questionnaire.validation.sixDigits')
        ]"
      ></v-text-field>
      <v-btn
        type="submit"
        @click="loadQuestionnaire()"
        :loading="isFetching"
        :disabled="!validForm"
      >
        {{ t('questionnaire.navigation.startQuestionnaire') }}</v-btn
      >
    </v-form>
  </v-card>
</template>

<script setup lang="ts">
import { useTypedI18n } from '@/composables/useTypedI18n'
import { useFetch } from '@vueuse/core'
import { ref } from 'vue'
import { useRoute } from 'vue-router'
import { useUserStore } from '@/stores/user'
import type { Questionnaire } from '@/data/interfaces'
import QuestionnairePage from '@/pages/user/QuestionnairePage.vue'

const { t } = useTypedI18n()
const route = useRoute()
const store = useUserStore()

const validForm = ref(false)
const alreadyUsed = ref(false)

const { execute, error, isFetching, data } = useFetch(
  'https://localhost:7184/questionnaire/light/hui',
  {
    immediate: false
  }
)
  .get()
  .json<Questionnaire>()

const pathParam = route.params.invitationCode
initPage()

async function initPage() {
  if (pathParam != null) {
    if (typeof pathParam == 'string') {
      store.inviteCode = pathParam
    }
    if (Array.isArray(pathParam) && pathParam.length != 0) {
      store.inviteCode = pathParam[0]
    }
    await loadQuestionnaire()
  }
}

async function loadQuestionnaire() {
  if (store.questionnaire) return

  await execute()

  if (data.value) {
    store.questionnaire = data.value
  }
}
</script>

<style>
.mdi-close {
  color: black !important;
}
</style>
