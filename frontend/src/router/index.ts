import { createRouter, createWebHistory } from 'vue-router'
import { getCurrentUser } from 'vuefire'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/questionnaire/home'
    },
    {
      path: '/questionnaire',
      name: 'layout-questionnaire',
      component: () => import('@/layouts/UserLayout.vue'),
      children: [
        {
          path: '/questionnaire/home',

          component: () => import('@/pages/user/QuestionnaireHomePage.vue')
        },
        {
          path: '/questionnaire/invite/:invitationCode',

          component: () => import('@/pages/user/InvitePage.vue')
        },
        {
          path: '/questionnaire/invite/',

          component: () => import('@/pages/user/InvitePage.vue')
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
          path: '/admin/login',
          name: 'admin-login',
          component: () => import('@/components/admin/LoginForm.vue')
        }
      ]
    }
  ]
})

//Router gard for protected routes
router.beforeEach(async (to) => {
  if (to.meta.requiresAuth) {
    const currentUser = await getCurrentUser()

    if (!currentUser) {
      //TODO error message
      return {
        path: '/admin/login',
        query: {
          redirect: to.fullPath
        }
      }
    }
    const token = await currentUser?.getIdTokenResult()

    if (!token || token.claims.admin != true) {
      //TODO error message
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
