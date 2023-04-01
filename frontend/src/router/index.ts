import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/questionnaire'
    },
    {
      path: '/questionnaire',
      name: 'home-questionnaire',
      component: () => import('@/layouts/UserLayout.vue'),
      children: [
        {
          path: '',
          name: 'questionnaire-home',
          component: () => import('@/pages/HomePage.vue')
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
          component: () => import('@/pages/HomePage.vue')
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

export default router
