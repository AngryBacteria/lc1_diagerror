import { useUserStore } from '@/stores/user'
import { createRouter, createWebHistory } from 'vue-router'
import { getCurrentUser } from 'vuefire'
import { i18n } from '@/plugins/i18n'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/questionnaire/invite'
    },
    {
      path: '/questionnaire',
      name: 'layout-questionnaire',
      redirect: '/questionnaire/invite',
      component: () => import('@/layouts/UserLayout.vue'),
      children: [
        {
          path: 'invite/:invitationCode',
          component: () => import('@/pages/user/InvitePage.vue')
        },
        {
          path: 'invite',
          component: () => import('@/pages/user/InvitePage.vue')
        },
        {
          path: 'thanks',
          name: 'thanks-page',
          component: () => import('@/pages/user/ThankYouPage.vue')
        }
      ]
    },
    {
      path: '/admin',
      name: 'home-admin',
      component: () => import('@/layouts/AdminLayout.vue'),
      children: [
        {
          path: '',
          name: 'admin-home',
          meta: { requiresAuth: true },
          component: () => import('@/pages/admin/AdminHomePage.vue')
        },
        {
          path: 'login',
          name: 'admin-login',
          component: () => import('@/components/admin/LoginForm.vue')
        }
      ]
    }
  ]
})

router.beforeEach(async (to) => {
  /**
   * Router gard for protected routes. Checks if user is logged in and has the admin claim.
   * Displays a snackbar if user is redirected
   */
  if (to.meta.requiresAuth) {
    const currentUser = await getCurrentUser()
    const store = useUserStore()

    //Redirection
    if (!currentUser) {
      store.resetSnackbarConfig
      store.snackbarConfig.message = i18n.global.t('admin.loginComponent.alerts.userRequired')
      store.snackbarConfig.color = 'error'
      store.snackbarConfig.visible = true

      return {
        path: '/admin/login',
        query: {
          redirect: to.fullPath
        }
      }
    }
    const token = await currentUser?.getIdTokenResult()

    //No admin right --> redirection
    if (!token || token.claims.admin != true) {
      store.resetSnackbarConfig
      store.snackbarConfig.message = i18n.global.t('admin.loginComponent.alerts.userRequired')
      store.snackbarConfig.color = 'error'
      store.snackbarConfig.visible = true

      return {
        path: '/admin/login',
        query: {
          redirect: to.fullPath
        }
      }
    }
  }
})

export default router
