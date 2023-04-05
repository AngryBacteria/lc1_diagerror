<template>
  <div v-if="readyFlag">
    <p>Questionnaire code loaded: {{ inviteCode }}</p>
  </div>
  <div v-else>
    <p>code could not be loaded</p>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()

const readyFlag = ref(false)
const errorFlag = ref(false)
const inviteCode = ref()

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

async function loadQuestionnaire(inviteCode: string) {
  await new Promise((resolve) => setTimeout(resolve, 1000))
}
</script>

<style></style>
