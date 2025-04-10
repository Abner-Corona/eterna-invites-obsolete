<template>
  <q-layout view="hHh lpR fFf">
    <q-header elevated class="bg-primary text-white">
      <q-toolbar>
        <q-btn flat dense round icon="menu" aria-label="Menu" @click="toggleLeftDrawer" />

        <q-toolbar-title>Eterna Invites Dashboard</q-toolbar-title>
        <q-btn flat round dense icon="account_circle">
          <q-menu>
            <q-list style="min-width: 150px">
              <q-item clickable v-close-popup @click="logout">
                <q-item-section avatar>
                  <q-icon name="logout" />
                </q-item-section>
                <q-item-section>Logout</q-item-section>
              </q-item>
            </q-list>
          </q-menu>
        </q-btn>
      </q-toolbar>
    </q-header>

    <q-drawer v-model="leftDrawerOpen" show-if-above bordered class="bg-grey-1">
      <q-list>
        <q-item-label header>Navigation</q-item-label>

        <q-item
          v-for="item in navigationItems"
          :key="item.label"
          clickable
          v-ripple
          :to="item.route">
          <q-item-section avatar>
            <q-icon :name="item.icon" />
          </q-item-section>
          <q-item-section>{{ item.label }}</q-item-section>
        </q-item>
      </q-list>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
  import { useAuthStore } from 'src/stores/authStore'
  import { ref } from 'vue'
  import { useRouter } from 'vue-router'

  const leftDrawerOpen = ref(false)
  const router = useRouter()
  const authStore = useAuthStore()

  // Navigation items as JSON
  const navigationItems = [
    {
      label: 'Inicio',
      icon: 'home',
      route: '/dashboard/inicio',
    },
    {
      label: 'Clientes',
      icon: 'people',
      route: '/dashboard/clientes',
    },
    {
      label: 'Plantillas',
      icon: 'description',
      route: '/dashboard/plantillas',
    },
  ]

  function toggleLeftDrawer() {
    leftDrawerOpen.value = !leftDrawerOpen.value
  }

  function logout() {
    authStore.token = '' // Clear the token and auth state
    router.push('/login') // Redirect to login page
  }
</script>

<style scoped>
  .q-page-container {
    background-color: #f5f5f5;
    min-height: 100vh;
  }
</style>
