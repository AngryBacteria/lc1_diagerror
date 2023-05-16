import { createVuetify, type ThemeDefinition } from 'vuetify'
import { md3 } from 'vuetify/blueprints'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
import 'vuetify/styles'
import '@mdi/font/css/materialdesignicons.css'
import { aliases, mdi } from 'vuetify/iconsets/mdi'

/**
 * Creates the theme of this app
 */
const inselTheme: ThemeDefinition = {
  dark: false,
  colors: {
    background: '#EEEEEE',
    surface: '#FFFFFF',
    primary: '#009870',
    secondary: '#677078',
    error: '#B00020',
    info: '#d1e2bc',
    success: '#4CAF50',
    warning: '#FB8C00'
  }
}

/**
 * Creates the vuetify instance and exports it
 */
export const vuetify = createVuetify({
  components,
  directives,
  blueprint: md3,
  theme: {
    defaultTheme: 'inselTheme',
    themes: {
      inselTheme: inselTheme
    }
  },
  icons: {
    defaultSet: 'mdi',
    aliases,
    sets: {
      mdi
    }
  },
  defaults: {
    global: {
      ripple: false
    }
  }
})
