<template>
  <main>
    <img src="@/assets/undraw_404_v1.svg" alt="404" />
    <h2>{{ t('common.404') }}</h2>
    <v-btn size="large" color="primary" @click="router.push('/')">
      {{ t('common.back') }}
    </v-btn>
  </main>
</template>

<script setup lang="ts">
import { useTypedI18n } from '@/composables/useTypedI18n'
import { useRouter } from 'vue-router'

const { t } = useTypedI18n()
const router = useRouter()
</script>

<style>
main {
  margin-top: 2rem;

  gap: 2rem;
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  flex-wrap: wrap;
}

img {
  width: 50%;
}

h2 {
  text-align: center;
  width: 75%;
}
</style>
