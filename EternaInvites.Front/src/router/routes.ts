import { useAuthStore } from 'src/stores/authStore'
import type { RouteRecordRaw } from 'vue-router'

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: () => import('src/views/Main/_Main.vue'),
    children: [],
  },
  {
    path: '/:id',
    component: () => import('src/views/Main/Invitacion.vue'),
    children: [],
  },
  {
    path: '/login',
    beforeEnter: () => {
      const authStore = useAuthStore()
      if (authStore.token) return { path: '/dashboard' }
      else return true
    },
    component: () => import('src/views/Login/_Login.vue'),
    children: [],
  },
  {
    path: '/dashboard',
    redirect: '/dashboard/inicio',
    beforeEnter: () => {
      const authStore = useAuthStore()

      if (!authStore.token) return { path: '/login' }
      else return true
    },
    component: () => import('src/views/Dashboard/_Dashboard.vue'),
    children: [
      {
        path: 'inicio',
        name: 'Inicio',
        component: () => import('src/views/Dashboard/Inicio.vue'),
      },
      {
        path: 'clientes',
        name: 'Clientes',
        component: () => import('src/views/Dashboard/Clientes.vue'),
      },
      {
        path: 'plantillas',
        name: 'Plantillas',
        component: () => import('src/views/Dashboard/Plantillas.vue'),
      },
    ],
  },
  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('src/views/NotFound.vue'),
  },
]

export default routes
