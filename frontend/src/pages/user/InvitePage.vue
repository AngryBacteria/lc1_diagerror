<template>
  <div v-if="readyFlag">
    <p>Questionnaire code loaded: {{ inviteCode }}</p>
  </div>
  <v-card v-else style="margin: 2rem auto 0 auto; padding: 1rem" max-width="95%" elevation="1">
    <div class="text-h6 font-weight-regular mb-2">Bitte geben sie ihren Einladungscode ein</div>
    <v-form @submit.prevent v-model="validForm">
      <v-text-field
        v-model="inviteCode"
        label="Einladungscode"
        rounded
        color="primary"
        :rules="[() => !!inviteCode || t('questionnaire.validation.fieldRequired')]"
      ></v-text-field>
      <v-btn type="submit" @click="loadQuestionnaire()" :loading="loadingFlag">
        Umfrage starten</v-btn
      >
    </v-form>
  </v-card>
</template>

<script setup lang="ts">
import { useTypedI18n } from '@/composables/useTypedI18n'
import { ref } from 'vue'
import { useRoute } from 'vue-router'

const { t } = useTypedI18n()
const route = useRoute()

//TODO i18n

const readyFlag = ref(false)
const loadingFlag = ref(false)
const inviteCode = ref()
const validForm = ref(false)

const pathParam = route.params.invitationCode

if (pathParam != null) {
  if (typeof pathParam == 'string') {
    inviteCode.value = pathParam
    readyFlag.value = true
  }
  if (Array.isArray(pathParam) && pathParam.length != 0) {
    inviteCode.value = pathParam[0]
    readyFlag.value = true
  }
  console.log(inviteCode.value)
}

async function loadQuestionnaire() {
  if (validForm.value) {
    readyFlag.value = false
    loadingFlag.value = true
    await new Promise((resolve) => setTimeout(resolve, 1000))
    loadingFlag.value = false
    readyFlag.value = true
  }
}
</script>

<style></style>
