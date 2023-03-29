import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from '@/App.vue'
import router from '@/router'
import { vuetify } from '@/plugins/vuetify'
import { i18n } from '@/plugins/i18n'
import { VueFire, VueFireAuth } from 'vuefire'
import { firebaseApp } from '@/plugins/firebase'

const app = createApp(App)

//Pinia (State management)
app.use(createPinia())

//Vue-Router (SPA)
app.use(router)

//Vuetify
app.use(vuetify)

//i18n (locale manager)
app.use(i18n)

//Firebase and VueFire
app.use(VueFire, {
  firebaseApp,
  modules: [VueFireAuth()]
})

app.mount('#app')
