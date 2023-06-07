<script setup lang="ts">
import { useUserStore } from '@/stores/user'
import { useRouter } from 'vue-router'
import { useFirebaseAuth } from 'vuefire'
import { signOut } from 'firebase/auth'
import { ref } from 'vue'
import { useDisplay } from 'vuetify/lib/framework.mjs'

const store = useUserStore()
const router = useRouter()

/**
 * Function to logout the firebase user. Redirects the user to '/admin/login' after logout
 */
async function logout() {
  try {
    const auth = useFirebaseAuth()
    if (auth) {
      await signOut(auth)
    }
    await router.push('/admin/login')
  } catch (error) {
    store.resetSnackbarConfig
    store.snackbarConfig.message = "Etwas ist schiefgelaufen, bitte versuchen sie es erneut"
    store.snackbarConfig.color = 'error'
    store.snackbarConfig.visible = true
    console.log(`Error while attempting to Log-Out: ${error}`)
  }
}

/**
 * Interface for the left-drawer navigation items
 */
interface navItem {
  title: string
  subtitle: string
  to: string
  icon: string
}

/**
 * Left-drawer navigation items
 */
const items: navItem[] = [
  {
    title: 'Übersicht Fragebogen',
    subtitle: 'Fragebogen anzeigen',
    to: '/admin/overview',
    icon: 'mdi-home'
  },
  {
    title: 'Fragebogen hochladen',
    subtitle: 'Einen neuen Fragebogen hochladen',
    to: '/admin/questionnaire/upload',
    icon: 'mdi-upload'
  }
]

const { mdAndUp } = useDisplay()
const drawer = mdAndUp ? ref(true) : ref(false)
</script>

<template>
  <v-layout class="mt-2 mr-6 ml-6 mb-4">
    <v-app-bar elevation="1" color="background" density="compact">
      <v-app-bar-nav-icon variant="text" @click.stop="drawer = !drawer"></v-app-bar-nav-icon>

      <v-toolbar-title>Admin-Tool</v-toolbar-title>

      <v-spacer></v-spacer>

      <v-menu>
        <template v-slot:activator="{ props }">
          <v-btn v-if="store.isLoggedIn" variant="elevated" color="primary" v-bind="props">
            {{ store.refUser?.email }}
          </v-btn>
          <v-btn v-if="!store.isLoggedIn" variant="elevated" color="error" v-bind="props">
            Nicht angemeldet
          </v-btn>
        </template>
        <v-list>
          <v-list-item v-if="!store.isLoggedIn">
            <v-btn @click="router.push('/admin/login')" block prepend-icon="mdi-login">
              Login
            </v-btn>
          </v-list-item>

          <v-list-item v-if="store.isLoggedIn">
            <v-btn @click="logout()" block prepend-icon="mdi-logout"> Logout </v-btn>
          </v-list-item>
        </v-list>
      </v-menu>
    </v-app-bar>

    <v-navigation-drawer
      v-model="drawer"
      location="left"
      permanent
      elevation="3"
      v-if="store.isLoggedIn"
    >
      <v-list nav>
        <v-list-item
          v-for="(item, i) in items"
          :key="i"
          :value="item"
          :title="item.title"
          :subtitle="item.subtitle"
          :to="item.to"
          :prepend-icon="item.icon"
          lines="two"
          active-color="primary"
        >
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-main>
      <RouterView></RouterView>
    </v-main>
  </v-layout>
</template>

<style></style>
