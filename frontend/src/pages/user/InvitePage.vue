<template>
  <div v-if="data && !error">
    <p>Questionnaire code loaded: {{ inviteCode }}</p>
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
    v-if="!data || error"
    style="margin: 2rem auto 0 auto; padding: 1rem"
    max-width="50%"
    elevation="1"
  >
    <div class="text-h6 font-weight-regular mb-2">
      {{ t('questionnaire.navigation.provideCode') }}
    </div>
    <v-form @submit.prevent v-model="validForm">
      <v-text-field
        v-model="inviteCode"
        autocomplete="off"
        :label="t('questionnaire.navigation.invitationCode')"
        rounded
        color="primary"
        :rules="[
          () => !!inviteCode || t('questionnaire.validation.fieldRequired'),
          () => inviteCode.length === 8 || t('questionnaire.validation.sixDigits')
        ]"
      ></v-text-field>
      <v-btn type="submit" @click="execute()" :loading="isFetching" :disabled="!validForm">
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

const { t } = useTypedI18n()
const route = useRoute()
const store = useUserStore()

const inviteCode = ref()
const validForm = ref(false)
const alreadyUsed = ref(false)

const { execute, error, isFetching, data } = useFetch('https://httpbin.org/get', {
  immediate: false
})

const pathParam = route.params.invitationCode
initPage()

async function initPage() {
  if (pathParam != null) {
    if (typeof pathParam == 'string') {
      inviteCode.value = pathParam
    }
    if (Array.isArray(pathParam) && pathParam.length != 0) {
      inviteCode.value = pathParam[0]
    }
    await execute()
  }
}
</script>

<style>
.mdi-close {
  color: black !important;
}
</style>
