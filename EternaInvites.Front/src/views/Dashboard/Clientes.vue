<template>
  <div class="q-pa-md">
    <div class="row justify-between q-mb-md">
      <h2 class="text-h4 q-my-none">Clientes</h2>
      <q-btn color="primary" icon="add" label="Nuevo Cliente" @click="openDialog()" />
    </div>

    <!-- Search and filters -->
    <div class="row q-mb-md">
      <q-input
        v-model="search"
        debounce="300"
        outlined
        placeholder="Buscar clientes..."
        class="col-12 col-md-6">
        <template v-slot:append>
          <q-icon name="search" />
        </template>
      </q-input>
    </div>

    <!-- Table with loading state -->
    <q-table
      :rows="filteredClientes"
      :columns="columns"
      row-key="id"
      :loading="loading"
      :pagination="pagination"
      binary-state-sort>
      <template v-slot:loading>
        <q-inner-loading showing color="primary" />
      </template>

      <template v-slot:body-cell-actions="props">
        <q-td :props="props">
          <div class="q-gutter-sm">
            <q-btn
              icon="edit"
              color="amber"
              dense
              size="sm"
              round
              @click="editCliente(props.row)" />
            <q-btn
              icon="delete"
              color="negative"
              dense
              size="sm"
              round
              @click="confirmDelete(props.row)" />
          </div>
        </q-td>
      </template>
    </q-table>

    <!-- Dialog for add/edit -->
    <q-dialog v-model="dialog" persistent>
      <q-card style="min-width: 600px">
        <q-card-section class="row items-center bg-primary text-white">
          <div class="text-h6">{{ editMode ? 'Editar Cliente' : 'Nuevo Cliente' }}</div>
          <q-space />
          <q-btn icon="close" flat round dense v-close-popup />
        </q-card-section>

        <q-card-section class="q-pt-md">
          <q-form @submit="saveCliente" class="q-gutter-md">
            <!-- Información Personal -->
            <div class="text-subtitle1 q-mb-sm">Información Personal</div>
            <div class="row q-col-gutter-md">
              <div class="col-12 col-md-4">
                <q-input
                  v-model="currentCliente.nombre"
                  label="Nombre *"
                  :rules="[(val) => !!val || 'Nombre es requerido']"
                  outlined
                  dense />
              </div>
              <div class="col-12 col-md-4">
                <q-input
                  v-model="currentCliente.apellido"
                  label="Apellido *"
                  :rules="[(val) => !!val || 'Apellido es requerido']"
                  outlined
                  dense />
              </div>
              <div class="col-12 col-md-4">
                <q-input
                  v-model="currentCliente.apellidoPaterno"
                  label="Apellido Paterno"
                  outlined
                  dense />
              </div>
            </div>

            <!-- Información de Contacto -->
            <div class="text-subtitle1 q-mb-sm q-mt-md">Información de Contacto</div>
            <div class="row q-col-gutter-md">
              <div class="col-12 col-md-6">
                <q-input
                  v-model="currentCliente.correo"
                  label="Correo "
                  type="email"
                  :rules="[
                    (val) => {
                      if (val == '') return true
                      return /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(val)
                    },
                  ]"
                  outlined
                  dense>
                  <template v-slot:prepend>
                    <q-icon name="email" />
                  </template>
                </q-input>
              </div>
              <div class="col-12 col-md-6">
                <q-input v-model="currentCliente.telefono" label="Teléfono" outlined dense>
                  <template v-slot:prepend>
                    <q-icon name="phone" />
                  </template>
                </q-input>
              </div>
            </div>

            <div class="row q-col-gutter-md">
              <div class="col-12">
                <q-input v-model="currentCliente.direccion" label="Dirección" outlined dense>
                  <template v-slot:prepend>
                    <q-icon name="location_on" />
                  </template>
                </q-input>
              </div>
            </div>

            <!-- Información del Evento -->
            <div class="text-subtitle1 q-mb-sm q-mt-md">Información del Evento</div>
            <div class="row q-col-gutter-md">
              <div class="col-12 col-md-6">
                <q-input
                  v-model="currentCliente.nombreEvento"
                  label="Nombre del Evento *"
                  :rules="[(val) => !!val || 'Nombre del evento es requerido']"
                  outlined
                  dense>
                  <template v-slot:prepend>
                    <q-icon name="event" />
                  </template>
                </q-input>
              </div>
              <div class="col-12 col-md-6">
                <q-input
                  v-model="currentCliente.fechaEvento"
                  label="Fecha del Evento *"
                  :rules="[(val) => !!val || 'Fecha del evento es requerida']"
                  outlined
                  dense
                  readonly>
                  <template v-slot:prepend>
                    <q-icon name="event" />
                  </template>
                  <template v-slot:append>
                    <q-icon name="event" class="cursor-pointer">
                      <q-popup-proxy cover transition-show="scale" transition-hide="scale">
                        <q-date
                          v-model="currentCliente.fechaEvento"
                          mask="YYYY-MM-DD"
                          today-btn
                          :options="dateOptions" />
                      </q-popup-proxy>
                    </q-icon>
                  </template>
                </q-input>
              </div>
            </div>

            <!-- URL de invitación -->
            <div class="row q-col-gutter-md">
              <div class="col-12">
                <q-input
                  v-model="currentCliente.url"
                  label="URL de invitación *"
                  hint="URL personalizada para invitación digital"
                  :rules="[
                    (val) => !!val || 'URL es requerida',
                    () => !urlExists || 'Esta URL ya está en uso',
                  ]"
                  :loading="checkingUrl"
                  outlined
                  dense
                  @blur="checkUrlAvailability"
                  @input="urlManuallyEdited = true">
                  <template v-slot:prepend>
                    <q-icon name="link" />
                  </template>
                  <template v-slot:append>
                    <q-icon
                      v-if="!checkingUrl && !urlManuallyEdited"
                      name="autorenew"
                      class="cursor-pointer"
                      @click="generateUrl" />
                    <q-icon
                      v-if="!checkingUrl && currentCliente.url"
                      :name="urlExists ? 'error' : 'check_circle'"
                      :color="urlExists ? 'negative' : 'positive'" />
                  </template>
                </q-input>
              </div>
            </div>

            <!-- Descripción -->
            <div class="text-subtitle1 q-mb-sm q-mt-md">Detalles Adicionales</div>
            <div class="row q-col-gutter-md">
              <div class="col-12">
                <q-input
                  v-model="currentCliente.descripcion"
                  label="Descripción"
                  type="textarea"
                  rows="3"
                  outlined>
                  <template v-slot:prepend>
                    <q-icon name="description" />
                  </template>
                </q-input>
              </div>
            </div>

            <div class="row justify-end q-mt-lg">
              <q-btn label="Cancelar" color="grey" flat v-close-popup class="q-mr-sm" />
              <q-btn label="Guardar" type="submit" color="primary" />
            </div>
          </q-form>
        </q-card-section>
      </q-card>
    </q-dialog>

    <!-- Confirmation dialog for delete -->
    <q-dialog v-model="deleteDialog" persistent>
      <q-card>
        <q-card-section class="row items-center">
          <q-avatar icon="warning" color="negative" text-color="white" />
          <span class="q-ml-sm">¿Está seguro de eliminar este cliente?</span>
        </q-card-section>

        <q-card-actions align="right">
          <q-btn flat label="Cancelar" color="primary" v-close-popup />
          <q-btn flat label="Eliminar" color="negative" @click="deleteCliente" v-close-popup />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted, computed } from 'vue'
  import { QTable, useQuasar } from 'quasar'
  import { ClientesModel, ClientesService } from 'src/services/Clientes/ClientesService'

  const $q = useQuasar()
  const urlExists = ref(false)
  const checkingUrl = ref(false)
  const originalUrl = ref('')
  const urlManuallyEdited = ref(false)
  // Table data
  const clientes = ref<ClientesModel[]>([])
  const loading = ref(false)
  const search = ref('')
  const pagination = ref({
    sortBy: 'nombre',
    descending: false,
    page: 1,
    rowsPerPage: 10,
    rowsNumber: 0,
  })

  // Dialog controls
  const dialog = ref(false)
  const deleteDialog = ref(false)
  const editMode = ref(false)
  const currentCliente = ref<ClientesModel>({
    nombre: '',
    apellido: '',
    apellidoPaterno: '',
    url: '',
    correo: '',
    telefono: '',
    direccion: '',
    fechaEvento: new Date().toISOString().substr(0, 10),
    descripcion: '',
    nombreEvento: '',
  })
  const clienteToDelete = ref<ClientesModel | null>(null)

  // Table columns definition
  const columns: QTable['columns'] = [
    {
      name: 'nombre',
      required: true,
      label: 'Nombre',
      align: 'left',
      field: (row: ClientesModel) => `${row.nombre} ${row.apellido}`,
      sortable: true,
    },
    { name: 'correo', align: 'left', label: 'Correo', field: 'correo', sortable: true },
    { name: 'telefono', align: 'left', label: 'Teléfono', field: 'telefono' },
    { name: 'nombreEvento', align: 'left', label: 'Evento', field: 'nombreEvento' },
    {
      name: 'fechaEvento',
      align: 'left',
      label: 'Fecha',
      field: 'fechaEvento',
      format: (val: string) => new Date(val).toLocaleDateString(),
    },
    { name: 'actions', align: 'center', label: 'Acciones', field: 'actions' },
  ]

  // Filtered clients based on search
  const filteredClientes = computed(() => {
    if (!search.value) return clientes.value

    const searchLower = search.value.toLowerCase()
    return clientes.value.filter(
      (cliente) =>
        cliente.nombre.toLowerCase().includes(searchLower) ||
        cliente.apellido.toLowerCase().includes(searchLower) ||
        cliente.correo.toLowerCase().includes(searchLower) ||
        cliente.nombreEvento.toLowerCase().includes(searchLower),
    )
  })

  // Load clients on component mount
  onMounted(async () => {
    await fetchClientes()
  })

  // Fetch clients from API
  async function fetchClientes() {
    loading.value = true
    try {
      clientes.value = await ClientesService.GetAll()
      pagination.value.rowsNumber = clientes.value.length
    } catch (error) {
      $q.notify({
        color: 'negative',
        position: 'top',
        message: 'Error al cargar los clientes',
        icon: 'report_problem',
      })
    } finally {
      loading.value = false
    }
  }

  // Dialog functions
  function openDialog(cliente?: ClientesModel) {
    if (cliente) {
      currentCliente.value = { ...cliente }
      originalUrl.value = cliente.url // Save original URL
      editMode.value = true
    } else {
      currentCliente.value = {
        nombre: '',
        apellido: '',
        apellidoPaterno: '',
        url: '',
        correo: '',
        telefono: '',
        direccion: '',
        fechaEvento: new Date().toISOString().substr(0, 10),
        descripcion: '',
        nombreEvento: '',
      }
      originalUrl.value = ''
      editMode.value = false
    }
    urlExists.value = false // Reset URL check
    urlManuallyEdited.value = false // Reset URL manually edited flag
    dialog.value = true
  }

  function editCliente(cliente: ClientesModel) {
    openDialog(cliente)
  }

  function confirmDelete(cliente: ClientesModel) {
    clienteToDelete.value = cliente
    deleteDialog.value = true
  }

  // CRUD operations
  async function saveCliente() {
    loading.value = true
    try {
      if (editMode.value && currentCliente.value.id) {
        await ClientesService.Update(currentCliente.value)
        $q.notify({
          color: 'positive',
          position: 'top',
          message: 'Cliente actualizado correctamente',
          icon: 'check',
        })
      } else {
        await ClientesService.Insert(currentCliente.value)
        $q.notify({
          color: 'positive',
          position: 'top',
          message: 'Cliente creado correctamente',
          icon: 'check',
        })
      }
      dialog.value = false
      await fetchClientes()
    } catch (error) {
      $q.notify({
        color: 'negative',
        position: 'top',
        message: 'Error al guardar el cliente',
        icon: 'report_problem',
      })
    } finally {
      loading.value = false
    }
  }

  async function deleteCliente() {
    if (!clienteToDelete.value || !clienteToDelete.value.id) return

    loading.value = true
    try {
      await ClientesService.Delete(clienteToDelete.value.id)
      $q.notify({
        color: 'positive',
        position: 'top',
        message: 'Cliente eliminado correctamente',
        icon: 'check',
      })
      await fetchClientes()
    } catch (error) {
      $q.notify({
        color: 'negative',
        position: 'top',
        message: 'Error al eliminar el cliente',
        icon: 'report_problem',
      })
    } finally {
      loading.value = false
      clienteToDelete.value = null
    }
  }

  // Generate URL from name fields
  function generateUrl() {
    // Only auto-generate if URL hasn't been manually edited
    if (!urlManuallyEdited.value) {
      const nombre = currentCliente.value.nombre?.trim() || ''
      const apellido = currentCliente.value.apellido?.trim() || ''
      const apellidoPaterno = currentCliente.value.apellidoPaterno?.trim() || ''

      // Combine the fields, removing spaces and special characters
      if (nombre || apellido || apellidoPaterno) {
        const combined = (nombre + apellido + apellidoPaterno)
          .toLowerCase()
          .normalize('NFD') // Normalize accented characters
          .replace(/[\u0300-\u036f]/g, '') // Remove diacritics
          .replace(/[^a-z0-9]/g, '') // Remove non-alphanumeric characters

        currentCliente.value.url = combined
        // Check URL availability after generation
        checkUrlAvailability()
      }
    }
  }

  // Add this function to check URL availability
  async function checkUrlAvailability() {
    // Don't check if URL is empty or unchanged (in edit mode)
    if (
      !currentCliente.value.url ||
      (editMode.value && currentCliente.value.url === originalUrl.value)
    ) {
      urlExists.value = false
      return
    }

    checkingUrl.value = true
    try {
      // Assuming you'll add this method to your service
      // const exists = await ClientesService.CheckUrlExists(currentCliente.value.url)
      //urlExists.value = exists
    } catch (error) {
      // If there's an error, we'll assume the URL doesn't exist
      urlExists.value = false
    } finally {
      checkingUrl.value = false
    }
  }

  // Date options function to limit dates to today and future
  const dateOptions = (date: string) => {
    const today = new Date()
    today.setHours(0, 0, 0, 0)
    return new Date(date) >= today
  }
</script>

<style scoped>
  .q-table__card {
    box-shadow: 0 1px 5px rgba(0, 0, 0, 0.2);
    border-radius: 4px;
  }
</style>
