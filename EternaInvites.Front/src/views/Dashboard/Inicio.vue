<template>
  <div class="q-pa-md">
    <h2 class="text-h4 text-weight-medium q-mb-md">Welcome to Eterna Invites</h2>
    <p class="text-subtitle1 q-mb-lg">Select an option to get started</p>

    <div class="row q-col-gutter-md">
      <div v-for="card in dashboardCards" :key="card.title" class="col-12 col-sm-6 col-md-4">
        <q-card class="dashboard-card" @click="navigateTo(card.route)">
          <q-card-section :class="card.color + ' text-white'">
            <div class="text-h6">
              <q-icon :name="card.icon" size="md" class="q-mr-sm" />
              {{ card.title }}
            </div>
          </q-card-section>
          <q-card-section>
            <p>{{ card.description }}</p>
          </q-card-section>
          <q-card-actions>
            <q-btn flat :color="card.buttonColor" :label="card.buttonLabel" />
          </q-card-actions>
        </q-card>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { useRouter } from 'vue-router'

  const router = useRouter()

  // Navigation items for sidebar
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

  // Dashboard cards data
  const dashboardCards = [
    {
      title: 'Clientes',
      icon: 'people',
      description: 'Administrar clientes y sus invitaciones',
      buttonLabel: 'Ver cleintes',
      buttonColor: 'primary',
      color: 'bg-primary',
      route: '/dashboard/clientes',
    },
    {
      title: 'Plantillas',
      icon: 'event',
      description: 'Administrar plantillas de invitaciones',
      buttonLabel: 'Ver Plantillas',
      buttonColor: 'secondary',
      color: 'bg-secondary',
      route: '/dashboard/plantillas',
    },
  ]

  function navigateTo(route: string) {
    router.push(route)
  }
</script>

<style scoped>
  .dashboard-card {
    cursor: pointer;
    transition:
      transform 0.2s,
      box-shadow 0.2s;
    height: 100%;
  }

  .dashboard-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
  }

  .q-card__section {
    padding: 20px;
  }
</style>
