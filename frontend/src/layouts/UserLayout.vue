<script setup lang="ts">
import { useUserStore } from '@/stores/user'
import { APP_LANGUAGES } from '@/plugins/i18n'
import { useTypedI18n } from '@/composables/useTypedI18n'
import { useDisplay } from 'vuetify'

const store = useUserStore()
const { t } = useTypedI18n()
const { smAndUp } = useDisplay()
</script>

<template>
  <v-layout class="mt-2 mr-6 ml-6 mb-4">
    <v-app-bar color="background">
      <template v-slot:prepend>
        <router-link to="/">
          <img v-if="smAndUp" src="../assets/UKNLogo.svg" alt="Logo" style="height: 80px" />
          <img v-else src="../assets/InselLogo.svg" alt="Logo" style="height: 60px" />
        </router-link>
      </template>

      <template v-slot:append>
        <v-menu>
          <template v-slot:activator="{ props }">
            <v-btn prepend-icon="mdi-translate" v-if="smAndUp" variant="elevated" color="primary" v-bind="props">
              {{ t('questionnaire.navigation.language') }} ({{ store.language }})
            </v-btn>
            <v-btn prepend-icon="mdi-translate" v-else variant="elevated" color="primary" v-bind="props">{{
              store.language
            }}</v-btn>
          </template>
          <v-list>
            <v-list-item
              @click="store.changeLanguage(item)"
              v-for="(item, index) in APP_LANGUAGES"
              :key="index"
              :value="index"
            >
              <v-list-item-title>{{ item.toUpperCase() }}</v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>
      </template>
    </v-app-bar>

    <v-main>
      <RouterView></RouterView>
    </v-main>
  </v-layout>
</template>

<style></style>
