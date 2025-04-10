<template>
  <div class="invitation-container">
    <!-- Loading indicator -->
    <div v-if="loading" class="loading-container">
      <q-spinner color="primary" size="3em" />
      <p class="q-mt-md">Cargando invitación...</p>
    </div>

    <!-- Error message -->
    <div v-else-if="error" class="error-container">
      <q-icon name="error" color="negative" size="3em" />
      <p class="q-mt-md">{{ error }}</p>
      <q-btn color="primary" label="Reintentar" @click="fetchInvitacion" class="q-mt-md" />
    </div>

    <!-- Invitation HTML content -->
    <div v-else-if="invitacionHtml" v-html="invitacionHtml" class="invitation-content"></div>
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { useRoute } from 'vue-router'
  import { ClientesService } from 'src/services/Clientes/ClientesService'
  import { useQuasar } from 'quasar'
  import { AxiosError } from 'axios'

  const $q = useQuasar()
  const route = useRoute()
  const invitacionHtml = ref<string>('')
  const loading = ref<boolean>(true)
  const error = ref<string>('')

  // LocalStorage key pattern
  const getStorageKey = (url: string) => `invitacion_${url}`

  // Main function to get invitation data
  const fetchInvitacion = async () => {
    loading.value = true
    error.value = ''

    try {
      const url = route.params.id as string
      if (!url) {
        throw new Error('URL de invitación no válida')
      }

      // Check if we have this invitation in LocalStorage
      const storageKey = getStorageKey(url)
      const storedData = localStorage.getItem(storageKey)

      if (storedData) {
        invitacionHtml.value = storedData
      } else {
        const response = await ClientesService.ObtenerInivitacion(url)

        if (!response || !response.html) {
          throw new Error('No se encontró la invitación solicitada')
        }

        // Save HTML to LocalStorage
        localStorage.setItem(storageKey, response.html)
        invitacionHtml.value = response.html
      }
    } catch (err) {
      error.value = (err as AxiosError).message || 'Error al cargar la invitación'

      $q.notify({
        type: 'negative',
        message: error.value,
        position: 'top',
        timeout: 3000,
      })
    } finally {
      loading.value = false
    }
  }

  // On component mount, fetch invitation
  onMounted(() => {
    fetchInvitacion()
  })
</script>

<style scoped>
  .invitation-container {
    width: 100%;
    min-height: 100vh;
  }

  .loading-container,
  .error-container {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    min-height: 70vh;
    text-align: center;
    padding: 2rem;
  }
</style>
