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
  <p>todo user-layout</p>
  <v-layout>
    <v-app-bar color="background">
      <template v-slot:prepend>
        <img v-if="smAndUp" src="../assets/UKNLogo.svg" alt="Logo" style="height: 80px" />
        <img v-else src="../assets/InselLogo.svg" alt="Logo" style="height: 60px" />
      </template>

      <template v-slot:append>
        <v-menu>
          <template v-slot:activator="{ props }">
            <v-btn v-if="smAndUp" variant="tonal" color="primary" v-bind="props">
              {{ t('navigation.language') }} ({{ store.language }})
            </v-btn>
            <v-btn v-else variant="tonal" color="primary" v-bind="props">{{
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
      <Suspense>
        <RouterView></RouterView>
      </Suspense>
    </v-main>
  </v-layout>
</template>

<style>
.v-main {
  margin-top: 0;
  margin-left: 5%;
  margin-right: 5%;
}
</style>
