import type { RouteRecordRaw } from 'vue-router'

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: () => import('src/views/Main/_Main.vue'),
    children: [],
  },
  {
    path: '/:id',
    component: () => import('src/views/Invite/_Invite.vue'),
    children: [],
  },
  {
    path: '/login',
    component: () => import('src/views/Login/_Login.vue'),
    children: [],
  },
  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('src/views/NotFound.vue'),
  },
]

export default routes
